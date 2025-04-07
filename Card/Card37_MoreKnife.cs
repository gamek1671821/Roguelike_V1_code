using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card37_MoreKnife : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果

            int count = CountIntellect("Arg0");

            MyFuns.Instance.GetKnife(count);

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountIntellect("Arg0"))); // 字串
    }
}