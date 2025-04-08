using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event008 : EventBase
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

                ButtonSetting(Button1A, "委婉拒絕", 0);
                ButtonSetting(Button1B, "接受：共同分享的提議", 1);
                ButtonSetting(Button1B, "接受：500金幣賣出", 1);
            }
            else
            {
                GoTobattle();
            }
        }
        else
        {
            int PlayerLevel = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
            if (PlayerLevel >= int.Parse(data["NextLevel"])) //如果玩家的等級 (地城通關等級) >= 下一階段等級 
            {
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.HalfChili).ToString())) //
                {
                    // var data = GameConfigManager.Instance.GetEventById(100009.ToString());
                    // GodManager.Instance.Res = int.Parse(data["LevelId"]);
                }
                else if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.Chili).ToString())) //
                {

                }
                else
                {
                    GoTobattle();
                }
            }
            else
            {
                GoTobattle();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
        }
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
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.Chili).ToString()); //失去辣椒
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.HalfChili).ToString()); //獲得一半的辣椒
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.DragonBone).ToString());//獲得龍骨
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();


            var txt = "你看著美食獵人處裡食材，並在他的指引下放入辣椒。";
            txt += "\n遺物*獸人族辣椒瓶*變為*剩下一半的辣椒瓶*";
            txt += "\n獲得遺物*龍骨*";
            EndBordShow(txt);
        }
    }
    public void Button1C()
    {
        if (!choseDone)
        {//獲得某樣道具
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.Chili).ToString());
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            MyFuns.Instance.GetGold(500);
            var txt = "美食獵人很爽快地支付500金幣，然後一轉眼就離開這裡。";
            txt += "\n失去遺物*獸人族辣椒瓶*";
            txt += "\n獲得*500金幣";
            EndBordShow(txt);
        }
    }

}
