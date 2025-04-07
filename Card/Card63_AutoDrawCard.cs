using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card63_AutoDrawCard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得1回合抽牌3、加速2。不受任何加成影響。
            EffAndAudio();
            //使用效果
            FightManager.Instance.GetBuff(BuffType.Draw, 1, 3);
            FightManager.Instance.GetBuff(BuffType.Speed, 1, 2);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
