using UnityEngine;
using UnityEngine.EventSystems;
public class Card16_Boom : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //獲得2回合<color=#7D7DFF>固定3點劇毒</color>發動。對全部敵人造成{0}傷害。此傷害受力量、智力、硬甲、猛毒加成。智力3：不須發動代價。
            //使用效果 
            if (FightManager.Instance.buffsVal[(int)BuffType.intellect] < 3) FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 2, 3); // 沒有3點智力，獲得3負面狀態
            int val = CountPowerPoisoned(CountDefend(CountIntellect(CountAttack("Arg0"))));
            //發動效果 {對全部敵人造成傷害}
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.Hit(val, true);
            }
            FatalAttackdetermination();
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
     //msgText.text = string.Format(data["Des"], "<color=red>" + CountDefend(CountIntellect(CountAttack("Arg0"))) + "</color>", data["Arg1"]); // 字串
        if (FightManager.Instance.buffsVal[(int)BuffType.intellect] < 3)
            msgText.text = $"獲得2回合<color=#7D7DFF>固定3點劇毒</color>發動。對全部敵人造成{CountPowerPoisoned(CountDefend(CountIntellect(CountAttack("Arg0"))))}傷害。此傷害受力量、智力、硬甲、猛毒加成。智力3：不須發動代價。";
        else
            msgText.text = $"{CGrayT("獲得2回合固定3點劇毒發動。")}對全部敵人造成{CountPowerPoisoned(CountDefend(CountIntellect(CountAttack("Arg0"))))}傷害。此傷害受力量、智力、硬甲、猛毒加成。{CGrayT("智力3：不須發動代價。")}";
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
