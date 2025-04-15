using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event011_B1 : EventBase
{
    private int seletedCard;
    private bool next;
    private List<UnityEngine.Object> Card = new List<UnityEngine.Object> { };
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
            storyBoard.GetComponentInChildren<TextMeshProUGUI>().text = CustomizedStory();

            int chose_0 = MyFuns.Instance.pickOneCard("UR"); //第一張 從SR牌堆選取一張卡牌 ()
            int chose_1 = MyFuns.Instance.pickOneCard("UR"); //第一張 從SR牌堆選取一張卡牌 ()
            int chose_2 = MyFuns.Instance.pickOneCard("UR"); //第一張 從SR牌堆選取一張卡牌 ()
            int chose_3 = MyFuns.Instance.pickOneCard("UR"); //第一張 從SR牌堆選取一張卡牌 ()
            int chose_4 = MyFuns.Instance.pickOneCard("UR"); //第一張 從SR牌堆選取一張卡牌 ()

            ButtonSetting(Button0, "推開他", 0);
            ButtonSetting(Button1, "無視他", 1);
            ButtonSetting(Button2, "搶劫", 2);

            CreatChooseCard(chose_0, 0);
            CreatChooseCard(chose_1, 1);
            CreatChooseCard(chose_2, 2);
            CreatChooseCard(chose_3, 3);
            CreatChooseCard(chose_4, 4);
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
    public override void Button2()
    {
        if (!choseDone)
        {//獲得某樣道具
            MyFuns.Instance.level.levelDone.Add(int.Parse(data["Id"]));
            MyFuns.Instance.SaveLevel();

            var txt = "幽靈受到攻擊後，看向投射物飛去的方向。鑽進了牆內消失了。";

            EndBordShow(txt);
        }
    }
    private void CreatChooseCard(int cardId, int BtnZoneId)
    {
        var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        Card.Add(CardChose_0);
        var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly>();
        cardSHowOnly_0.showGold = false;
        var data = GameConfigManager.Instance.GetCardById(cardId.ToString());
        cardSHowOnly_0.Init(data);
        if (MyFuns.Instance.Gold() >= int.Parse(data["Gold"])) cardSHowOnly_0.onPointDown += OnCardSelected;
        var btnTr_0 = transform.Find("CardChose" + BtnZoneId).transform;
        CardChose_0.GetComponent<Transform>().position = new Vector2(btnTr_0.position.x, btnTr_0.position.y);
    }
}
