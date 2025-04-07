using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card71_LuckyDefend : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}護甲。每1幸運+10%機率額外獲得{1}護甲。額外護甲不受破滅影響。
            EffAndAudio();
            //使用效果
            int val = CountDisappoint(CountDefend("Arg0"));
            //增加護盾
            FightManager.Instance.shieldCount += val;

            int val1 = CountDefend("Arg1");
            if (LuckyProbability(0, 10)) //基礎機率0 , 每1幸運+10%機率
            {
                FightManager.Instance.shieldCount += val1;
            }

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountDisappoint(CountDefend("Arg0"))), CRedT(CountDefend("Arg1"))); // 字串
    }
}
