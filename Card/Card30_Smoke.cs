using System;
using UnityEngine.EventSystems;

public class Card30_Smoke : CardItem
{
     public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}點<color=#7D7DFF>生命</color>，並{1}%機率獲得1固定劇毒發動。獲得1智力。之後，回合結束並跳過敵人的回合。
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            //增加生命
             MyFuns.Instance.RestoreHp(val);
            //刷新數值
            FightManager.Instance.GetBuff(BuffType.intellect, 999, 1);//智力+1

            int val1 = int.Parse(data["Arg1"]) - CountLucky(0);
            int Random50 = UnityEngine.Random.Range(0, 100);
            if (Random50 > val1) FightManager.Instance.GetDeBuff(DeBuffType.poisoned, val, 1); //50%機率 獲得1點劇毒

            FightManager.Instance.skipEnemyTurn = true;

            CardEffectEnd(false);//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(int.Parse(data["Arg0"])), CRedT(int.Parse(data["Arg1"]) - CountLucky(0))); // 字串
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
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
