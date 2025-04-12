using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//git hub test
public class Room_Base : MonoBehaviour
{
    public enum Type
    {
        nomalRoom, //一般房間
        firstRoom, //初始房間
        middleRoom, //中繼
        endRoom, //最終房間
        roadRoom_ToMiddle, //主幹道房間
        roadRoom_ToEnd, //主幹道房間->終點
        treasureRoom, //寶藏房間
        guardianRoom,//守護者房間
        treasureRoadRoom ///aaa

    }
    public List<Type> type = new List<Type>();
    public int roomId;
    public GameObject door_Up, door_Down, door_Left, door_Right, clean; //手動放置
    public MeshRenderer roomIcon;
    public bool doorUp_ison, doorDown_ison, doorLeft_ison, doorRight_ison;
    public bool roomUp, roomDown, roomLeft, roomRight; //對應位置有沒有房間
    public RoomGenerator roomGenerator_;
    public int stepToStart; //計算與起始點的位置
    public int oriDoorCount; //初始-門的數量
    public int levelId; //關卡id 切換場景讀取
    public bool isClearRoom; //通關
    public int roomRandomSeed;
    /// <summary>
    /// 在Instantiate後，設定屬性 (計算門的數量)
    /// [由RoomGenerator呼叫Start]
    /// </summary>
    public void Room_Base_Start() //
    {
        //根據對應位置 開啟門object
        door_Up.SetActive(roomUp);
        door_Down.SetActive(roomDown);
        door_Left.SetActive(roomLeft);
        door_Right.SetActive(roomRight);
        // (初始狀態) 根據對應位置 判斷門是否存在 
        doorUp_ison = roomUp;
        doorDown_ison = roomDown;
        doorLeft_ison = roomLeft;
        doorRight_ison = roomRight;
        // 計算門的數量
        if (roomUp) oriDoorCount++;
        if (roomDown) oriDoorCount++;
        if (roomLeft) oriDoorCount++;
        if (roomRight) oriDoorCount++;
        //根據id設定圖示

        RoomGenerator.OnRoomGeneratorDone += setClean;
    }
    private void OnDestroy()
    {
        RoomGenerator.OnRoomGeneratorDone -= setClean;
    }
    private void setClean()
    {
        clean.SetActive(isClearRoom);
    }
    public void SetIcon()
    {
        var data = GameConfigManager.Instance.GetlevelById(levelId.ToString());
        if (type.Contains(Type.firstRoom))
        {
            roomIcon.material.mainTexture = Resources.Load<Texture>("Icon/Monk_25");
            isClearRoom = true;
            setClean();
        }
        else
        {
            roomIcon.material.mainTexture = Resources.Load<Texture>(data["Icon"]);
        }
    }

    /// <summary>
    /// 計算幾步到起點
    /// <para>[必須] roomGenerator_傳入後</para>
    /// </summary>
    public void set_StepToStart()
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / roomGenerator_.x_Offset) + Mathf.Abs(transform.position.z / roomGenerator_.z_Offset));
    }
    public bool isReachable = true; // 這個值可以讓你從外部設置，例如 Flood Fill 時用

    void OnDrawGizmos()
    {
        // 畫房間方框
        Gizmos.color = isReachable ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 0.1f, 1f));

        // 畫出與鄰居房間的連線（用來表示有門）
        Gizmos.color = Color.cyan;

        if (doorUp_ison)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
        if (doorDown_ison)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.back);
        if (doorLeft_ison)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left);
        if (doorRight_ison)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
    }
}