using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

//戰鬥介面
public class FightUI : UIBase
{
    public static FightUI Instance;
    private TextMeshProUGUI cardCountTxt;
    private TextMeshProUGUI noCardCountTxt, banishCardCounrTxt, limitCardCountTxt;
    private TextMeshProUGUI powerTxt;
    private TextMeshProUGUI hpTxt;
    private Image hpImg;
    private TextMeshProUGUI fyTxt;
    private RectTransform handZone;
    public List<CardItem> handCardItemList = new List<CardItem>();
    public List<CardItem> keepHandCardItemList = new List<CardItem>(); //手牌保留清單 (待製作)
    public TextMeshProUGUI UsedCard, message;
    private float lateFadeOut, messageTimer = 2;
    public TextMeshProUGUI damageText;
    private Coroutine fadeCoroutine; // 用来保存当前运行的协程
    private void Awake()
    {
        Instance = this;
        damageText = transform.Find("DamageText").GetComponent<TextMeshProUGUI>();
        cardCountTxt = transform.Find("hasCard/icon/Text").GetComponent<TextMeshProUGUI>();
        noCardCountTxt = transform.Find("noCard/icon/Text").GetComponent<TextMeshProUGUI>();
        banishCardCounrTxt = transform.Find("banishCard/icon/Text").GetComponent<TextMeshProUGUI>();
        limitCardCountTxt = transform.Find("limitCard/icon/Text").GetComponent<TextMeshProUGUI>();
        UsedCard = transform.Find("LastCard/Text").GetComponent<TextMeshProUGUI>();
        message = transform.Find("message/Text").GetComponent<TextMeshProUGUI>();
        powerTxt = transform.Find("mana/Text").GetComponent<TextMeshProUGUI>();
        hpTxt = transform.Find("hp/Text").GetComponent<TextMeshProUGUI>();
        hpImg = transform.Find("hp/fill").GetComponent<Image>();
        fyTxt = transform.Find("hp/fangyu/Text").GetComponent<TextMeshProUGUI>();
        handZone = transform.Find("handZone").GetComponent<RectTransform>();
        transform.Find("turnBtn").GetComponent<Button>().onClick.AddListener(onChangeTurnBtn);
        transform.Find("freshBtn").GetComponent<Button>().onClick.AddListener(onChangeFreshBtn);
        UpdateHp();
        UpadtePower();
        UpadteDefense();
    }

    //玩家回合結束，切換到敵人回合
    private void onChangeTurnBtn()
    {
        //只有玩家回合才能切換
        if (FightManager.Instance.fightUnit is Fight_PlayerTurn && FightManager.Instance.canUseCard)
        {
            FightManager.Instance.canUseCard = false;
            FightManager.Instance.ChangeType(FightType.Player_Before_End);
        }
    }
    private void onChangeFreshBtn()
    {
        //只有玩家回合才能刷新
        if (FightManager.Instance.fightUnit is Fight_PlayerTurn && FightManager.Instance.canUseCard)
        {
            ReFreshHandCard();
        }
    }
    public bool isMouseInHandZone()
    {
        Vector2 localMousePosition = handZone.InverseTransformPoint(Input.mousePosition);
        return handZone.rect.Contains(localMousePosition);
    }
    public void UpdateHp()
    {
        if (FightManager.Instance.CurHp > FightManager.Instance.MaxHp) FightManager.Instance.CurHp = FightManager.Instance.MaxHp;
        hpTxt.text = FightManager.Instance.CurHp + "/" + FightManager.Instance.MaxHp;
        hpImg.fillAmount = (float)FightManager.Instance.CurHp / (float)FightManager.Instance.MaxHp;
    }
    public void UpadtePower()
    {
        powerTxt.text = FightManager.Instance.CurMoveCount + "/" + FightManager.Instance.MaxMoveCount;
    }
    public void UpadteDefense()
    {
        fyTxt.text = FightManager.Instance.shieldCount.ToString();
    }
    public void UpdateCardCount()
    {
        cardCountTxt.text = FightCardManager.Instance.cardList.Count.ToString();
    }
    public void UpdateNoCardCount()
    {
        noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
        banishCardCounrTxt.text = FightCardManager.Instance.banishCardList.Count.ToString();
        limitCardCountTxt.text = FightCardManager.Instance.limitCardList.Count.ToString();
    }
    //創建卡牌實體
    public void CreatCardItem(int count)
    {
        if (handCardItemList.Count + count >= 12)
        {
            count = 12 - handCardItemList.Count;
            MyFuns.Instance.ShowMessage($"手牌數超過上限");
        }

        int additional = 0; //補抽

        if (count > FightCardManager.Instance.cardList.Count)
        {
            additional = count - FightCardManager.Instance.cardList.Count;
            count = FightCardManager.Instance.cardList.Count;
        }
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, -700);

