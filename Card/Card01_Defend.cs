using UnityEngine;
using UnityEngine.EventSystems;

public class Card01_Defend : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}護甲。
            EffAndAudio();
            //使用效果
            int val = CountDisappoint(CountDefend("Arg0"));
            //增加護盾
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
        msgText.text = string.Format(data["Des"], CRedT(CountDisappoint(CountDefend("Arg0")))); // 字串
    }
}
