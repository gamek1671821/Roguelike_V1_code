using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card66_Timer : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    // 將所有增益時間延長1回合，所有減益時間、效果縮短1回合。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            for (int i = 0; i < FightManager.Instance.buffsTurn.Count; i++)
            {
                if (FightManager.Instance.buffsTurn[i] > 0)
                {
                    FightManager.Instance.buffsTurn[i] -= 1;
                }
            }
            for (int i = 0; i < FightManager.Instance.deBuffsItem.Count; i++)
            {
                FightManager.Instance.deBuffsTurn[i] -= 1;
                FightManager.Instance.deBuffsVal[i] -= 1;
            }
            FightManager.Instance.SetBuffItem();
            FightManager.Instance.SetDeBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }

}
