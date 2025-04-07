using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card58_crazyIntellect : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得增長智慧。使角色每次回合開始獲得1智慧，同時失去2生命。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.crazyIntellect, 999, 1);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
