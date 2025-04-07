using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card10_Return : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    //public System
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
        //從棄牌堆<color=#7D7DFF>檢視3</color>。抽出其中1張。之後將其餘2張放回棄牌堆
        next = false;

        int chose_0 = Random.Range(0, FightCardManager.Instance.usedCardList.Count); //第一張 從棄牌堆中抽選出第一張卡的 ID
        string card_0 = FightCardManager.Instance.usedCardList[chose_0];
        FightCardManager.Instance.usedCardList.Remove(card_0); //從棄牌堆中 移除

        int chose_1 = Random.Range(0, FightCardManager.Instance.usedCardList.Count);
        string card_1 = FightCardManager.Instance.usedCardList[chose_1];
        FightCardManager.Instance.usedCardList.Remove(card_1);

        int chose_2 = Random.Range(0, FightCardManager.Instance.usedCardList.Count);
        string card_2 = FightCardManager.Instance.usedCardList[chose_2];
        FightCardManager.Instance.usedCardList.Remove(card_2);

        Transform canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform;
        var choseboard = Instantiate(Resources.Load("UI/choseboard"), canvesTf); //.GetComponent<Transform>(). SetAsFirstSibling()
        Transform choseboardTf = choseboard.GetComponent<Transform>();
        choseboard.GetComponent<choseboard>().Init(3, 0);

        var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly>();
        cardSHowOnly_0.Init(GameConfigManager.Instance.GetCardById(card_0));
        cardSHowOnly_0.onPointDown += OnCardSelected;

        CardChose_0.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(0).transform.position.x, choseboardTf.GetChild(0).transform.position.y);
        //CardChose.GetComponentInChildren<Button>().onClick.AddListener(() => { OnCardSelected(int.Parse(card1)); }); //
        var CardChose_1 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_1 = CardChose_1.AddComponent<CardItemShowOnly>();
        cardSHowOnly_1.Init(GameConfigManager.Instance.GetCardById(card_1));
        cardSHowOnly_1.onPointDown += OnCardSelected;
        CardChose_1.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(1).transform.position.x, choseboardTf.GetChild(1).transform.position.y);

        var CardChose_2 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_2 = CardChose_2.AddComponent<CardItemShowOnly>();
        cardSHowOnly_2.Init(GameConfigManager.Instance.GetCardById(card_2));
        cardSHowOnly_2.onPointDown += OnCardSelected;
        CardChose_2.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(2).transform.position.x, choseboardTf.GetChild(2).transform.position.y);

        while (!next)
        {
            yield return null;
            //yield return new WaitUnit(OnCardSelected);
        }

        Destroy(choseboard); //刪除面板
        FightCardManager.Instance.usedCardList.Add(card_0);//卡牌放回棄牌堆
        FightCardManager.Instance.usedCardList.Add(card_1);//卡牌放回棄牌堆
        FightCardManager.Instance.usedCardList.Add(card_2);//卡牌放回棄牌堆
        FightCardManager.Instance.cardList.Add(seletedCard.ToString()); //牌堆上方追加
        FightCardManager.Instance.usedCardList.Remove(seletedCard.ToString());//卡牌移除棄牌堆
        UIManager.Instance.GetUI<FightUI>("FightUI").CreatCardItem(1); //抽1
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//更新卡牌位置
        Destroy(CardChose_0); //刪除面板
        Destroy(CardChose_1); //刪除面板
        Destroy(CardChose_2); //刪除面板

        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
        CardEffectEnd();//卡片效果結束
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("費用不足", Color.red);
            return false;
        }
        else if (FightCardManager.Instance.usedCardList.Count <= 2)
        {
            UIManager.Instance.showTip("棄牌堆卡牌數量不足", Color.red);
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

    public void OnCardSelected(int index)
    {
        seletedCard = index;
        next = true;
        // return true;
    }
}
