using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card59_crazyPower : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得增長力量。使角色每次回合開始獲得1力量，同時失去2生命。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.crazyPower, 999, 1);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
