using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card25_Assassin : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack(CountAttack("Arg0"))}傷害";
        damageText.text += "\n此傷害無視護甲";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack(CountAttack("Arg0")), hitEnemy); //之後可以動態計算傷害
        damageText.text += "\n此傷害無視護甲";
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害。之後，回合結束。此傷害無視護甲，力量有兩倍效果。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountAttack(CountAttack("Arg0")); //兩倍力量
        penetrate = val; //無視護甲 直接吸血
        hitEnemy.InterHit_IsDeath(val); //無視護甲 直接扣除生命
        hitEnemy.updateShield();//更新護甲
        FatalAttackdetermination();
        CardEffectEnd(false);//卡片效果結束
    }
}
