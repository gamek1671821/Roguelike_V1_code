using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card74_LuckyMagic : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountIntellect("Arg0")}傷害";
        damageText.text += $"+{CountIntellect("Arg1")}傷害";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountIntellect("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += $"+{PointMessage(CountIntellect("Arg1"), hitEnemy)}";
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}法術傷害。每1幸運+10%機率額外造成{1}法術傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountIntellect("Arg0"); //傷害值
        hitEnemy.Hit(val, false); //造成傷害
        int val1 = CountIntellect("Arg1"); //額外傷害
        if (LuckyProbability(0, 10)) //基礎機率0 , 每1幸運+10%機率
        {
            hitEnemy.Hit(val1, true); //造成額外傷害
        }
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }

}
