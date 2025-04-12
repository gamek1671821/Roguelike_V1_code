using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card95_HeavyShield : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    // 將所有增益時間延長1回合，所有減益時間、效果縮短1回合。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            int val = CountDisappoint(CountDefend(CountDefend("Arg0")));
            int power = FightManager.Instance.buffsVal[(int)BuffType.power];
            int heavy = Mathf.Clamp(5 - power, 0, 99);
            FightManager.Instance.GetDeBuff(DeBuffType.heavy, 2, heavy);
            FightManager.Instance.shieldCount += val;
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
