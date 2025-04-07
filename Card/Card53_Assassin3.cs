using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card53_Assassin3 : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
        damageText.text += "\n附加1暈眩";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += "\n附加1暈眩";
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害與1回合暈眩。之後，回合結束。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountAttack("Arg0");
        hitEnemy.InterHit_IsDeath(val); //無視護甲 直接扣除生命
        
        hitEnemy.GetDeBuff(DeBuffType.dizz, 1, 1); //給予暈眩
        FatalAttackdetermination();
        CardEffectEnd(false);//卡片效果結束

        DirectEnd(); //直接結束回合
    }
}
