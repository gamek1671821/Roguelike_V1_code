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

    public RoomDataSaver RoomDataSaver => roomDataSaver;
    RoomDataSaver roomDataSaver;
    public GodManager GodManager => godManager;
    GodManager godManager;

    [Header("位置控制")]
    public Transform generatorPoint_;
    public float x_Offset; //根據房間長寬改變
    public float z_Offset; //根據房間長寬改變
    public List<Room_Base> rooms = new List<Room_Base>();
    public int maxStep;
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
    public List<string> RoomType => roomType;
    public List<int> RoomId => roomId;
    public List<int> RoomLevel => roomLevel;

    public List<int> DataNomal_Id => dataNomal_Id;
    public List<int> DataNomal_Level => dataNomal_Level;

    public List<int> DataMiddle_Id => dataMiddle_Id;
    public List<int> DataMiddle_Level => dataMiddle_level;

    public List<int> DataEnd_Id => dataEnd_Id;
    public List<int> DataEnd_Level => dataEnd_Level;

    public List<int> DataGuardian_Id => dataGuardian_Id;
    public List<int> DataGuardian_Level => dataGuardian_Level;


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
    public List<int> allRoomLevel = new List<int>();

    public static event System.Action OnRoomGeneratorDone; // 初始化完成事件
    List<GameObject> path = new();

    void Start()
    {
        godManager = GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>();
        roomDataSaver = GetComponent<RoomDataSaver>();
        roomDataSaver.LoadRoomData();

        SetData();

        RoomGener_Funs.Instance.SetRandomSeed(this); //設定亂數種子
        RoomGener_Funs.Instance.SetAllRooms(this); //生成全部房間
        RoomGener_Funs.Instance.RG_SeparationType(this); //分離nomal房間的ID

        RoomGener_Funs.Instance.SetFirstRoom(this);//設定初始其他房間

        for (int i = 0; i < firstDestroyRoomsId.Count; i++) //關閉首先需要刪除的房間 (重疊的房間)
        {
            try
            {
                rooms[firstDestroyRoomsId[i]].gameObject.SetActive(false);
            }
            catch { }
        } //BUG

        RoomGener_Funs.Instance.SetOtherRoom(this); //設定其他房間
        RoomGener_Funs.Instance.FindMaxSetp(this); //尋找最大步數(int)
        RoomGener_Funs.Instance.FindMiddleRoom(this); //尋找中途房間
        RoomGener_Funs.Instance.FindEndRoom(this);//尋找最遠房間

        RoomGener_Funs.Instance.AStarPath(this); //使用 AStar 設定 最終房間 與 中途房間的路徑 
        RoomGener_Funs.Instance.TreasureRoom(this); //從可成為寶藏房選一
        RoomGener_Funs.Instance.TreasureRoomPath(this); //設定寶藏房路徑

        RoomGener_Funs.Instance.RoomDone(this); //設定 房間是否已經通關

        // 每個房間 設定關卡ID
        RoomGener_Funs.Instance.SetRoomLevel(this);

        PlayerMapSystem playerMapSystem = Instantiate(playerMapSystem_Prefab).GetComponent<PlayerMapSystem>();
        playerMapSystem.SetRoomGenerator(this);
        playerMapSystem.InitPlayerMapSystem(rooms[roomDataSaver.roomData.playerRoom].transform.position);

        OnRoomGeneratorDone?.Invoke();
    }
    private void SetData()
    {
        roomType = GetOneByType(); //指定 type 
        roomId = GetOneById();
        roomLevel = GetOneByLevel();
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

}
