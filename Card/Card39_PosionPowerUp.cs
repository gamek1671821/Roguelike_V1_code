using UnityEngine.EventSystems;
using UnityEngine;
//<color=#7D7DFF>失去</color>劇毒值的生命。獲得劇毒值一半的猛毒與力量。之後，失去所有劇毒。

public class Card39_PosionPowerUp : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}護甲。
            EffAndAudio();
            //使用效果
            int Poisoned = FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned]; // 獲得劇毒疊層
            FightManager.Instance.GetBuff(BuffType.powerpoisoned, 999, (int)(Poisoned * 0.5f)); //根據劇毒疊層獲得猛毒

            int powerval = (int)(Poisoned * 0.5f);
            if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
            {
                MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                powerval += 1;
            }
            FightManager.Instance.GetBuff(BuffType.power, 999, powerval); //根據劇毒疊層獲得猛毒
            FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned] = 0; // 獲得劇毒疊層歸零

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData); //powerpoisoned
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        //msgText.text = string.Format(data["Des"], "<color=red>" + CountDefend("Arg0") + "</color>"); // 字串
    }
    public override bool TryUse()
    {
        int cost = totalCost;
        int costPoisoned = FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned];
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("動點不足", Color.red);
            return false;
        }

        else if (costPoisoned >= FightManager.Instance.CurHp) //需要消耗生命
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("生命不足", Color.red);
            return false;
        }
        else
        {
            //減少費用 -> 生命
            FightManager.Instance.InterHit_IsDeath(cost, true);
            MyFuns.Instance.ShowMessage($"失去{cost}生命");

            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
}
