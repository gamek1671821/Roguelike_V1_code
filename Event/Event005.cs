using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event005 : EventBase
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
        if (!MyFuns.Instance.level.levelDone.Contains(int.Parse(data["Id"]))) // 如果 沒有執行過這個關卡 
        {
            storyBoard.GetComponentInChildren<TextMeshProUGUI>().text = CustomizedStory(); //

            ButtonSetting(Button0, "等到魔物離去", 0);
            ButtonSetting(Button1, "加入戰鬥", 1);
        }
        else
        {
            storyBoard.GetComponent<Transform>().gameObject.SetActive(false);
            cb.GetComponent<Transform>().gameObject.SetActive(false);

            int PlayerLevel = PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID);
            if (PlayerLevel >= int.Parse(data["NextLevel"])) //如果玩家的等級 (地城通關等級) >= 1 {可以進入第2層}
            {
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.BeastNecklace).ToString()))
                { //如果完成100005 (獸娘第一階段) 擁有 獸娘的項鍊  進入 100006 
                    var data = GameConfigManager.Instance.GetEventById(100006.ToString());
                    GodManager.Instance.Res = int.Parse(data["LevelId"]);
                }
                else
                {//沒有獸娘項鍊 = 無視了獸娘 進入另一路線
                    var data = GameConfigManager.Instance.GetEventById(100007.ToString());
                    GodManager.Instance.Res = int.Parse(data["LevelId"]);
                }
            }
            else //如果等級不足 開啟戰鬥關卡
            {
                GoTobattle();
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
        }
    }


    public override void Button0()
    {
        if (!choseDone)
        {//獲得某樣道具
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.BloodBeastNecklace).ToString());
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();
            var txt = "你在魔物散去之後，看見已經只剩下一攤血泊與一條血痕。你稍微翻找了這裡遺留的物品。";
            txt += "\n獲得遺物*獸娘的血染項鍊";
            EndBordShow(txt);
        }
    }

    public override void Button1()
    {
        if (!choseDone)
        {//獲得某樣道具
            GodManager.Instance.isBattle = true;
            GodManager.Instance.Res = 10005;
            UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();
            EndChoose(true);
        }
    }

}
