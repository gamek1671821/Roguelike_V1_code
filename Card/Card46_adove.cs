using System.Diagnostics;
using UnityEngine.EventSystems;

public class Card46_adove : CardItem

{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}動點，{1}護甲，1回合{0}暈眩。
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            FightManager.Instance.CurMoveCount += val;
            //增加護盾 
            int val2 = CountDisappoint(CountDefend("Arg1"));
            FightManager.Instance.shieldCount += val2;
            //獲得暈眩
            FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, val);

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(int.Parse(data["Arg0"])), CRedT(CountDisappoint(CountDefend("Arg1")))); // 字串as
    }
}
