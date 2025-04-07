using UnityEngine;
using UnityEngine.EventSystems;

public class Card13_Energy : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //失去{0}點生命發動。獲得失去值的動點。
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            //增加能量
            FightManager.Instance.CurMoveCount += val;

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }

    public override bool TryUse()
    {
        int cost = int.Parse(data["Arg0"]);
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
