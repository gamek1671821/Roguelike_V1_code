using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card04_BurnAttack : CardItem, IPointerDownHandler
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
        //指定一敵人。造成{0}傷害。附加{1}回合的直擊燒傷。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //攻擊

        int preHit = Mathf.Clamp(hitEnemy.CheckHit(val), 0, int.MaxValue); //預先計算會貫穿多少傷害，直擊傷害給予效果
        int deBuffTurn = int.Parse(data["Arg1"]); //debuff持續時間 

        hitEnemy.Hit(val,false);
        hitEnemy.GetDeBuff(DeBuffType.burn, deBuffTurn, preHit);
        FatalAttackdetermination();

        CardEffectEnd();//卡片效果結束
    }
}
