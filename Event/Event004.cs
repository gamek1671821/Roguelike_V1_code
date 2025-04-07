using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event004 : EventBase
{
    private int seletedCard;
    private bool next;
    private List<UnityEngine.Object> Card = new List<UnityEngine.Object> { };
    public void OnCardSelected(int index)
    {
        seletedCard = index;
        var data = GameConfigManager.Instance.GetCardById(seletedCard.ToString());
        MyFuns.Instance.GetGold(-int.Parse(data["Gold"]));
        RoleManager.Instance.roleCard.cardList.Add(seletedCard.ToString());
        RoleManager.Instance.SaveCardList();
        EndChoose();
    }
    public override void CreatButtonOrNextStory()
    {
        int chose_0 = MyFuns.Instance.pickOneCard("SR"); //第一張 從SR牌堆選取一張卡牌 ()
        int chose_1 = MyFuns.Instance.pickOneCard("SR"); //第一張 從SR牌堆選取一張卡牌 ()
        int chose_2 = MyFuns.Instance.pickOneCard("SR"); //第一張 從SR牌堆選取一張卡牌 ()
        int chose_3 = MyFuns.Instance.pickOneCard("SR"); //第一張 從SR牌堆選取一張卡牌 ()
        int chose_4 = MyFuns.Instance.pickOneCard("SR"); //第一張 從SR牌堆選取一張卡牌 ()

        CreatChooseCard(chose_0, 0);
        CreatChooseCard(chose_1, 1);
        CreatChooseCard(chose_2, 2);
        CreatChooseCard(chose_3, 3);
        CreatChooseCard(chose_4, 4);

        ButtonSetting(Button0, "不需要", 0);

        storyBoard.GetComponentInChildren<TextMeshProUGUI>().text += $"\n持有金幣：{MyFuns.Instance.Gold()}";
    }
    private void CreatChooseCard(int cardId, int BtnZoneId)
    {
        var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        Card.Add(CardChose_0);
        var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly>();
        var data = GameConfigManager.Instance.GetCardById(cardId.ToString());
        cardSHowOnly_0.Init(data);
        if (MyFuns.Instance.Gold() >= int.Parse(data["Gold"])) cardSHowOnly_0.onPointDown += OnCardSelected;
        var btnTr_0 = transform.Find("CardChose" + BtnZoneId).transform;
        CardChose_0.GetComponent<Transform>().position = new Vector2(btnTr_0.position.x, btnTr_0.position.y);
    }


    public override void Button0()
    {
        if (!choseDone)
        {
            EndChoose();
        }
    }
}
