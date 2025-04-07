using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDataSaver : MonoBehaviour
{
    public RoomData roomData;

    [System.Serializable]
    public class RoomData
    {
        public bool isNewData = true;
        public int playerRoom = 0; //玩家最後所在位置
        public List<bool> isClean = new List<bool>();
        public int RandomSeed;
        public static RoomData InitRoomData()
        { //如果沒有資料，設定初始資料
            var roomData = new RoomData();
            roomData.isNewData = true;
            roomData.playerRoom = 0;
            roomData.isClean.Add(true);
            roomData.RandomSeed = 0;
            return roomData;
        }
    }

    public void Awake() //避免資料是空的
    {
        LoadRoomData();
        if (roomData == null) //
        {
            roomData = RoomData.InitRoomData();
            SaveRoomData();
        }
    }
    public void LoadRoomData()
    {
        roomData = JsonUtility.FromJson<RoomData>(PlayerPrefs.GetString("RoomDataSaver" + GodManager.Instance.SaveData_ID));
    }
    public void SaveRoomData()
    {
        PlayerPrefs.SetString("RoomDataSaver" + GodManager.Instance.SaveData_ID, JsonUtility.ToJson(roomData));
    }
    public void cleanRoomData()
    {
        roomData = RoomData.InitRoomData();
        SaveRoomData();
    }

}
