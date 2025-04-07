using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card38_KnifePosion : CardItem, IPointerDownHandler
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
        //指定一敵人。造成{0}物理傷害。<color=#7D7DFF>直擊時</color>附加{1}劇毒。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //攻擊
        int val1 = CountPowerPoisoned("Arg1");
        int preHit = Mathf.Clamp(hitEnemy.CheckHit(val), 0, int.MaxValue); //預先計算會貫穿多少傷害，直擊傷害給予效果
        if (preHit > 0) //如果貫穿 給予
            hitEnemy.GetDeBuff(DeBuffType.poisoned, 1, val1);
        hitEnemy.Hit(val, false);
        FatalAttackdetermination();

        CardEffectEnd();//卡片效果結束
    }
}
