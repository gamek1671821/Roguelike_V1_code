using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card49_AutoMagic : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //獲得{0}護甲並複製{1}張此卡到手中。攻擊後：複製次數改為0。結束時持有：對隨機敵人造成{2}傷害。傷害僅受尖刺影響。
            EffAndAudio();
            //使用效果
            int val = CountDisappoint(CountDefend("Arg0"));
            //增加護盾
            FightManager.Instance.shieldCount += val;
            int val2 = CountIntellect("Arg1");
            if (checkLastCardType("attack")) //攻擊：攻擊後：複製次數改為0
            {
                val2 = 0;
            }
            for (int i = 0; i < val2; i++)
            {
                MyFuns.Instance.PutCardOnDeck(1049); //牌堆上方追加
                MyFuns.Instance.DrawCard(1);//抽牌 
            }
            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        if (checkLastCardType("attack")) //攻擊：
        {
            msgText.text = $"獲得{CRedT(CountDisappoint(CountDefend("Arg0")))}護甲並複製{CRedT("0")}張此卡到手中。{CRedT("攻擊後：複製次數改為0")}。結束時持有：對隨機敵人造成{CRedT(CountRebound("Arg2"))}傷害。傷害僅受尖刺影響。";
        }
        else
        {
            msgText.text = $"獲得{CRedT(CountDisappoint(CountDefend("Arg0")))}護甲並複製{CRedT(CountIntellect("Arg1"))}張此卡到手中。{CGrayT("攻擊後：複製次數改為0")}。結束時持有：對隨機敵人造成{CRedT(CountRebound("Arg2"))}傷害。傷害僅受尖刺影響。";
        }
    }
    public override void BeforeEndEffect()
    {
        int val = CountRebound("Arg2");
        if (EnemyManager.Instance.enemyList.Count >= 0)
        {
            Enemy enemy = EnemyManager.Instance.enemyList[Random.Range(0, EnemyManager.Instance.enemyList.Count)]; //隨機抽一個敵人
            enemy.Hit(val, true);
            FatalAttackdetermination(); //每次攻擊後 確認是否致死
        }
    }
}
