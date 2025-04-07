using UnityEngine.EventSystems;
public class Card28_AllChi : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得3力量、智力
            EffAndAudio();
            //獲得力量與智力
            int powerval = int.Parse(data["Arg0"]);
            if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
            {
                MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                powerval += 1;
            }
            FightManager.Instance.GetBuff(BuffType.power, 99, powerval);
            FightManager.Instance.GetBuff(BuffType.intellect, 99, int.Parse(data["Arg0"]));
            //刷新數值
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
