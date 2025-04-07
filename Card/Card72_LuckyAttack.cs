using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card72_LuckyAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
        damageText.text += $"+{CountAttack("Arg1")}傷害";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += $"+{PointMessage(CountAttack("Arg1"), hitEnemy)}";
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}物理傷害。每1幸運+10%機率額外造成{1}物理傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //傷害值
        hitEnemy.Hit(val , false); //造成傷害
        int val1 = CountAttack("Arg1"); //額外傷害
        if (LuckyProbability(0, 10)) //基礎機率0 , 每1幸運+10%機率
        {
            hitEnemy.Hit(val1 , false); //造成額外傷害
        }
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }

}
