using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card11_ShieldAttack2 : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
        damageText.text += $"\n{CountDisappoint(CountDefend("Arg1"))}護甲"; //之後可以動態計算傷害
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += $"\n{CountDisappoint(CountDefend("Arg1"))}護甲"; //之後可以動態計算傷害
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害後獲得{1}護甲。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //傷害值
        penetrate = hitEnemy.Hit(val , false) ;
        FatalAttackdetermination();

        val = CountDisappoint(CountDefend("Arg1")); //護甲值
        FightManager.Instance.shieldCount += val;

        CardEffectEnd();//卡片效果結束
    }
}
