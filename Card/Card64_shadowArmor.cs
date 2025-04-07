using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class Card64_shadowArmor : CardItem
{
    //獲得{0}護甲。技能後：此卡額外受到智力加成。攻擊後：額外獲得{1}護甲。
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = CountDefend("Arg0");
            FightManager.Instance.shieldCount += val;
            if (checkLastCardType("skill")) //技能後：此卡額外受到智力加成
            {
                FightManager.Instance.shieldCount += CountIntellect(0);
            }

            if (checkLastCardType("attack")) //攻擊後：額外獲得{1}護甲。
            {
                val = CountDisappoint(CountDefend("Arg1"));
                FightManager.Instance.shieldCount += val;
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
        if (checkLastCardType("skill")) //技能後：此卡額外受到智力加成
        {
            msgText.text = $"獲得{CRedT(CountIntellect(CountDefend("Arg0")))}護甲。技能後：此卡額外受到智力加成。{CGrayT($"攻擊後：額外獲得{CountDisappoint(CountDefend("Arg1"))}護甲。")}";
        }
        else if (checkLastCardType("attack"))
        {
            msgText.text = $"獲得{CRedT(CountDefend("Arg0"))}護甲。{CGrayT($"技能後：此卡額外受到智力加成。")}攻擊後：額外獲得{CRedT(CountDisappoint(CountDefend("Arg1")))}護甲。";
        }
        else
        {
            msgText.text = $"獲得{CRedT(CountDefend("Arg0"))}護甲。{CGrayT($"技能後：此卡額外受到智力加成。攻擊後：額外獲得{CountDisappoint(CountDefend("Arg1"))}護甲。")}";
        }
    }
}
