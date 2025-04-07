
using UnityEngine;

public class Fight_Loss : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.showTip("戰敗", Color.green, delegate ()
    {

        DeleteKey();
        UnityEngine.SceneManagement.SceneManager.LoadScene("chose");
        //切換到玩家回合

    });
        FightManager.Instance.StopAllCoroutines();
    }
    public override void OnUpdate()
    {

    }

    public void DeleteKey()
    {
        PlayerPrefs.DeleteKey("profession" + GodManager.Instance.SaveData_ID);//刪除此存檔位置的職業 
        PlayerPrefs.DeleteKey("RoomDataSaver" + GodManager.Instance.SaveData_ID);//刪除此存檔位置的房間 
        PlayerPrefs.DeleteKey("CardListDataSaver" + GodManager.Instance.SaveData_ID);//刪除此存檔位置的卡牌清單
        PlayerPrefs.DeleteKey("Gold" + GodManager.Instance.SaveData_ID); //刪除金幣
        PlayerPrefs.DeleteKey("MaxHP" + GodManager.Instance.SaveData_ID);
        PlayerPrefs.DeleteKey("CurHP" + GodManager.Instance.SaveData_ID);
        PlayerPrefs.DeleteKey("ItemListDataSaver" + GodManager.Instance.SaveData_ID); //刪除道具
        PlayerPrefs.DeleteKey("LevelDataSaver" + GodManager.Instance.SaveData_ID); //刪除已經讀取過的關卡
    }
}

