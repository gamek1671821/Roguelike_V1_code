using UnityEngine.EventSystems;

public class Card44_Defend2 : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}護甲。額外獲得{1}護甲。額外護甲根據敵人數量提升。
            EffAndAudio();
            //使用效果
            int val = CountDisappoint(CountDefend("Arg0"));
            int val2 = CountDisappoint(CountDefend("Arg1"));
            //增加護盾
            FightManager.Instance.shieldCount += val + val2 * EnemyManager.Instance.enemyList.Count;

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountDisappoint(CountDefend("Arg0"))), CRedT(CountDisappoint(CountDefend("Arg1")))); // 字串as
    }
}
