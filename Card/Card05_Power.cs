using UnityEngine;
using UnityEngine.EventSystems;

public class Card05_Power : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {   //失去{Expend}點生命發動。獲得{0}力量。
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg1"]);
            //獲得攻擊力
            if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
            {
                MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                val += 1;
            }
            FightManager.Instance.GetBuff(BuffType.power, 999, val);
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }

    public override bool TryUse()
    {
        int cost = totalCost;
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
            FightManager.Instance.canUseCard = false;
            //減少費用 -> 生命
            FightManager.Instance.InterHit_IsDeath(cost, true);
            MyFuns.Instance.ShowMessage($"失去{cost}生命");

            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
}
