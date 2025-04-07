using UnityEngine.EventSystems;
public class Card24_CondenseShield : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}次{1}護甲，並複製{0}張此卡於棄牌堆。次數與複製受智力加成。
            EffAndAudio();
            //使用效果
            int val = CountDefend("Arg1"); //
            //隨機攻擊
            for (int i = 0; i < CountIntellect("Arg0"); i++) //施放次數
            {
                FightManager.Instance.shieldCount += val;
                FightCardManager.Instance.usedCardList.Add("1024");//每次攻擊 額外將此卡放入棄排堆
            }
            //刷新數值
            CardEffectEnd();//卡片效果結束

        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountIntellect("Arg0")), CountAttack("Arg1")); // 字串

    }
}
