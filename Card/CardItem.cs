using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public string[] cardPools;
    public Dictionary<string, string> data;//卡牌訊息
    private int index;
    Vector2 initPos; //開始拖曳時 紀錄卡牌位置
    public TextMeshProUGUI msgText, costText;
    public string msgOriTxt, costOriTxt;
    private GameObject GoldObject;
    public virtual void Init(Dictionary<string, string> data)
    {
        this.data = data;
        cardPoolsSplit();
    }

    //開始拖曳
    public virtual void OnBeginDrag(PointerEventData eventData)
    {

        initPos = transform.GetComponent<RectTransform>().anchoredPosition;
        //播放聲音
        AudioManager.Instance.PlayEffect("Cards/draw");
        DragMsgChange();

        Debug.Log($"cardPools: {string.Join(", ", cardPools)}");
    }
    public virtual void DragMsgChange() { }
    /// <summary>
    /// 卡片效果
    /// </summary>
    public virtual void CardEffect() { }


    //拖曳中
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out pos
        ))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
    //結束拖曳
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = initPos;
        transform.SetSiblingIndex(index);
        msgText.text = msgOriTxt; //還原文字敘述
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.15f, 0.01f); //滑鼠放上時的縮放效果

        index = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.yellow);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 10);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.01f);
        transform.SetSiblingIndex(index);
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.black);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 1);
    }

    private void Start()
    {
        transform.Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["BgIcon"]); //設定卡牌主卡
        transform.Find("bg/icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["Icon"]); //設定卡牌圖示
        //設定文字敘述
        msgText = transform.Find("bg/msgTxt").GetComponent<TextMeshProUGUI>();
        msgOriTxt = string.Format(data["Des"], data["Arg0"], data["Arg1"], data["Arg2"]); // 字串 (原始敘述)
        msgText.text = msgOriTxt;
        msgText.fontSize = int.Parse(data["Size"]); //設定文字敘述字體大小
        //設定卡牌名稱
        transform.Find("bg/nameTxt").GetComponent<TextMeshProUGUI>().text = data["Name"];
        //設定卡牌消耗
        costText = transform.Find("bg/useTxt").GetComponent<TextMeshProUGUI>();
        costOriTxt = data["Expend"];
        costText.text = costOriTxt;
        //設定卡牌名稱
        transform.Find("bg/Text").GetComponent<TextMeshProUGUI>().text = GameConfigManager.Instance.GetCardTypeById(data["Type"])["Name"];
        //設置背景image外邊框材質
        transform.Find("bg").GetComponent<Image>().material = Instantiate(Resources.Load<Material>("Mats/outline"));
        //設定卡牌稀有度與ID
        transform.Find("bg/cardID").GetComponent<TextMeshProUGUI>().text = $"{data["Rarity"]} {data["Id"]}";

        if (GetType().Name == "CardItemShowOnly" || GetType().Name == "CardItemShowOnly_Book")
        {
            transform.Find("bg/Gold").gameObject.SetActive(true);//GoldTxt
            transform.Find("bg/Gold/GoldTxt").GetComponent<TextMeshProUGUI>().text = data["Gold"];
        }
        else
        {
            transform.Find("bg/Gold").gameObject.SetActive(false);//GoldTxt
        }
    }
    public virtual bool TryUse()
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
            UIManager.Instance.showTip("動點不足", Color.red);
            return false;
        }
        else
        {
            FightManager.Instance.canUseCard = false;
            //減少費用
            FightManager.Instance.CurMoveCount -= cost;
            //更新文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
    //創建卡牌使用過的特效
    public virtual void PlayEffect(Vector3 pos)
    {
        GameObject effectObj = Instantiate(Resources.Load(data["Effects"])) as GameObject;
        effectObj.transform.position = pos;
        Destroy(effectObj, 2);
    }
    /// <summary>
    /// 指定類型的卡牌，更改內容資訊
    /// </summary>
    public virtual string PointMessage(int damage, Enemy enemy)
    {
        string mess;
        int baseCount = damage - enemy.shield;
        if (baseCount >= 0) mess = $"<color=red>{damage}</color>傷害 [貫穿]";
        else mess = $"<color=red>{damage}</color>傷害";

        return mess;
    }
    /// <summary>
    /// 計算攻擊力
    /// </summary>
    public virtual int CountAttack(string arg)
    {
        int dataAtk = int.Parse(data[arg]); //卡片面板傷害 
        return CountAttack(dataAtk);
    }
    public virtual int CountAttack(int dataAtk)
    {
        int withPow = dataAtk + FightManager.Instance.buffsVal[(int)BuffType.power]; //加上 角色力量
        return withPow;
    }
    public virtual int CountDefend(string arg)
    {
        int dataDef = int.Parse(data[arg]); //卡片面板防禦
        return CountDefend(dataDef);
    }
    public virtual int CountDefend(int dataDef)
    {
        //卡片面板防禦
        int withHard = dataDef + FightManager.Instance.buffsVal[(int)BuffType.hard]; //加上 角色硬甲
        return withHard;
    }
    public virtual int CountIntellect(string arg)
    {
        int datavalue = int.Parse(data[arg]); //卡片面版數值
        return CountIntellect(datavalue);
    }
    public virtual int CountIntellect(int datavalue)
    {
        int withIntellect = datavalue + FightManager.Instance.buffsVal[(int)BuffType.intellect]; //加上 角色精智力
        return withIntellect;
    }
    public virtual int CountRebound(string arg)
    {
        int datavalue = int.Parse(data[arg]); //卡片面版數值
        return CountRebound(datavalue);
    }

    public virtual int CountRebound(int datavalue)
    {
        int withIntellect = datavalue + FightManager.Instance.buffsVal[(int)BuffType.rebound]; //加上 角色尖刺
        return withIntellect;
    }
    public virtual int CountPowerPoisoned(string arg)
    {
        int datavalue = int.Parse(data[arg]); //卡片面版數值
        return CountPowerPoisoned(datavalue);
    }
    public virtual int CountPowerPoisoned(int datavalue)
    {
        int withIntellect = datavalue + FightManager.Instance.buffsVal[(int)BuffType.powerpoisoned]; //加上 角色猛毒
        return withIntellect;
    }
    public virtual int CountLucky(string arg)
    {
        int datavalue = int.Parse(data[arg]); //卡片面版數值
        return CountLucky(datavalue);
    }
    public virtual int CountLucky(int datavalue)
    {
        int withIntellect = datavalue + FightManager.Instance.buffsVal[(int)BuffType.Lucky]; //加上 角色猛毒
        return withIntellect;
    }
    public virtual int CountUnLucky(string arg)
    {
        int datavalue = int.Parse(data[arg]); //卡片面版數值
        return CountUnLucky(datavalue);
    }
    public virtual int CountUnLucky(int datavalue)
    {
        int withIntellect = datavalue - FightManager.Instance.buffsVal[(int)BuffType.Lucky]; //加上 角色猛毒
        return withIntellect;
    }
    public int CountDisappoint(int val)
    {
        if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.disappoint] > 0)
            return val / 2;
        else
            return val;
    }
    public bool LuckyProbability(int baseVal, int preVal, bool isPositive = true)
    {
        int check;
        int random = Random.Range(0, 101);
        if (isPositive)
        {
            check = baseVal + preVal * FightManager.Instance.buffsVal[(int)BuffType.Lucky];
            return check >= random;
        }
        else
        {
            check = baseVal - preVal * FightManager.Instance.buffsVal[(int)BuffType.Lucky];
            return check >= random;
        }
    }
    /// <summary>
    /// (非指定)生成效果跟音效
    /// </summary>
    public void EffAndAudio()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f)); //計算點位
        PlayEffect(pos);//在計算點位上升成特效
        AudioManager.Instance.PlayEffect(data["sound"]); //播放使用音
    }
    /// <summary>
    /// 確認卡片傷害是否致死
    /// </summary>
    public void FatalAttackdetermination()
    {
        FightManager.Instance.FatalAttackdetermination();
    }
    public void UseCardCountPlus(string Type)
    {
        switch (Type)
        {
            case "10000":
                //全部可替代卡
                break;
            case "10001":
                UseingItem_Attack();
                FightManager.Instance.thisTurnAttackCount++; //使用了攻擊卡 
                break;
            case "10002":
                UseingItem_Defend();
                FightManager.Instance.thisTurnDefendCount++; //使用了防禦卡 
                break;
            case "10003":
                UseingItem_Sus();
                break;
            case "10004":
                UseingItem_Attack();
                UseingItem_Banish();
                FightManager.Instance.thisTurnAttackCount++; //使用了攻擊卡 
                FightManager.Instance.thisTurnBanishCardCount++; //消耗了卡牌
                break;
            case "10005":
                UseingItem_Skill();
                FightManager.Instance.thisTurnSkillkCount++; //使用了技能卡 
                break;
            case "10006":
                UseingItem_Skill();
                UseingItem_Banish();
                FightManager.Instance.thisTurnSkillkCount++; //使用了技能卡 
                FightManager.Instance.thisTurnBanishCardCount++; //消耗了卡牌
                break;
            case "10007":
                UseingItem_Magic();
                FightManager.Instance.thisTurnMagickCount++; //使用了法術卡 
                break;
            case "10008":
                UseingItem_Magic();
                UseingItem_Banish();
                FightManager.Instance.thisTurnMagickCount++; //使用了法術卡 
                FightManager.Instance.thisTurnBanishCardCount++; //消耗了卡牌
                break;
            case "10009":
                UseingItem_Defend();
                UseingItem_Banish();
                FightManager.Instance.thisTurnDefendCount++; //使用了防禦卡 
                FightManager.Instance.thisTurnBanishCardCount++; //消耗了卡牌
                break;
        }
        FightManager.Instance.thisTurnCardCount++; //使用了卡牌
        FightManager.Instance.lastCardType = Type;
        FightManager.Instance.UpdateCardUsed();

    }
    private void UseingItem_Attack()
    {
        if (MyFuns.Instance.HaveItem(ItemData.PainArmband))
        {
            if (FightManager.Instance.thisTurnAttackCount >= 3) //當攻擊3次以上
            {
                foreach (var enemy in EnemyManager.Instance.enemyList)
                {
                    enemy.Hit(1, true);
                }
                MyFuns.Instance.ShowMessage($"觸發「痛苦臂環」", MyFuns.MessageType.Item);
                FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)
            }
        }

        if (MyFuns.Instance.HaveItem(ItemData.Violence)) //暴力寶珠
        {
            if (FightManager.Instance.thisTurnAttackCount == 5) //當攻擊3次以上
            {
                MyFuns.Instance.ShowMessage($"觸發「暴力寶珠」", MyFuns.MessageType.Item);
                FightManager.Instance.GetBuff(BuffType.power, 99, 1);
            }
        }
    }
    private void UseingItem_Defend()
    {
        if (MyFuns.Instance.HaveItem(ItemData.ArmorPotion))
        {
            if (FightManager.Instance.thisTurnDefendCount == 5)
            {
                MyFuns.Instance.ShowMessage($"觸發「裝甲強化劑」", MyFuns.MessageType.Item);
                FightManager.Instance.shieldCount += FightManager.Instance.shieldCount;
            }
        }
        if (MyFuns.Instance.HaveItem(ItemData.FlashPotion))
        {
            if (FightManager.Instance.thisTurnDefendCount >= 1)
            {
                MyFuns.Instance.ShowMessage($"觸發「速戰藥劑」", MyFuns.MessageType.Item);
                FightManager.Instance.InterHit_IsDeath(2, true);
            }
        }
    }
    private void UseingItem_Sus()
    {

    }
    private void UseingItem_Skill()
    {

    }
    private void UseingItem_Magic()
    {
        if (MyFuns.Instance.HaveItem(ItemData.Magicloak))
        {
            MyFuns.Instance.ShowMessage($"觸發「魔法斗篷」", MyFuns.MessageType.Item);
            FightManager.Instance.shieldCount += 1;
        }
    }
    private void UseingItem_Banish()
    {

    }

    public virtual void CardEffectEnd(bool turnContinue = true)
    {
        UseCardCountPlus(data["Type"]);//計算卡牌
        FightManager.Instance.CardEffectEnd(turnContinue);
        if (!turnContinue) DirectEnd(); //直接結束回合  
    }
    public void DirectEnd()
    {
        if (EnemyManager.Instance.enemyList.Count > 0)
            FightManager.Instance.ChangeType(FightType.Player_Before_End);
    }
    /// <summary>
    /// skill , attack , magic , sus , defend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool checkLastCardType(string input)
    {
        string useCard = FightManager.Instance.lastCardType; ;
        bool canUse = true;
        switch (input)
        {
            case "skill":
                canUse = useCard == "10005" || useCard == "10006"; //技能後
                return canUse;
            case "attack":
                canUse = useCard == "10001" || useCard == "10004"; //攻擊後
                return canUse;
            case "magic":
                canUse = useCard == "10007" || useCard == "10008"; //法術後
                return canUse;
            case "sus":
                canUse = useCard == "10003"; //永續後
                return canUse;
            case "defend":
                canUse = useCard == "10002" || useCard == "10009"; //永續後
                return canUse;
            default:
                return false;
        }
    }
    private void cardPoolsSplit()
    {
        cardPools = data["cardPool"].Split('='); //卡牌卡池
    }
    public void BeforeEnd()
    {
        FightManager.Instance.TurnEndList(BeforeEndEffect);
    }
    public virtual void BeforeEndEffect() { }
    public void NoThrowAway()
    {
        FightCardManager.Instance.cardList.Add(data["Id"]); //牌堆上方追加
        FightCardManager.Instance.usedCardList.Remove(data["Id"]); //棄牌堆刪除
        UIManager.Instance.GetUI<FightUI>("FightUI").CreatCardItem(1);//抽牌 
    }
    public void NoBanish()
    {
        FightCardManager.Instance.cardList.Add(data["Id"]); //牌堆上方追加
        FightCardManager.Instance.banishCardList.Remove(data["Id"]); //消耗牌堆刪除
        UIManager.Instance.GetUI<FightUI>("FightUI").CreatCardItem(1);//抽牌 
    }
    /// <summary>
    /// 這是修改文字顏色 (紅色)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string CRedT(object input)
    {
        return $"<color=red>{input}</color>";
    }
    /// <summary>
    /// 這是修改文字顏色 (灰色+刪除線)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string CGrayT(object input)
    {
        return $"<color=#4F4F4F><s>{input}</s></color>";
    }
    public string CPurpT(object input)
    {
        return $"<color=#7D7DFF>{input}</color>";
    }
    public virtual void DamageText() { }
    public virtual void OnPointDamageText() { }

    public Enemy hitEnemy; //射線檢測到的敵人腳本
    public TextMeshProUGUI damageText;
    public void CheckRayToEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //產生射線
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
        {
            hitEnemy = hit.transform.GetComponent<Enemy>();
            hitEnemy.OnSelect();

            OnPointDamageText();

            if (Input.GetMouseButtonDown(0)) //按下左鍵 使用
            {
                StopAllCoroutines(); //關閉所有協程
                Cursor.visible = true; //顯示滑鼠
                UIManager.Instance.CloseUI("LineUI");
                if (TryUse() == true)
                {
                    CardEffect();
                }

                DamageText();

                hitEnemy.UnOnSelect();
                hitEnemy = null;
            }
        }
        else
        {
            if (hitEnemy != null)
            {
                DamageText();

                hitEnemy.UnOnSelect();
                hitEnemy = null;
            }
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayEffect("Cards/draw");
        UIManager.Instance.ShowUI<LineUI>("LineUI");//顯示曲線介面
        UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition);

        damageText = UIManager.Instance.GetUI<LineUI>("LineUI").transform.Find("endPoint/DamageText").GetComponent<TextMeshProUGUI>();

        DamageText();

        Cursor.visible = false; //隱藏滑鼠
        StopAllCoroutines(); //關閉所有協程
        StartCoroutine(OnMouseDownRight(eventData));
    }

    IEnumerator OnMouseDownRight(PointerEventData pData)
    {
        while (true)
        {
            //按下滑鼠右鍵 跳出循環
            if (Input.GetMouseButton(1) || !FightManager.Instance.canUseCard)
            {
                break;
            }
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                pData.position,
                pData.pressEventCamera,
                out pos
            ))
            {
                UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
                //射線檢測是否碰到怪物
                CheckRayToEnemy();
            }
            yield return null;
        }
        Cursor.visible = true; //跳出迴圈後顯示滑鼠
        UIManager.Instance.CloseUI("LineUI");
    }
}
