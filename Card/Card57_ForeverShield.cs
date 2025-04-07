using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card57_ForeverShield: CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
     public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得護甲不滅。使角色的護甲不會在回合開始時消失。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.ForeverShield, 999, 1);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
