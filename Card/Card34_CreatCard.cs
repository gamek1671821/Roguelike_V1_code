using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card34_CreatCard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //EffAndAudio();
            StartCoroutine(Useing());
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    private bool next;
    private int seletedCard;
    public IEnumerator Useing()
    {
        //獲得1回合2智壞，從全牌庫檢視5。複製其中1張。不受任何加成影響。
        next = false;

        FightManager.Instance.GetDeBuff(DeBuffType.deIntellect, 1, 2); // 獲得1回合2智壞

        int chose_0 = FightManager.Instance.PickCard("all"); //第一張 從棄牌堆中抽選出第一張卡的 ID
        int chose_1, chose_2, chose_3, chose_4;
        //避免卡片重複
        do { chose_1 = MyFuns.Instance.pickOneCard("all"); }
        while (chose_0 == chose_1);

        do { chose_2 = MyFuns.Instance.pickOneCard("all"); }
        while (chose_0 == chose_2 || chose_1 == chose_2);

        do { chose_3 = MyFuns.Instance.pickOneCard("all"); }
        while (chose_0 == chose_3 || chose_1 == chose_3 || chose_2 == chose_3);

        do { chose_4 = MyFuns.Instance.pickOneCard("all"); }
        while (chose_0 == chose_4 || chose_1 == chose_4 || chose_2 == chose_4 || chose_3 == chose_4);

        Transform canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform;
        var choseboard = Instantiate(Resources.Load("UI/choseboard"), canvesTf); //.GetComponent<Transform>(). SetAsFirstSibling()
        Transform choseboardTf = choseboard.GetComponent<Transform>();
        choseboard.GetComponent<choseboard>().Init(5, 0);

        var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly>();
        cardSHowOnly_0.Init(GameConfigManager.Instance.GetCardById(chose_0.ToString()));
        cardSHowOnly_0.onPointDown += OnCardSelected;
        CardChose_0.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(0).transform.position.x, choseboardTf.GetChild(0).transform.position.y);

        //CardChose.GetComponentInChildren<Button>().onClick.AddListener(() => { OnCardSelected(int.Parse(card1)); }); //
        var CardChose_1 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_1 = CardChose_1.AddComponent<CardItemShowOnly>();
        cardSHowOnly_1.Init(GameConfigManager.Instance.GetCardById(chose_1.ToString()));
        cardSHowOnly_1.onPointDown += OnCardSelected;
        CardChose_1.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(1).transform.position.x, choseboardTf.GetChild(1).transform.position.y);

        var CardChose_2 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_2 = CardChose_2.AddComponent<CardItemShowOnly>();
        cardSHowOnly_2.Init(GameConfigManager.Instance.GetCardById(chose_2.ToString()));
        cardSHowOnly_2.onPointDown += OnCardSelected;
        CardChose_2.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(2).transform.position.x, choseboardTf.GetChild(2).transform.position.y);

        var CardChose_3 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_3 = CardChose_3.AddComponent<CardItemShowOnly>();
        cardSHowOnly_3.Init(GameConfigManager.Instance.GetCardById(chose_3.ToString()));
        cardSHowOnly_3.onPointDown += OnCardSelected;
        CardChose_3.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(8).transform.position.x, choseboardTf.GetChild(8).transform.position.y);

        var CardChose_4 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_4 = CardChose_4.AddComponent<CardItemShowOnly>();
        cardSHowOnly_4.Init(GameConfigManager.Instance.GetCardById(chose_4.ToString()));
        cardSHowOnly_4.onPointDown += OnCardSelected;
        CardChose_4.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(9).transform.position.x, choseboardTf.GetChild(9).transform.position.y);


        while (!next)
        {
            yield return null;
            //yield return new WaitUnit(OnCardSelected);
        }
        Destroy(choseboard); //刪除面板

        FightCardManager.Instance.cardList.Add(seletedCard.ToString()); //牌堆上方追加
        UIManager.Instance.GetUI<FightUI>("FightUI").CreatCardItem(1); //抽1
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//更新卡牌位置
        Destroy(CardChose_0); //刪除面板
        Destroy(CardChose_1); //刪除面板
        Destroy(CardChose_2); //刪除面板
        Destroy(CardChose_3); //刪除面板
        Destroy(CardChose_4); //刪除面板

        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
        CardEffectEnd();//卡片效果結束
    }
    public void OnCardSelected(int index)
    {
        seletedCard = index;
        next = true;
        // return true;
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", UnityEngine.Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("費用不足", Color.red);
            return false;
        }
        else
        {
            //減少費用
            FightManager.Instance.CurMoveCount -= cost;
            //更新文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();
            //使用的卡牌刪除 (有協程不譨刪除)
            //UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            //(有協程只關閉子物件)
            transform.GetChild(0).gameObject.SetActive(false);
            return true;
        }
    }
}
