using UnityEngine.EventSystems;
public class Card07_Hard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}硬甲。
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            //獲得防禦力(硬甲)
            FightManager.Instance.GetBuff(BuffType.hard, 999, val);
            
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
