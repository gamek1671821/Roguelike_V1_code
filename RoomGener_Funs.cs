using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static RoomGenerator;
public class RoomGener_Funs
{
    public static RoomGener_Funs Instance = new();
    List<GameObject> path = new();
    /// <summary>
    /// 分離nomal房間的ID
    /// </summary>
    public void RG_SeparationType(RoomGenerator RG)
    {
        for (int i = 0; i < RG.RoomType.Count; i++)
        {
            if (RG.RoomType[i] == "nomalRoom")
            {
                RG.DataNomal_Id.Add(RG.RoomId[i]);
                RG.DataNomal_Level.Add(RG.RoomLevel[i]);
            }
            else if (RG.RoomType[i] == "middleRoom")
            {
                RG.DataMiddle_Id.Add(RG.RoomId[i]);
                RG.DataMiddle_Level.Add(RG.RoomLevel[i]);
            }
            else if (RG.RoomType[i] == "endRoom")
            {
                RG.DataEnd_Id.Add(RG.RoomId[i]);
                RG.DataEnd_Level.Add(RG.RoomLevel[i]);
            }
            else if (RG.RoomType[i] == "guardianRoom")
            {
                RG.DataGuardian_Id.Add(RG.RoomId[i]);
                RG.DataGuardian_Level.Add(RG.RoomLevel[i]);
            }
        }
    }
    /// <summary>
    /// 設定亂數種子
    /// </summary>
    public void SetRandomSeed(RoomGenerator RG)
    {
        if (RG.DB.DB_mode)//如果是DB 
        {
            //DB種子不變
        }
        else if (!RG.RoomDataSaver.roomData.isNewData) //不是新檔案 讀取儲存的種子
        {
            RG.DB.DB_RandomSeed = RG.RoomDataSaver.roomData.RandomSeed;
        }
        else //不是DB狀態，但是是新存檔
        {
            RG.DB.DB_RandomSeed = (int)System.DateTime.Now.Ticks; //根據時間設一個隨機種子
            RG.RoomDataSaver.roomData.RandomSeed = RG.DB.DB_RandomSeed;
            //Debug.Log($"(int)System.DateTime.Now.Ticks : {(int)System.DateTime.Now.Ticks}");
        }
        Random.InitState(RG.DB.DB_RandomSeed); //設定隨機種子
    }
    /// <summary>
    /// 生成全部房間
    /// </summary>
    public void SetAllRooms(RoomGenerator RG)
    {
        for (int i = 0; i < RG.roomNumber_; i++)
        {
            RG.rooms.Add(Object.Instantiate(RG.roomPrefab_, RG.generatorPoint_.position, Quaternion.identity, RG.rooms_Father).GetComponent<Room_Base>());
            RG.CanDestroyRooms.Add(RG.rooms[i].gameObject); //紀錄所有房間
            ChagePoint_pos(i, RG);//改變生成點位
        }
    }
    private void ChagePoint_pos(int input, RoomGenerator RG) //input + 1 因為是下一個生成的點
    {
        Vector3 ori_pos = RG.generatorPoint_.position;
        int breakCount = 0;
        do
        {
            breakCount++;
            RG.generatorPoint_.position = ori_pos; //(當迴圈後) 彈回初始位置
            RG.direction = (Direction)Random.Range(0, 4);
            switch (RG.direction)
            {
                case (Direction.up):
                    RG.generatorPoint_.position += new Vector3(0, 0, RG.z_Offset);

                    break;
                case (Direction.down):
                    RG.generatorPoint_.position += new Vector3(0, 0, -RG.z_Offset);

                    break;
                case (Direction.left):
                    RG.generatorPoint_.position += new Vector3(-RG.x_Offset, 0, 0);

                    break;
                case (Direction.right):
                    RG.generatorPoint_.position += new Vector3(RG.x_Offset, 0, 0);

                    break;

            }
            if (breakCount >= 50)
            {
                //Debug.Log($"房間號碼{input + 1}的生成點，迴圈執行 {breakCount} 次，強制跳出");
                RG.firstDestroyRoomsId.Add(input + 1);
                break;
            }
        } while (Physics.OverlapSphere(RG.generatorPoint_.position, 0.2f, RG.roomLayer).Count() != 0);
    }
    /// <summary>
    /// 設定關卡ID
    /// </summary>
    /// <param name="input"></param>
    /// <param name="RG"></param>
    /// <returns></returns>
    public int SetLevelId(string input, RoomGenerator RG) // 設定關卡ID 
    {
        int randomData;
        int saveData = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
        switch (input)
        {
            case "End":
                do
                {
                    randomData = Random.Range(0, RG.DataEnd_Id.Count);
                } while (RG.DataEnd_Level[randomData] != saveData); // 這個地圖的等級 不等於 存檔的關卡等級。  重新骰
                RG.allRoomLevel.Add(RG.DataEnd_Id[randomData]);
                return RG.DataEnd_Id[randomData];
            case "Middle":
                do
                {
                    randomData = Random.Range(0, RG.DataMiddle_Id.Count);
                } while (RG.DataMiddle_Level[randomData] != saveData);  // 這個地圖的等級 必須等於 存檔的關卡等級。
                RG.allRoomLevel.Add(RG.DataMiddle_Id[randomData]);
                return RG.DataMiddle_Id[randomData];
            case "Guardian":
                do
                {
                    randomData = Random.Range(0, RG.DataGuardian_Id.Count);
                } while (RG.DataGuardian_Level[randomData] != saveData);  // 這個地圖的等級 必須等於 存檔的關卡等級。
                RG.allRoomLevel.Add(RG.DataGuardian_Id[randomData]);
                return RG.DataGuardian_Id[randomData];
            default:
                do
                {
                    randomData = Random.Range(0, RG.DataNomal_Id.Count);
                } while (RG.DataNomal_Level[randomData] != saveData); // 這個地圖的等級 必須等於 存檔的關卡等級。
                RG.allRoomLevel.Add(RG.DataNomal_Id[randomData]);
                return RG.DataNomal_Id[randomData];
        }
    }

