using UnityEngine;
using UnityEngine.EventSystems;


public class Card35_Rebound : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {   //獲得{0}<color=#7D7DFF>尖刺</color>與{1}護甲。
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            //獲得反彈
            FightManager.Instance.GetBuff(BuffType.rebound, 999, val);
            val = CountDisappoint(CountDefend("Arg1"));
            FightManager.Instance.shieldCount += val;
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(int.Parse(data["Arg0"])), CRedT(CountDisappoint(CountDefend("Arg1")))); // 字串
    }
}
