using UnityEngine;
using UnityEngine.EventSystems;
public class Card15_BloodShield : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //失去5點生命發動。獲得{0}護甲。硬甲有兩倍效果。
            EffAndAudio();
            //使用效果
            int val = CountDisappoint(CountDefend(CountDefend("Arg0")));
            //增加護盾
            FightManager.Instance.shieldCount += val;
            //刷新數值
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountDisappoint(CountDefend(CountDefend("Arg0"))))); // 字串
    }

    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost >= FightManager.Instance.CurHp)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("生命不足", Color.red);
            return false;
        }
        else
        {
            //減少費用 -> 生命
           FightManager.Instance.InterHit_IsDeath(cost, true);
             MyFuns.Instance.ShowMessage($"失去{cost}生命");
            //更新文本 -> 更新生命文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
}
