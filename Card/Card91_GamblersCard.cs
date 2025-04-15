using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card91_GamblersCard : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
    }


    public override void CardEffect()
    {
        //指定一敵人。造成{0}物理傷害。之後，獲得{1}幸運。攻擊卡7：額外獲得{2}幸運。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //傷害值
        penetrate = hitEnemy.Hit(val, false); //造成傷害
        FatalAttackdetermination(); //確認傷害是否致死

        val = int.Parse(data["Arg1"]);
        FightManager.Instance.GetBuff(BuffType.Lucky, 99, val);
        if (FightManager.Instance.thisTurnAttackCount >= 7) //攻擊卡7
        {
            val = int.Parse(data["Arg2"]);
            FightManager.Instance.GetBuff(BuffType.Lucky, 99, val);
        }
        CardEffectEnd();//卡片效果結束
    }

}