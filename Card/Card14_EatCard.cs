using UnityEngine;
using UnityEngine.EventSystems;

public class Card14_EatCard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        //隨機銷毀牌組3張牌發動。角色回復5點生命。
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            Debug.Log(int.Parse(data["Arg0"]));

            for (int i = 0; i < int.Parse(data["Arg0"]); i++)
            {
                FightCardManager.Instance.banishCardList.Add(FightCardManager.Instance.cardList[0]);// 牌頂添加至除外區
                FightCardManager.Instance.cardList.RemoveAt(0); //移除牌頂
                FightManager.Instance.thisTurnDestroyCount++; //移除計數器加1
            }
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardCount(); //更新卡牌數量
            FightManager.Instance.CurHp += int.Parse(data["Arg1"]); //獲得生命
                                                                    //刷新數值
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("費用不足", Color.red);
            return false;
        }
        else if (FightCardManager.Instance.cardList.Count < 3)
        {
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("無法摧毀需求數量", Color.red);
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
