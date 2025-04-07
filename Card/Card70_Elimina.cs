using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;



public class Card70_Elimina : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    //消耗所有能量發動並獲得1回合消耗能量的智壞。對全部敵人造成{0}x消耗能量的法術傷害。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = CountIntellect("Arg0"); //受智力影響
            int cost = FightManager.Instance.CurMoveCount;
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.Hit(val * cost, true);
            }
            FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)
            FightManager.Instance.CurMoveCount = 0;
            FightManager.Instance.GetDeBuff(DeBuffType.deIntellect, 1, cost);
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = $"<color=#7D7DFF>失去</color>所有能量發動並獲得1回合<color=#7D7DFF>失去值</color>的智壞。對全部敵人造成{CRedT(CountIntellect("Arg0"))}x<color=#7D7DFF>失去值</color>({CRedT(CountIntellect("Arg0") * FightManager.Instance.CurMoveCount)})的法術傷害。";
    }
}
