using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card51_LuckyKiss : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    // 獲得{0}幸運。結束時：對全部敵人造成{1}傷害，並獲得{2}護甲。
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //清除所有負面效果
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]);
            FightManager.Instance.GetBuff(BuffType.Lucky, 999, val);
            FightManager.Instance.SetBuffItem();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變

        msgText.text = string.Format(data["Des"], CRedT(int.Parse(data["Arg0"])), CRedT(CountLucky("Arg1")), CRedT(CountDisappoint(CountLucky("Arg2")))); // 字串as
    }
    public override void BeforeEndEffect()
    {
        int val = CountLucky("Arg1");
        foreach (var enemy in EnemyManager.Instance.enemyList)
        {
            enemy.Hit(val, true);
        }
        FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)

        int val2 = CountDisappoint(CountLucky("Arg2"));
        FightManager.Instance.shieldCount += val2;
    }
}
