using System;
using UnityEngine.EventSystems;
public class Card31_DicePower : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得獲得3回合1脫力發動。擲骰，獲得骰數的力量。
            EffAndAudio();
            int Dice = UnityEngine.Random.Range(1, 7);
            if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
            {
                MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                Dice += 1;
            }
            //獲得骰數的力量
            FightManager.Instance.GetBuff(BuffType.power, 999, Dice);
            //獲得3回合 1脫力
            FightManager.Instance.GetDeBuff(DeBuffType.dePower, 3, 1);
            //刷新數值
            FightManager.Instance.SetBuffItem();//更新buff狀態

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }

    public override bool TryUse()
    {
        int cost = totalCost;
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", UnityEngine.Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("動點不足", UnityEngine.Color.red);
            return false;
        }
        else
        {
            //減少費用
            FightManager.Instance.CurMoveCount -= cost;
            //更新文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);

            return true;
        }
    }
}
