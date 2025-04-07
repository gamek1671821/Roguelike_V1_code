using UnityEngine.EventSystems;

public class Card27_Gas : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //對敵我全體附加{0}的劇毒。智力3：改為只對敵人生效。
            int val = CountPowerPoisoned(CountIntellect("Arg0")); //
            if (FightManager.Instance.buffsVal[(int)BuffType.intellect] < 3) //如果智力少於3
                FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 999, val); //獲得同等劇毒
            else
            {
                //FightManager.Instance.buffsVal[(int)BuffType.intellect]--; //減少1智力
                FightManager.Instance.SetBuffItem();
            }
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.GetDeBuff(DeBuffType.poisoned, 999, val);
            }
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//更新卡牌位置
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
     //msgText.text = string.Format(data["Des"], "<color=red>" + CountDefend(CountIntellect(CountAttack("Arg0"))) + "</color>", data["Arg1"]); // 字串
        if (FightManager.Instance.buffsVal[(int)BuffType.intellect] < 3)
            msgText.text = $"對敵我全體附加3回合{CRedT(CountPowerPoisoned(CountIntellect("Arg0")))}劇毒。{CGrayT("智力3：改為只對敵人生效。")}";
        else
            msgText.text = $"對{CGrayT("敵我全體")}敵方全體附加3回合{CRedT(CountPowerPoisoned(CountIntellect("Arg0")))}劇毒。<u>智力3：改為只對敵人生效，智力降低1。</u>";
    }

}
