using UnityEngine;
using UnityEngine.EventSystems;

public class Card02_AddCard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //從牌組抽出{0}張卡。
            EffAndAudio();

            int val = CountIntellect("Arg0"); //抽卡數量
            MyFuns.Instance.DrawCard(val);

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountIntellect("Arg0"))); // 字串
    }
    public override bool TryUse()
    {
        int cost = totalCost;
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
        else if (CountIntellect("Arg0") > (FightCardManager.Instance.cardList.Count + FightCardManager.Instance.usedCardList.Count))
        {
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("無法抽出需求數量", Color.red);
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
}
