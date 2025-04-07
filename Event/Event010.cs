using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event010 : EventBase
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

                ButtonSetting(Button1A, "無視沒有攻擊意圖的幽靈", 0);
                ButtonSetting(Button1B, "舉起武器慢慢接近", 1);
                ButtonSetting(Button1C, "遠程攻擊", 2);
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
                if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.DarkRedNecklace).ToString()))
                {
                    // var data = GameConfigManager.Instance.GetEventById(100009.ToString());
                    // GodManager.Instance.Res = int.Parse(data["LevelId"]);
                }
                else if (RoleManager.Instance.roleItem.ItemList.Contains(((int)ItemData.CrazyBeastNecklace).ToString()))
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
            RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.DarkBeastNecklace).ToString()); //
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.DarkRedNecklace).ToString()); //
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "你無視了幽靈，但是幽靈在你接近時盯著你的項鍊。就在他接近你的那一瞬間，黯淡的項鍊發出暗紅色的光芒消滅了幽靈。";
            txt = "\n遺物*黯淡的獸娘項鍊*變為*暗紅色項鍊*";
            EndBordShow(txt);
        }
    }
    public void Button1B()
    {
        if (!choseDone)
        {//獲得某樣道具
             RoleManager.Instance.roleItem.ItemList.Remove(((int)ItemData.DarkBeastNecklace).ToString()); //
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.CrazyBeastNecklace).ToString()); //
            RoleManager.Instance.SaveItemList();

            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "舉起了武器，幽靈也感覺到了威脅發出了尖嘯。就在你發出攻擊時，項鍊發出暗紅色的光芒，你感覺到有一種瘋狂的力量在你體內亂竄。瞬間就消滅了幽靈。";
            txt = "\n遺物*黯淡的獸娘項鍊*變為*野獸之力項鍊*";

            EndBordShow(txt);
        }
    }
    public void Button1C()
    {
        if (!choseDone)
        {//獲得某樣道具
            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "幽靈受到攻擊後，看向投射物飛去的方向。鑽進了牆內消失了。";

            EndBordShow(txt);
        }
    }
}
