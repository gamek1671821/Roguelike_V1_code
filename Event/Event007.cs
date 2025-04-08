using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event007 : EventBase
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

                ButtonSetting(Button1A, "無視", 0);
                ButtonSetting(Button1B, "仔細打量", 1);
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
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.Chili).ToString()))
                {
                    // var data = GameConfigManager.Instance.GetEventById(100009.ToString());
                    // GodManager.Instance.Res = int.Parse(data["LevelId"]);
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
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.BloodBeastNecklace).ToString());
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.DarkBeastNecklace).ToString());
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();


            var txt = "你把項鍊拿起來放在耳邊仔細聽，真的有心跳的聲音。拿到火炬前，看著透著光的寶石。輕輕擦拭它上面殘留的污漬。這樣一摩擦，你感覺手指像是被燒紅的鐵釘刺中，反射性地縮了手。項鍊寶石發生了變化。";
            txt += "\n遺物*獸娘的血染項鍊*變為*黯淡的獸娘項鍊*";
            EndBordShow(txt);
        }
    }

}