    /// <summary>
    /// 設定初始房間
    /// </summary>
    /// <param name="RG"></param>
    public void SetFirstRoom(RoomGenerator RG)
    {
        RG.rooms[0].GetComponent<MeshRenderer>().material.color = RG.RC.startColor_; //初始房間上色
        var room_Base = RG.rooms[0].GetComponent<Room_Base>(); //獲取房間 (初始房間資訊)
        room_Base.type.Add(Room_Base.Type.firstRoom);//設定房間Type
        room_Base.roomId = 0;//設定房間ID
        RG.CanDestroyRooms.Remove(RG.rooms[0].gameObject); //刪除 初始房間  = 初始房間不可被隨機摧毀
    }
    /// <summary>
    /// 設定其他房間
    /// </summary>
    public void SetOtherRoom(RoomGenerator RG)
    {
        RG.endRoom = RG.rooms[0].gameObject; //先設定終點房間為初始房間 
        int foreachIndex = 0;
        foreach (var room in RG.rooms) //生成其餘房間
        {
            RoomGenerated_SetProperty(room, room.transform.position, RG);
            room.GetComponent<Room_Base>().roomId = foreachIndex;
            room.GetComponent<Room_Base>().roomRandomSeed = Random.Range(int.MinValue, int.MaxValue);
            foreachIndex++;
        }
    }
    /// <summary>
    /// 所有地城生成完，設定room的屬性
    /// (計算四方向的門、距離起點、房間start
    /// </summary>
    public void RoomGenerated_SetProperty(Room_Base newRoom, Vector3 roomPostion, RoomGenerator RG)
    {
        //根據room的四方向，設定room的bool 
        newRoom.roomUp = Physics.OverlapSphere(roomPostion + new Vector3(0, 0, RG.z_Offset), 0.2f, RG.roomLayer).Count() != 0;
        newRoom.roomDown = Physics.OverlapSphere(roomPostion + new Vector3(0, 0, -RG.z_Offset), 0.2f, RG.roomLayer).Count() != 0;
        newRoom.roomLeft = Physics.OverlapSphere(roomPostion + new Vector3(-RG.x_Offset, 0, 0), 0.2f, RG.roomLayer).Count() != 0;
        newRoom.roomRight = Physics.OverlapSphere(roomPostion + new Vector3(RG.x_Offset, 0, 0), 0.2f, RG.roomLayer).Count() != 0;
        // 方便呼叫
        newRoom.roomGenerator_ = RG;
        newRoom.set_StepToStart();
        newRoom.Room_Base_Start();
    }
    /// <summary>
    /// 從全部房間 尋找最遠步數
    /// </summary>
    public void FindMaxSetp(RoomGenerator RG)
    {
        //從全部房間 尋找最遠步數
        for (int i = 0; i < RG.rooms.Count; i++)
        {
            if (RG.rooms[i].stepToStart > RG.maxStep)
            {
                RG.maxStep = RG.rooms[i].stepToStart;
            }
        }
    }
    /// <summary>
    /// 尋找中途房間 上色、Type
    /// </summary>
    /// <param name="RG"></param>
    public void FindMiddleRoom(RoomGenerator RG)
    {
        foreach (var room in RG.rooms)
        {
            if (room.stepToStart == (int)RG.maxStep / 2)
                RG.middleRooms.Add(room.gameObject);
        }
        RG.middleRoom = RG.middleRooms[Random.Range(0, RG.middleRooms.Count)];

        RG.middleRoom.GetComponent<MeshRenderer>().material.color = RG.RC.middleColor; //中繼房間上色
        RG.middleRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.middleRoom); //設定房間Type
        RG.CanDestroyRooms.Remove(RG.middleRoom); // 刪除 中繼房間 = 中繼房間不可被隨機摧毀
    }
    /// <summary>
    /// 設定最終房間 上色、Type
    /// [必須] 房間屬性設定完成後
    /// </summary>
    public void FindEndRoom(RoomGenerator RG)
    {
        //從全部房間 尋找最遠步數

        //從全部房間 尋找最遠與次遠房間
        foreach (var room in RG.rooms)
        {
            if (room.stepToStart == RG.maxStep)
                RG.farRooms.Add(room.gameObject);
            if (room.stepToStart == RG.maxStep - 1)
                RG.lessFarRooms.Add(room.gameObject);
        }
        //從最遠房間 尋找單向房間
        foreach (var farRoom in RG.farRooms)
        {
            if (farRoom.GetComponent<Room_Base>().oriDoorCount == 1)
                RG.oneWayRooms.Add(farRoom);
        }
        //從次遠房間 尋找單向房間
        foreach (var lessRoom in RG.lessFarRooms)
        {
            if (lessRoom.GetComponent<Room_Base>().oriDoorCount == 1)
                RG.oneWayRooms.Add(lessRoom);
        }
        //設定 最終房間
        if (RG.oneWayRooms.Count != 0) //有單向房間，從中隨機設定
        {
            RG.endRoom = RG.oneWayRooms[Random.Range(0, RG.oneWayRooms.Count)];
        }
        else //沒單向房間，從最遠房間中隨機設定
        {
            RG.endRoom = RG.farRooms[Random.Range(0, RG.farRooms.Count)];
        }
        RG.endRoom.GetComponent<MeshRenderer>().material.color = RG.RC.endColor_; //最遠房間上色
        RG.endRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.endRoom); //設定房間Type
        RG.CanDestroyRooms.Remove(RG.endRoom); //刪除 結束房間
    }
    public void AStarPath(RoomGenerator RG)
    {
        AStar.AStar.Init(RG.x_Offset, RG.z_Offset, RG.roomLayer); //尋路系統
        path = AStar.AStar.AStarAlg(RG.rooms[0].transform.position, RG.middleRoom.transform.position);//把起點到終點之間的最短路徑找出
        foreach (var room in path)
        {
            if (room != RG.rooms[0].gameObject && room != RG.endRoom)
            {
                if (room != RG.middleRoom && room != RG.middleRoom)
                    room.GetComponent<MeshRenderer>().material.color = RG.RC.roadColor_1; //主幹道房間上色
                RG.roadRooms.Add(room);
                room.GetComponent<Room_Base>().type.Add(Room_Base.Type.roadRoom_ToMiddle); //設定房間Type
                RG.CanDestroyRooms.Remove(room); //刪除 道路房間
            }
        }
        path.Clear();
        path = AStar.AStar.AStarAlg(RG.middleRoom.transform.position, RG.endRoom.transform.position);
        foreach (var room in path)
        {
            if (room != RG.rooms[0].gameObject && room != RG.endRoom && room != RG.middleRoom)
            {
                if (room != RG.middleRoom && room != RG.middleRoom)
                    room.GetComponent<MeshRenderer>().material.color = RG.RC.roadColor_2; //主幹道房間上色
                RG.roadRooms.Add(room);
                room.GetComponent<Room_Base>().type.Add(Room_Base.Type.roadRoom_ToEnd); //設定房間Type
                RG.CanDestroyRooms.Remove(room); //刪除 道路房間
            }
        }
        path.Clear();
    }
    public void TreasureRoom(RoomGenerator RG)
    {
        foreach (var room in RG.rooms)
        {
            if (room != RG.rooms[0].gameObject && room != RG.endRoom)
            {
                FindCanBeTreasureRoom(room, RG);
            }
        }
        RandomChoseTreasureRoom(RG); //從可成為寶藏房選一
        RG.CanDestroyRooms.Remove(RG.treasureRoom); //刪除 寶藏房間
    }
    /// <summary>
    /// 尋找可以成為寶藏房間的房間
    /// </summary>
    private void FindCanBeTreasureRoom(Room_Base room_input, RoomGenerator RG)
    {
        Vector3 room_point = room_input.transform.position;
        Vector3 helf = new Vector3(2f * RG.x_Offset, 0.02f, 2f * RG.z_Offset);
        bool no_rood_room = FindRoadRoom(Physics.OverlapBox(room_point, helf, Quaternion.identity, RG.roomLayer), room_input.gameObject, RG);
        if (no_rood_room)
        {
            RG.CanBeTreasureRooms.Add(room_input.gameObject);
            room_input.GetComponent<MeshRenderer>().material.color = RG.RC.canBeTreasureColor;
        }
    }
    /// <summary>
    /// 尋找道路
    /// </summary>
    private bool FindRoadRoom(Collider[] input, GameObject room_self, RoomGenerator RG)
    {
        input = input.Where(val => val.gameObject != room_self).ToArray(); //刪除陣列中的 room_self

        if (input.Length != 0)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (RG.roadRooms.Contains(input[i].gameObject) || input[i].gameObject == RG.rooms[0].gameObject || input[i].gameObject == RG.middleRoom)
                { // 
                    return false;
                }
            }
            return true;
        }
        else
            return true;
    }
    private void RandomChoseTreasureRoom(RoomGenerator RG)
    {
        if (RG.CanBeTreasureRooms.Count != 0)
        {
            RG.treasureRoom = RG.CanBeTreasureRooms[Random.Range(0, RG.CanBeTreasureRooms.Count)];
            RG.treasureRoom.GetComponent<MeshRenderer>().material.color = RG.RC.treasureColor;
        }

    }
    public void TreasureRoomPath(RoomGenerator RG)
    {
        if (RG.treasureRoom != null)
        {
            RG.treasureRoom.GetComponent<Room_Base>().type.Add(Room_Base.Type.treasureRoom); //設定房間Type
            FindGuardianRoom(RG); //尋找守護者房間 

            //隨機在後半段道路尋找一點
            int RandomChoose = (int)Random.Range(RG.roadRooms.Count / 2, RG.roadRooms.Count);
            RG.roadRooms[RandomChoose].GetComponent<MeshRenderer>().material.color = RG.RC.treasureRoadEndColor;
            path = AStar.AStar.AStarAlg(RG.roadRooms[RandomChoose].transform.position, RG.treasureRoom.transform.position);
            foreach (var room in path)
            {
                if (room != RG.rooms[0].gameObject && room != RG.endRoom)
                {
                    if (!RG.guardianRooms.Contains(room) && !RG.roadRooms.Contains(room) && room != RG.treasureRoom && room != RG.middleRoom)
                        room.GetComponent<MeshRenderer>().material.color = RG.RC.treasureRoadColor; //寶藏道路房間上色
                    RG.treasureRoadRooms.Add(room);
                    room.GetComponent<Room_Base>().type.Add(Room_Base.Type.treasureRoadRoom);
                    RG.CanDestroyRooms.Remove(room); //刪除 寶藏道路房間
                }
            }
        }
        path.Clear();
    }
    private void FindGuardianRoom(RoomGenerator RG)
    {
        if (RG.treasureRoom != null)
        {
            Vector3 room_point = RG.treasureRoom.transform.position;
            Vector3 helf = new Vector3(RG.x_Offset, 0.02f, RG.z_Offset);
            Collider[] touchRooms = Physics.OverlapBox(room_point, helf, Quaternion.identity, RG.roomLayer);
            touchRooms = touchRooms.Where(val => val.gameObject != RG.treasureRoom).ToArray(); //除外 寶藏房間

            for (int i = 0; i < touchRooms.Length; i++)
            {
                if (!RG.roadRooms.Contains(touchRooms[i].gameObject) && touchRooms[i].gameObject != RG.rooms[0].gameObject && touchRooms[i].gameObject != RG.endRoom)
                { //不是 road /初始room/結束room
                    RG.guardianRooms.Add(touchRooms[i].gameObject);
                    touchRooms[i].GetComponent<MeshRenderer>().material.color = RG.RC.guardiaColor;
                    touchRooms[i].GetComponent<Room_Base>().type.Add(Room_Base.Type.guardianRoom);
                    RG.CanDestroyRooms.Remove(touchRooms[i].gameObject);
                }
            }
        }
    }
    public void RoomDone(RoomGenerator RG)
    {
        if (RG.RoomDataSaver.roomData.isNewData) //如果是新檔案
        {
            RG.RoomDataSaver.roomData.isClean.Clear();
            for (int i = 0; i < RG.rooms.Count; i++)
            {
                RG.RoomDataSaver.roomData.isClean.Add(false); //全部填入false
            }
            RG.RoomDataSaver.roomData.isNewData = false;
            RG.RoomDataSaver.SaveRoomData();
        }
        else //反之
        {
            for (int i = 0; i < RG.rooms.Count; i++)
            {
                RG.rooms[i].isClearRoom = RG.RoomDataSaver.roomData.isClean[i]; //根據存檔一一填入
            }
            if (RG.GodManager.battleWin) //如果是勝利狀態
            {
                RG.rooms[RG.GodManager.playerRoom].isClearRoom = true;
                RG.RoomDataSaver.roomData.isClean[RG.GodManager.playerRoom] = true;
                RG.RoomDataSaver.roomData.playerRoom = RG.GodManager.playerRoom;
                RG.RoomDataSaver.SaveRoomData();
            }
        }

        if (RG.RoomDataSaver.roomData.isClean.Count == RG.rooms.Count)
        {
            for (int i = 0; i < RG.RoomDataSaver.roomData.isClean.Count; i++)
            {
                RG.rooms[i].isClearRoom = RG.RoomDataSaver.roomData.isClean[i];
            }
        }
    }
    /// <summary>
    /// 每個房間 設定關卡ID與Icon
    /// </summary>
    /// <param name="RG"></param>
    public void SetRoomLevel(RoomGenerator RG)
    {
        for (int i = 1; i < RG.rooms.Count; i++)
        {
            if (RG.rooms[i].type.Contains(Room_Base.Type.endRoom) && RG.rooms[i].levelId == 0)
            {
                RG.rooms[i].levelId = SetLevelId("End", RG);
            }
            else if (RG.rooms[i].type.Contains(Room_Base.Type.middleRoom) && RG.rooms[i].levelId == 0)
            {
                RG.rooms[i].levelId = SetLevelId("Middle", RG);
            }
            else if (RG.rooms[i].type.Contains(Room_Base.Type.guardianRoom) && RG.rooms[i].levelId == 0)
            {
                RG.rooms[i].levelId = SetLevelId("Guardian", RG);
            }
            else if (RG.rooms[i].levelId == 0) //最後才設定
            {
                RG.rooms[i].levelId = SetLevelId("Nomal", RG);
            }
        }
        foreach (var room in RG.rooms)
        {
            room.SetIcon();
        }
    }
    
    public void SetRoomDestroy(RoomGenerator RG)
    {
        int destroyCount = RG.CanDestroyRooms.Count / 10;
        int destroyed = 0;

        List<GameObject> availableRooms = new List<GameObject>(RG.CanDestroyRooms);
        RG.DestroyedRooms.Clear(); // 清空舊記錄

        while (destroyed < destroyCount && availableRooms.Count > 0)
        {
            int randomPick = Random.Range(0, availableRooms.Count);
            GameObject roomObj = availableRooms[randomPick];
            Room_Base pickRoom = roomObj.GetComponent<Room_Base>();
            availableRooms.RemoveAt(randomPick);

            if (!roomObj.activeSelf || pickRoom.oriDoorCount <= 2) continue;
            if (!CanDestroyRoom(pickRoom, RG)) continue;

            // 模擬刪除並檢查連通性
            roomObj.SetActive(false);
            bool stillConnected = IsMapConnected(RG);
            roomObj.SetActive(true);

            if (stillConnected)
            {
                CloseAdjacentDoors(pickRoom, RG);
                roomObj.SetActive(false);
                RG.DestroyedRooms.Add(roomObj);
                destroyed++;
            }
        }

        Debug.Log($"刪除成功：{destroyed} 間房間（目標：{destroyCount}）");
    }
    private bool IsMapConnected(RoomGenerator RG)
    {
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        // 找第一個啟用的房間作為起點
        GameObject startRoom = RG.CanDestroyRooms.Find(r => r.activeSelf);
        if (startRoom == null) return false;

        queue.Enqueue(startRoom);
        visited.Add(startRoom);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            Room_Base room = current.GetComponent<Room_Base>();

            // 往四個方向探索
            ExploreNeighbor(room, RG, Vector3.forward * RG.z_Offset, visited, queue, room.doorUp_ison);
            ExploreNeighbor(room, RG, Vector3.back * RG.z_Offset, visited, queue, room.doorDown_ison);
            ExploreNeighbor(room, RG, Vector3.left * RG.x_Offset, visited, queue, room.doorLeft_ison);
            ExploreNeighbor(room, RG, Vector3.right * RG.x_Offset, visited, queue, room.doorRight_ison);
        }

        // 只要能訪問到所有啟用房間，就算連通
        int activeCount = RG.CanDestroyRooms.Count(r => r.activeSelf);
        return visited.Count >= activeCount;
    }
    private void ExploreNeighbor(Room_Base fromRoom, RoomGenerator RG, Vector3 offset, HashSet<GameObject> visited, Queue<GameObject> queue, bool hasDoor)
    {
        if (!hasDoor) return;

        Vector3 checkPos = MyFuns.Instance.T2V(fromRoom.transform) + offset;
        Collider[] hits = Physics.OverlapSphere(checkPos, 0.2f, RG.roomLayer);

        if (hits.Length > 0)
        {
            GameObject neighborObj = hits[0].gameObject;
            if (neighborObj.activeSelf && !visited.Contains(neighborObj))
            {
                visited.Add(neighborObj);
                queue.Enqueue(neighborObj);
            }
        }
    }

    private bool CanDestroyRoom(Room_Base room, RoomGenerator RG)
    {
        Vector3 pos = MyFuns.Instance.T2V(room.transform);

        bool upOK = !room.doorUp_ison || GetRoomAt(pos + Vector3.forward * RG.z_Offset, RG)?.oriDoorCount > 2;
        bool downOK = !room.doorDown_ison || GetRoomAt(pos + Vector3.back * RG.z_Offset, RG)?.oriDoorCount > 2;
        bool leftOK = !room.doorLeft_ison || GetRoomAt(pos + Vector3.left * RG.x_Offset, RG)?.oriDoorCount > 2;
        bool rightOK = !room.doorRight_ison || GetRoomAt(pos + Vector3.right * RG.x_Offset, RG)?.oriDoorCount > 2;

        return upOK && downOK && leftOK && rightOK;
    }
    private Room_Base GetRoomAt(Vector3 pos, RoomGenerator RG)
    {
        Collider[] hits = Physics.OverlapSphere(pos, 0.2f, RG.roomLayer);
        if (hits.Length > 0)
            return hits[0].GetComponent<Room_Base>();
        return null;
    }
    private void CloseAdjacentDoors(Room_Base room, RoomGenerator RG)
    {
        Vector3 pos = MyFuns.Instance.T2V(room.transform);

        if (room.doorUp_ison)
        {
            Room_Base r = GetRoomAt(pos + Vector3.forward * RG.z_Offset, RG);
            if (r != null) r.doorDown_ison = false;
        }
        if (room.doorDown_ison)
        {
            Room_Base r = GetRoomAt(pos + Vector3.back * RG.z_Offset, RG);
            if (r != null) r.doorUp_ison = false;
        }
        if (room.doorLeft_ison)
        {
            Room_Base r = GetRoomAt(pos + Vector3.left * RG.x_Offset, RG);
            if (r != null) r.doorRight_ison = false;
        }
        if (room.doorRight_ison)
        {
            Room_Base r = GetRoomAt(pos + Vector3.right * RG.x_Offset, RG);
            if (r != null) r.doorLeft_ison = false;
        }

        // 自己的門也要關
        room.doorUp_ison = false;
        room.doorDown_ison = false;
        room.doorLeft_ison = false;
        room.doorRight_ison = false;
    }


    // private void TryCloseNeighborDoor(Room_Base room, RoomGenerator RG, bool hasDoor, Vector3 offset, System.Action<Room_Base> closeAction)
    // {
    //     if (!hasDoor) return;

    //     Vector3 checkPos = MyFuns.Instance.T2V(room.transform) + offset;
    //     Collider[] hits = Physics.OverlapSphere(checkPos, 0.2f, RG.roomLayer);
    //     if (hits.Length == 0) return;

    //     Room_Base neighbor = hits[0].GetComponent<Room_Base>();
    //     closeAction(neighbor);
    // }
    // private bool CheckNeighborDoor(Room_Base room, RoomGenerator RG, bool hasDoor, Vector3 offset)
    // {
    //     if (!hasDoor) return true;

    //     Vector3 checkPos = MyFuns.Instance.T2V(room.transform) + offset;
    //     Collider[] hits = Physics.OverlapSphere(checkPos, 0.2f, RG.roomLayer);
    //     if (hits.Length == 0) return false;

    //     Room_Base neighbor = hits[0].GetComponent<Room_Base>();
    //     return neighbor.oriDoorCount > 2;
    // }

}
