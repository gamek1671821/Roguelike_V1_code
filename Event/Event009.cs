using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event009 : EventBase
{
    private int seletedCard;
    private bool next;
    public void OnCardSelected(int index)
    {
        seletedCard = index;
        Debug.Log("" + index);
        RoleManager.Instance.roleCard.cardList.Add(seletedCard.ToString());
        RoleManager.Instance.SaveCardList();
        EndChoose();
    }
    public override void CreatButtonOrNextStory()
    {
        if (!MyFuns.Instance.level.levelDone.Contains(int.Parse(data["Id"]))) //
        {
            if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.BloodBeastNecklace).ToString()))
            {
                storyBoard.GetComponentInChildren<TextMeshProUGUI>().text = CustomizedStory();

                ButtonSetting(Button1A, "將項鍊收起", 0);
                ButtonSetting(Button1B, "將項鍊丟棄", 1);
                ButtonSetting(Button1C, "用簡易封印術封印", 2).GetComponent<Button>().interactable = ButtonSet();
            }
            else
            {
                NoChainEvent();
            }
        }
        else
        {
            int PlayerLevel = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
            if (PlayerLevel >= int.Parse(data["NextLevel"])) //如果玩家的等級 (地城通關等級) >= 下一階段等級 
            {
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.BloodBeastNecklace).ToString()))
                {
                    // var data = GameConfigManager.Instance.GetEventById(100009.ToString());
                    // GodManager.Instance.Res = int.Parse(data["LevelId"]);
                }
                else if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.ImprisonNecklace).ToString()))
                {

                }
                else
                {
                    NoChainEvent();
                }
            }
            else
            {
                NoChainEvent();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
        }
    }
    private void NoChainEvent()
    {
        GodManager.Instance.Res = 10017;
    }
    public void Button1A()
    {
        if (!choseDone)
        {//獲得某樣道具
            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();
            EndChoose();
        }
    }
    public void Button1B()
    {
        if (!choseDone)
        {//獲得某樣道具
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.BloodBeastNecklace).ToString()); //
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            EndChoose();
        }
    }
    public void Button1C()
    {
        if (!choseDone)
        {//獲得某樣道具
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.BloodBeastNecklace).ToString()); //失去血染項鍊
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.ImprisonNecklace).ToString()); //獲得被封印的項鍊
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "\n遺物*獸娘的血染項鍊*變為*被封印的血染項鍊*";
            EndBordShow(txt);
        }
    }
    public bool ButtonSet() //設定是否可以點
    {
        return GodManager.Instance.profession == "Wise";
    }
}
