using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class Card50_UrgentClean : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //清除所有負面效果
            EffAndAudio();
            //使用效果
            for (int i = 0; i < FightManager.Instance.deBuffsVal.Count; i++)
            {
                FightManager.Instance.deBuffsVal[i] = 0;
            }
            FightManager.Instance.SetDeBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
