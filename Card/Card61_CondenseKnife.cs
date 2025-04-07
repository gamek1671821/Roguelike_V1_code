using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card61_CondenseKnife : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得凝聚刀刃。每次回合開始時，額外獲得1隨機小刀。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.CondenseKnife, 999, 1);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
