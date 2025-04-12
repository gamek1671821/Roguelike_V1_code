using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card94_WindSmoke : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    // 將所有增益時間延長1回合，所有減益時間、效果縮短1回合。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            int val = CountDisappoint(CountDefend("Arg0"));
            FightManager.Instance.GetBuff(BuffType.light, 2, 1);
            FightManager.Instance.shieldCount += val;
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void BeforeEndEffect()
    {
        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, "B"); //強制被移除
    }
}
