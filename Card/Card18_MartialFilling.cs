using UnityEngine.EventSystems;

public class Card18_MartialFilling : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得2力量、硬甲與1回合2脫力、崩甲。
            EffAndAudio();
            //獲得攻擊力
            int val = int.Parse(data["Arg0"]);
            int powerVal = val;
            FightManager.Instance.GetBuff(BuffType.power, 99, powerVal);
            //獲得硬甲
            FightManager.Instance.GetBuff(BuffType.hard, 99, val);
            //獲得1回合 脫力
            FightManager.Instance.GetDeBuff(DeBuffType.dePower, 1, val);
            //獲得1回合 
            FightManager.Instance.GetDeBuff(DeBuffType.deHard, 1, val);
            //刷新數值
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
