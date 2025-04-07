using System.Diagnostics;
using UnityEngine.EventSystems;

public class Card47_GasShield : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            int val = CountPowerPoisoned("Arg0");
            int val2 = CountDisappoint(CountDefend("Arg1"));
            //給予全體{0}劇毒與{1}護甲。防禦後：改為猛毒只對敵人生效、護甲只對角色生效。
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.GetDeBuff(DeBuffType.poisoned, 3, val);
            }
            FightManager.Instance.shieldCount += val2;
            if (!checkLastCardType("defend")) //不是防禦後 
            {
                foreach (var enemy in EnemyManager.Instance.enemyList)
                {
                    enemy.shield += val2;
                    enemy.updateShield();
                }
                FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 3, val);
            }
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }//給予全體3回合{0}劇毒與{1}護甲。防禦卡後：改為猛毒只對敵人生效、護甲只對角色生效。
    public override void DragMsgChange()
    {//參數0會改變
        if (checkLastCardType("defend")) //防禦後 
        {
            msgText.text = $"給予全體3回合{CRedT(CountPowerPoisoned("Arg0"))}劇毒與{CRedT(CountDisappoint(CountDefend("Arg1")))}護甲。{CRedT("防禦卡後：改為猛毒只對敵人生效、護甲只對角色生效。")}";
        }
        else
        {
            msgText.text = $"給予全體3回合{CRedT(CountPowerPoisoned("Arg0"))}劇毒與{CRedT(CountDisappoint(CountDefend("Arg1")))}護甲。{CGrayT("防禦卡後：改為猛毒只對敵人生效、護甲只對角色生效。")}";
        }

    }
}
