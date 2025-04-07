using UnityEngine.EventSystems;


public class Card09_Intellect : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {//獲得能量值的智力。之後失去所有能量並使回合結束。
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //獲得精智力
            FightManager.Instance.GetBuff(BuffType.intellect, 999, FightManager.Instance.CurMoveCount);
            FightManager.Instance.CurMoveCount = 0;
            //更新文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();

            CardEffectEnd(false);//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