            string cardId = FightCardManager.Instance.DrawCard();

            Dictionary<string, string> data = GameConfigManager.Instance.GetCardById(cardId);
            CardItem item = obj.AddComponent(System.Type.GetType(data["Script"])) as CardItem;

            item.Init(data);

            handCardItemList.Add(item);
        }
        if (additional != 0)
        {
            FightCardManager.Instance.ResetCardList();
            MyFuns.Instance.ShowMessage($"洗牌後抽牌{additional}張");
        }
        for (int i = 0; i < additional; i++)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, -700);

            string cardId = FightCardManager.Instance.DrawCard();

            Dictionary<string, string> data = GameConfigManager.Instance.GetCardById(cardId);
            CardItem item = obj.AddComponent(System.Type.GetType(data["Script"])) as CardItem;

            item.Init(data);

            handCardItemList.Add(item);

            // UpdateCardCount();
        }
        UpdateCardCount();
        UpdateNoCardCount();
    }
    public void UpdateCardItemPos()  //更新手牌位置
    {
        float offset;
        if (handCardItemList.Count > 0)
            offset = 1700 / handCardItemList.Count;
        else
            offset = 0;
        Vector2 startPos = new Vector2(offset * 0.63f - 1700 * 0.37f, -520);
        for (int i = 0; i < handCardItemList.Count; i++)
        {
            handCardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.5f);
            startPos.x = startPos.x + offset;
        }
    }
    public void RemoveCard(CardItem item, string isBanishCard = "N")
    {
        AudioManager.Instance.PlayEffect("Cards/cardShove");//移除音效
        item.enabled = false; //禁用卡牌邏輯

        switch (isBanishCard)
        {
            case "N": //一般卡
                FightCardManager.Instance.usedCardList.Add(item.data["Id"]);//添加至棄牌堆
                break;
            case "B": //消耗卡 
                FightCardManager.Instance.banishCardList.Add(item.data["Id"]);//添加至除外區
                break;
            case "L": //限一卡
                FightCardManager.Instance.limitCardList.Add(item.data["Id"]);//添加至限一卡除外區
                break;
        }
        //noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();//更新棄牌堆數值
        UpdateNoCardCount();
        handCardItemList.Remove(item); //從集合中刪
        UpdateCardItemPos(); //刷新卡牌位置除
        item.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -700), 0.25f);//卡牌移到棄牌效果
        item.transform.DOScale(0, 0.25f);
        Destroy(item.gameObject, 1);
    }
    public void RemoveAllCards()
    {
        for (int i = handCardItemList.Count - 1; i >= 0; i--)
        {
            RemoveCard(handCardItemList[i]);
        }
    }
    public void ReFreshHandCard()
    {
        int hands = handCardItemList.Count;
        for (int i = hands - 1; i >= 0; i--)
        {
            FightCardManager.Instance.cardList.Add(handCardItemList[i].data["Id"]); //牌堆上方追加 
            Destroy(handCardItemList[i].gameObject);

        }
        handCardItemList.Clear();
        CreatCardItem(hands);//抽牌 
        UpdateCardItemPos(); //刷新卡牌位置除
    }
    public void MessageStartFadeOutText(string text)
    {
        message.text = text;
        StartFadeOut();
    }
    private IEnumerator FadeOutText()
    {
        message.color = new Color(256, 256, 256, 256); // 修改颜色
        float elapsedTime = 0f;
        yield return new WaitForSeconds(lateFadeOut);

        Color originalColor = message.color; // 获取初始颜色

        while (elapsedTime < messageTimer)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / messageTimer); // 计算 Alpha 值
            message.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // 修改颜色
            yield return null;
        }

        // 确保最后 Alpha 为 0
        message.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
    private void StartFadeOut()
    {
        // 如果协程正在运行，先停止它
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // 启动新的淡化协程
        fadeCoroutine = StartCoroutine(FadeOutText());
    }
}
