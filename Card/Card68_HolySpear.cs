using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card68_HolySpear : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountIntellect("Arg0")}傷害";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountIntellect("Arg0"), hitEnemy); //之後可以動態計算傷害
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}法術傷害。此傷害無視護甲。此傷害同時對護甲造成傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)


        int val = CountIntellect("Arg0"); //傷害值
        hitEnemy.InterHit_IsDeath(val); //造成直接傷害
        hitEnemy.shield = Mathf.Clamp(hitEnemy.shield - val, 0, int.MaxValue);
        
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
}
