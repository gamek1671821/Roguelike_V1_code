using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 房間生成器
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;
    [Header("房間訊息")]
    public GameObject roomPrefab_;
    public GameObject playerMapSystem_Prefab;
    public int roomNumber_;

    RoomDataSaver roomDataSaver;
    GodManager godManager;

    [Header("位置控制")]
    public Transform generatorPoint_;
    public float x_Offset; //根據房間長寬改變
    public float z_Offset; //根據房間長寬改變
    public List<Room_Base> rooms = new List<Room_Base>();
    private int maxStep;
    public List<GameObject> middleRooms = new List<GameObject>();
    public List<GameObject> farRooms = new List<GameObject>();
    public List<GameObject> lessFarRooms = new List<GameObject>();
    public List<GameObject> oneWayRooms = new List<GameObject>();
    public List<GameObject> roadRooms = new List<GameObject>();
    public List<GameObject> CanBeTreasureRooms = new List<GameObject>();
    public List<GameObject> CanDestroyRooms = new List<GameObject>(); //可以被隨機摧毀的房間
    public List<int> firstDestroyRoomsId = new List<int>(); //首先 重複的必須刪除
    public List<GameObject> guardianRooms = new List<GameObject>();
    public List<GameObject> treasureRoadRooms = new List<GameObject>();
    public LayerMask roomLayer; //
    public GameObject endRoom, middleRoom, treasureRoom;
    public Transform rooms_Father;
    [System.Serializable]
    public class room_Color
    {
        public Color startColor_, endColor_, middleColor, roadColor_1, roadColor_2, canBeTreasureColor, treasureColor, guardiaColor, treasureRoadColor, treasureRoadEndColor;
    }
    public room_Color RC;

    public DB_Control DB;
    [System.Serializable]
    public class DB_Control
    {
        [Header("勾選，種子固定")]
        public bool DB_mode;
        public int DB_RandomSeed;
    }
    private List<string> roomType = new List<string>();
    private List<int> roomId = new List<int>();
    private List<int> roomLevel = new List<int>();
    private List<int> dataNomal_Id = new List<int>(); //全部的 nomalRoom (Type)
    private List<int> dataNomal_Level = new List<int>(); //全部 nomalRoom 的Level
    private List<int> dataMiddle_Id = new List<int>(); //全部的 middleRoom (Type)
    private List<int> dataMiddle_level = new List<int>(); //全部 middleRoom 的Level
    private List<int> dataEnd_Id = new List<int>(); //全部的 endRoom (Type)
    private List<int> dataEnd_Level = new List<int>(); //全部 endRoom 的Level
    private List<int> dataGuardian_Id = new List<int>(); //全部的 guardianRoom (Type)
    private List<int> dataGuardian_Level = new List<int>(); //全部 endRoom 的Level

    public static event System.Action OnRoomGeneratorDone; // 初始化完成事件

    private void SeparationType()
    {
        for (int i = 0; i < roomType.Count; i++)
        {
            if (roomType[i] == "nomalRoom")
            {
                dataNomal_Id.Add(roomId[i]);
                dataNomal_Level.Add(roomLevel[i]);
            }
            else if (roomType[i] == "middleRoom")
            {
                dataMiddle_Id.Add(roomId[i]);
                dataMiddle_level.Add(roomLevel[i]);
            }
            else if (roomType[i] == "endRoom")
            {
                dataEnd_Id.Add(roomId[i]);
                dataEnd_Level.Add(roomLevel[i]);
            }
            else if (roomType[i] == "guardianRoom")
            {
                dataGuardian_Id.Add(roomId[i]);
                dataGuardian_Level.Add(roomLevel[i]);
            }

        }
    }
    private void SetRandomSeed()
    {
        if (DB.DB_mode)//如果是DB 
        {
            //DB種子不變
        }
        else if (!roomDataSaver.roomData.isNewData) //不是新檔案 讀取儲存的種子
        {
            DB.DB_RandomSeed = roomDataSaver.roomData.RandomSeed;
        }
        else //不是DB狀態，但是是新存檔
        {
            DB.DB_RandomSeed = (int)System.DateTime.Now.Ticks; //根據時間設一個隨機種子
            roomDataSaver.roomData.RandomSeed = DB.DB_RandomSeed;
            //Debug.Log($"(int)System.DateTime.Now.Ticks : {(int)System.DateTime.Now.Ticks}");
        }
        Random.InitState(DB.DB_RandomSeed); //設定隨機種子
    }
    private void SetAllRooms()
    {
        for (int i = 0; i < roomNumber_; i++)
        {
            rooms.Add(Instantiate(roomPrefab_, generatorPoint_.position, Quaternion.identity, rooms_Father).GetComponent<Room_Base>());
            CanDestroyRooms.Add(rooms[i].gameObject); //紀錄所有房間
            ChagePoint_pos(i);//改變生成點位
        }
    }
    void Start()
    {
        godManager = GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>();
        roomDataSaver = GetComponent<RoomDataSaver>();
        roomDataSaver.LoadRoomData();

        SetData();

        SetRandomSeed(); //設定亂數種子
        SetAllRooms(); //生成全部房間
        SeparationType(); //分離nomal房間的ID

        rooms[0].GetComponent<MeshRenderer>().material.color = RC.startColor_; //初始房間上色
        var room_Base = rooms[0].GetComponent<Room_Base>(); //獲取房間 (初始房間資訊)
        room_Base.type.Add(Room_Base.Type.firstRoom);//設定房間Type
        room_Base.roomId = 0;//設定房間ID
        CanDestroyRooms.Remove(rooms[0].gameObject); //刪除 初始房間  = 初始房間不可被隨機摧毀

        for (int i = 0; i < firstDestroyRoomsId.Count; i++) //關閉首先需要刪除的房間 (重疊的房間)
        {
            try
            {
                rooms[firstDestroyRoomsId[i]].gameObject.SetActive(false);
            }
            catch { }
        } //BUG

        endRoom = rooms[0].gameObject; //先設定終點房間為初始房間 
        int foreachIndex = 0;
        foreach (var room in rooms) //生成其餘房間
        {
            RoomGenerated_SetProperty(room, room.transform.position);
            room.GetComponent<Room_Base>().roomId = foreachIndex;
            room.GetComponent<Room_Base>().roomRandomSeed = Random.Range(int.MinValue, int.MaxValue);
            foreachIndex++;
        }
        FindMaxSetp(); //尋找最大步數(int)
        FindMiddleRoom(); //尋找中途房間
        FindEndRoom();
        middleRoom.GetComponent<MeshRenderer>().material.color = RC.middleColor; //中繼房間上色
        middleRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.middleRoom); //設定房間Type
        endRoom.GetComponent<MeshRenderer>().material.color = RC.endColor_; //最遠房間上色
        endRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.endRoom); //設定房間Type
        CanDestroyRooms.Remove(middleRoom); // 刪除 中繼房間 = 中繼房間不可被隨機摧毀
        CanDestroyRooms.Remove(endRoom); //刪除 結束房間

        AStar.AStar.Init(x_Offset, z_Offset, roomLayer); //尋路系統
        List<GameObject> path = AStar.AStar.AStarAlg(rooms[0].transform.position, middleRoom.transform.position);//把起點到終點之間的最短路徑找出
        foreach (var room in path)
        {
            if (room != rooms[0].gameObject && room != endRoom)
            {
                if (room != middleRoom && room != middleRoom)
                    room.GetComponent<MeshRenderer>().material.color = RC.roadColor_1; //主幹道房間上色
                roadRooms.Add(room);
                room.GetComponent<Room_Base>().type.Add(Room_Base.Type.roadRoom_ToMiddle); //設定房間Type
                CanDestroyRooms.Remove(room); //刪除 道路房間
            }
        }
        path.Clear();
        path = AStar.AStar.AStarAlg(middleRoom.transform.position, endRoom.transform.position);
        foreach (var room in path)
        {
            if (room != rooms[0].gameObject && room != endRoom && room != middleRoom)
            {
                if (room != middleRoom && room != middleRoom)
                    room.GetComponent<MeshRenderer>().material.color = RC.roadColor_2; //主幹道房間上色
                roadRooms.Add(room);
                room.GetComponent<Room_Base>().type.Add(Room_Base.Type.roadRoom_ToEnd); //設定房間Type
                CanDestroyRooms.Remove(room); //刪除 道路房間
            }
        }
        path.Clear();
        foreach (var room in rooms)
        {
            if (room != rooms[0].gameObject && room != endRoom)
            {
                FindCanBeTreasureRoom(room);
            }
        }
        RandomChoseTreasureRoom(); //從可成為寶藏房選一
        CanDestroyRooms.Remove(treasureRoom); //刪除 寶藏房間
        if (treasureRoom != null)
        {
            treasureRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.treasureRoom); //設定房間Type
            FindGuardianRoom(); //尋找守護者房間 

            //隨機在後半段道路尋找一點
            int RandomChoose = (int)Random.Range(roadRooms.Count / 2, roadRooms.Count);
            roadRooms[RandomChoose].GetComponent<MeshRenderer>().material.color = RC.treasureRoadEndColor;
            path = AStar.AStar.AStarAlg(roadRooms[RandomChoose].transform.position, treasureRoom.transform.position);
            foreach (var room in path)
            {
                if (room != rooms[0].gameObject && room != endRoom)
                {
                    if (!guardianRooms.Contains(room) && !roadRooms.Contains(room) && room != treasureRoom && room != middleRoom)
                        room.GetComponent<MeshRenderer>().material.color = RC.treasureRoadColor; //寶藏道路房間上色
                    treasureRoadRooms.Add(room);
                    room.GetComponent<Room_Base>().type.Add(Room_Base.Type.treasureRoadRoom);
                    CanDestroyRooms.Remove(room); //刪除 寶藏道路房間
                }
            }
        }
        path.Clear();
        //讀取房間是否被完成
        if (roomDataSaver.roomData.isNewData) //如果是新檔案
        {
            roomDataSaver.roomData.isClean.Clear();
            for (int i = 0; i < rooms.Count; i++)
            {
                roomDataSaver.roomData.isClean.Add(false); //全部填入false
            }
            roomDataSaver.roomData.isNewData = false;
            roomDataSaver.SaveRoomData();
        }
        else //反之
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                rooms[i].isClearRoom = roomDataSaver.roomData.isClean[i]; //根據存檔一一填入
            }
            if (godManager.battleWin) //如果是勝利狀態
            {
                rooms[godManager.playerRoom].isClearRoom = true;
                roomDataSaver.roomData.isClean[godManager.playerRoom] = true;
                roomDataSaver.roomData.playerRoom = godManager.playerRoom;
                roomDataSaver.SaveRoomData();
            }
        }

        if (roomDataSaver.roomData.isClean.Count == rooms.Count)
        {
            for (int i = 0; i < roomDataSaver.roomData.isClean.Count; i++)
            {
                rooms[i].isClearRoom = roomDataSaver.roomData.isClean[i];
            }
        }
        // 每個房間 設定關卡ID
        for (int i = 1; i < rooms.Count; i++)
        {
            if (rooms[i].type.Contains(Room_Base.Type.endRoom) && rooms[i].levelId == 0)
            {
                rooms[i].levelId = SetLevelId("End");
            }
            else if (rooms[i].type.Contains(Room_Base.Type.middleRoom) && rooms[i].levelId == 0)
            {
                rooms[i].levelId = SetLevelId("Middle");
            }
            else if (rooms[i].type.Contains(Room_Base.Type.guardianRoom) && rooms[i].levelId == 0)
            {
                rooms[i].levelId = SetLevelId("Guardian");
            }
            else if (rooms[i].levelId == 0) //最後才設定
            {
                rooms[i].levelId = SetLevelId("Nomal");
            }
        }
        foreach (var room in rooms)
        {
            room.SetIcon();
        }

        PlayerMapSystem playerMapSystem = Instantiate(playerMapSystem_Prefab).GetComponent<PlayerMapSystem>();
        playerMapSystem.SetRoomGenerator(this);
        playerMapSystem.InitPlayerMapSystem(rooms[roomDataSaver.roomData.playerRoom].transform.position);

        OnRoomGeneratorDone?.Invoke();
    }
    private int SetLevelId(string input) // 設定關卡ID 
    {
        int randomData;
        int saveData = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
        switch (input)
        {
            case "End":
                do
                {
                    randomData = Random.Range(0, dataEnd_Id.Count);
                } while (dataEnd_Level[randomData] != saveData); // 這個地圖的等級 不等於 存檔的關卡等級。  重新骰
                return dataEnd_Id[randomData];
            case "Middle":
                do
                {
                    randomData = Random.Range(0, dataMiddle_Id.Count);
                } while (dataMiddle_level[randomData] != saveData);  // 這個地圖的等級 必須等於 存檔的關卡等級。
                return dataMiddle_Id[randomData];
            case "Guardian":
                do
                {
                    randomData = Random.Range(0, dataGuardian_Id.Count);
                } while (dataGuardian_Level[randomData] != saveData);  // 這個地圖的等級 必須等於 存檔的關卡等級。
                return dataGuardian_Id[randomData];
            default:
                do
                {
                    randomData = Random.Range(0, dataNomal_Id.Count);
                } while (dataNomal_Level[randomData] != saveData); // 這個地圖的等級 必須等於 存檔的關卡等級。
                return dataNomal_Id[randomData];
        }
    }
    private void FindGuardianRoom()
    {
        if (treasureRoom != null)
        {
            Vector3 room_point = treasureRoom.transform.position;
            Vector3 helf = new Vector3(x_Offset, 0.02f, z_Offset);
            Collider[] touchRooms = Physics.OverlapBox(room_point, helf, Quaternion.identity, roomLayer);
            touchRooms = touchRooms.Where(val => val.gameObject != treasureRoom).ToArray(); //除外 寶藏房間

            for (int i = 0; i < touchRooms.Length; i++)
            {
                if (!roadRooms.Contains(touchRooms[i].gameObject) && touchRooms[i].gameObject != rooms[0].gameObject && touchRooms[i].gameObject != endRoom)
                { //不是 road /初始room/結束room
                    guardianRooms.Add(touchRooms[i].gameObject);
                    touchRooms[i].GetComponent<MeshRenderer>().material.color = RC.guardiaColor;
                    touchRooms[i].GetComponent<Room_Base>().type.Add(Room_Base.Type.guardianRoom);
                    CanDestroyRooms.Remove(touchRooms[i].gameObject);
                }
            }
        }
    }
    private void RandomChoseTreasureRoom()
    {
        if (CanBeTreasureRooms.Count != 0)
        {
            treasureRoom = CanBeTreasureRooms[Random.Range(0, CanBeTreasureRooms.Count)];
            treasureRoom.GetComponent<MeshRenderer>().material.color = RC.treasureColor;
        }

    }
    private bool FindRoadRoom(Collider[] input, GameObject room_self)
    {
        input = input.Where(val => val.gameObject != room_self).ToArray(); //刪除陣列中的 room_self

        if (input.Length != 0)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (roadRooms.Contains(input[i].gameObject) || input[i].gameObject == rooms[0].gameObject || input[i].gameObject == middleRoom)
                { // 
                    return false;
                }
            }
            return true;
        }
        else
            return true;
    }
    private void FindCanBeTreasureRoom(Room_Base room_input)
    {
        Vector3 room_point = room_input.transform.position;
        Vector3 helf = new Vector3(2f * x_Offset, 0.02f, 2f * z_Offset);
        bool no_rood_room = FindRoadRoom(Physics.OverlapBox(room_point, helf, Quaternion.identity, roomLayer), room_input.gameObject);
        if (no_rood_room)
        {
            CanBeTreasureRooms.Add(room_input.gameObject);
            room_input.GetComponent<MeshRenderer>().material.color = RC.canBeTreasureColor;
        }
    }

    private void ChagePoint_pos(int input) //input + 1 因為是下一個生成的點
    {
        Vector3 ori_pos = generatorPoint_.position;
        int breakCount = 0;
        do
        {
            breakCount++;
            generatorPoint_.position = ori_pos; //(當迴圈後) 彈回初始位置
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case (Direction.up):
                    generatorPoint_.position += new Vector3(0, 0, z_Offset);

                    break;
                case (Direction.down):
                    generatorPoint_.position += new Vector3(0, 0, -z_Offset);

                    break;
                case (Direction.left):
                    generatorPoint_.position += new Vector3(-x_Offset, 0, 0);

                    break;
                case (Direction.right):
                    generatorPoint_.position += new Vector3(x_Offset, 0, 0);

                    break;

            }
            if (breakCount >= 50)
            {
                //Debug.Log($"房間號碼{input + 1}的生成點，迴圈執行 {breakCount} 次，強制跳出");
                firstDestroyRoomsId.Add(input + 1);
                break;
            }
        } while (Physics.OverlapSphere(generatorPoint_.position, 0.2f, roomLayer).Count() != 0);
    }
    /// <summary>
    /// 所有地城生成完，設定room的屬性
    /// (計算四方向的門、距離起點、房間start
    /// </summary>
    private void RoomGenerated_SetProperty(Room_Base newRoom, Vector3 roomPostion)
    {
        //根據room的四方向，設定room的bool 
        newRoom.roomUp = Physics.OverlapSphere(roomPostion + new Vector3(0, 0, z_Offset), 0.2f, roomLayer).Count() != 0;
        newRoom.roomDown = Physics.OverlapSphere(roomPostion + new Vector3(0, 0, -z_Offset), 0.2f, roomLayer).Count() != 0;
        newRoom.roomLeft = Physics.OverlapSphere(roomPostion + new Vector3(-x_Offset, 0, 0), 0.2f, roomLayer).Count() != 0;
        newRoom.roomRight = Physics.OverlapSphere(roomPostion + new Vector3(x_Offset, 0, 0), 0.2f, roomLayer).Count() != 0;
        // 方便呼叫
        newRoom.roomGenerator_ = this;
        newRoom.set_StepToStart();
        newRoom.Room_Base_Start();
    }
    //從全部房間 尋找最遠步數
    private void FindMaxSetp()
    {
        //從全部房間 尋找最遠步數
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
            {
                maxStep = rooms[i].stepToStart;
            }
        }
    }

    /// <summary>
    /// 設定最終房間
    /// [必須] 房間屬性設定完成後
    /// </summary>
    private void FindEndRoom()
    {
        //從全部房間 尋找最遠步數

        //從全部房間 尋找最遠與次遠房間
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }
        //從最遠房間 尋找單向房間
        foreach (var farRoom in farRooms)
        {
            if (farRoom.GetComponent<Room_Base>().oriDoorCount == 1)
                oneWayRooms.Add(farRoom);
        }
        //從次遠房間 尋找單向房間
        foreach (var lessRoom in lessFarRooms)
        {
            if (lessRoom.GetComponent<Room_Base>().oriDoorCount == 1)
                oneWayRooms.Add(lessRoom);
        }
        //設定 最終房間
        if (oneWayRooms.Count != 0) //有單向房間，從中隨機設定
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else //沒單向房間，從最遠房間中隨機設定
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
    private void FindMiddleRoom()
    {
        foreach (var room in rooms)
        {
            if (room.stepToStart == (int)maxStep / 2)
                middleRooms.Add(room.gameObject);
        }
        middleRoom = middleRooms[Random.Range(0, middleRooms.Count)];
    }
    private List<string> GetOneByType()
    {
        List<Dictionary<string, string>> dataDic = new List<Dictionary<string, string>>(GameConfigManager.Instance.GetlevelLines());
        List<string> list = new List<string>();

        for (int i = 0; i < dataDic.Count; i++)
        {
            list.Add(dataDic[i]["Type"]);
        }
        return list;
    }
    private List<int> GetOneById()
    {
        List<Dictionary<string, string>> dataDic = new List<Dictionary<string, string>>(GameConfigManager.Instance.GetlevelLines());
        List<string> list = new List<string>();

        for (int i = 0; i < dataDic.Count; i++)
        {
            list.Add(dataDic[i]["Id"]);
        }
        List<int> intList = list
            .Where(str => int.TryParse(str, out _)) // 過濾出有效的數字字串
            .Select(int.Parse) // 轉換為整數
            .ToList();
        return intList;
    }
    private List<int> GetOneByLevel()
    {
        List<Dictionary<string, string>> dataDic = new List<Dictionary<string, string>>(GameConfigManager.Instance.GetlevelLines());
        List<string> list = new List<string>();

        for (int i = 0; i < dataDic.Count; i++)
        {
            list.Add(dataDic[i]["Level"]);
        }
        List<int> intList = list
            .Where(str => int.TryParse(str, out _)) // 過濾出有效的數字字串
            .Select(int.Parse) // 轉換為整數
            .ToList();
        return intList;
    }
    private void SetData()
    {
        roomType = GetOneByType(); //指定 type 
        roomId = GetOneById();
        roomLevel = GetOneByLevel();
    }

}
