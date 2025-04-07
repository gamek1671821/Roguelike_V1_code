using UnityEngine;
using UnityEngine.EventSystems;
public class Card29_Intellect2 : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //失去2點生命發動。獲得2智力與1回合2智壞。
            EffAndAudio();
            //獲得智力
            FightManager.Instance.GetBuff(BuffType.intellect, 999, 2);
            //獲得1回合 智壞
            FightManager.Instance.GetDeBuff(DeBuffType.deIntellect, 1, 2);
            //刷新數值
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
