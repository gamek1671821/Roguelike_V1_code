using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card60_crazyPosion : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得猛毒吸取。每當敵方受到劇毒傷害，獲得1生命。(每10劇毒傷害+1獲得生命)。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.crazyPosion, 999, 1);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
