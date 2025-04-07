using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card65_Meteoritefall : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    //獲得下回合99暈眩。對全部敵人造成{0}法術傷害，並附加{1}點燃與{2}劇毒。之後，回合結束。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = CountIntellect("Arg0");
            int val1 = int.Parse(data["Arg1"]);
            int val2 = CountPowerPoisoned("Arg2");
            //發動效果 {對全部敵人造成傷害}
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.Hit(val, true);
                enemy.GetDeBuff(DeBuffType.burn, 10, val1);
                enemy.GetDeBuff(DeBuffType.poisoned, 10, val2);
            }
            FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)
            FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, 99);
            CardEffectEnd(false);//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountIntellect("Arg0")), CRedT(int.Parse(data["Arg1"])), CRedT(CountPowerPoisoned("Arg2"))); // 字串as
    }
}