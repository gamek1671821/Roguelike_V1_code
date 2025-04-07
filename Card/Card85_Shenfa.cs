using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card85_Shenfa : CardItem, IPointerDownHandler
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            // 獲得下回合破滅。將護甲值加倍。
            EffAndAudio();
            AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
            //使用效果
            int val = FightManager.Instance.shieldCount; //獲得護甲
            FightManager.Instance.shieldCount += val; //護甲 加倍

            FightManager.Instance.GetDeBuff(DeBuffType.disappoint, 1, 1);
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}