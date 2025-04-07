using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class Card62_CardShield : CardItem
{//從牌組抽出{0}~{1}張卡。之後，獲得等同手牌數的護甲。僅受幸運影響。最大值受幸運2倍影響。
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = CountLucky("Arg0");
            int val2 = CountLucky(CountLucky("Arg1"));
            int R_Val3 = Random.Range(val, val2 + 1);

            MyFuns.Instance.DrawCard(R_Val3);//抽牌 
            FightManager.Instance.shieldCount += CountDisappoint(FightUI.Instance.handCardItemList.Count); //增加護盾

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
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
        else if (CountLucky(CountLucky("Arg1")) > (FightCardManager.Instance.cardList.Count + FightCardManager.Instance.usedCardList.Count))
        {
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("無法抽出最大值需求數量", Color.red);
            return false;
        }
        else
        {
            //減少費用
            FightManager.Instance.CurMoveCount -= cost;
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountLucky("Arg0")), CRedT(CountLucky(CountLucky("Arg1")))); // 字串as
    }
}
