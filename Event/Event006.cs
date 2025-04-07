using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event006 : EventBase
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
        if (!MyFuns.Instance.level.levelDone.Contains(int.Parse(data["Id"]))) //沒有執行過10021
        {
            if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.BeastNecklace).ToString()))
            {
                storyBoard.GetComponentInChildren<TextMeshProUGUI>().text = CustomizedStory();

                ButtonSetting(Button0A, "以100金幣要求她買下", 0);
                ButtonSetting(Button0B, "將項鍊還給她", 1);
            }
            else
            {
                NoChainEvent();
            }
        }
        else
        {
            int PlayerLevel = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
            if (PlayerLevel >= int.Parse(data["NextLevel"])) //如果玩家的等級 (地城通關等級) >= 下一階段等級  //如果玩家的等級 (地城通關等級) >= 2 {可以進入第3層}
            {
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.Chili).ToString()))
                {
                    var data = GameConfigManager.Instance.GetEventById(100008.ToString());
                    GodManager.Instance.Res = int.Parse(data["LevelId"]);
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
    public void Button0A()
    {
        if (!choseDone)
        {//獲得某樣道具
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.BeastNecklace).ToString());
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            MyFuns.Instance.GetGold(100);
            var txt = "你看著獸娘從小錢囊裡倒出的金幣，點交給妳後只剩下了3枚。";
            txt += "\n失去遺物*獸娘的項鍊";
            txt += "\n獲得*100金幣";
            EndBordShow(txt);
        }
    }
    public void Button0B()
    {
        if (!choseDone)
        {//獲得某樣道具
            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "你看著獸娘一臉不可置信的表情，把項鍊直接交給他。";
            txt += "\n失去遺物*獸娘的項鍊";
            txt += "\n獸娘戴起項鍊後，拿出一瓶鮮紅色的辣椒給你。";
            txt += "\n獲得遺物*獸人族辣椒瓶";
            EndBordShow(txt);
        }
    }

}
