using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card19_TigerPunch : CardItem, IPointerDownHandler
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
        //指定一敵人，獲得2力量與1回合2脫力發動。造成{0}傷害，直擊時附加1脫力。傷害對護甲無效。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int powerval = 2;

        FightManager.Instance.GetBuff(BuffType.power, 99, powerval); //獲得2力量
        FightManager.Instance.GetDeBuff(DeBuffType.dePower, 1, 2); //獲得2脫力

        int val = CountAttack("Arg0"); //傷害值

        penetrate = hitEnemy.Hit(val, false);
        hitEnemy.GetDeBuff(DeBuffType.dePower, 999, 1);

        FatalAttackdetermination();
        CardEffectEnd();//卡片效果結束
    }

}
