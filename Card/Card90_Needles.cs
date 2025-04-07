using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card90_Needles : CardItem, IPointerDownHandler
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
        if (hitEnemy.shield > 0) damageText.text = $"傷害對護甲無效";//無傷害
        else
        {
            damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
            if (hitEnemy.data["heart"] == "T") damageText.text += "\n血肉生命體";
        }
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}物理傷害。對血肉生命體直擊時：給予目標1脫力。傷害對護甲無效。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountAttack("Arg0"); //傷害值

        if (hitEnemy.shield > 0) { }//無傷害
        else
        {
            hitEnemy.Hit(val , false);
            if (hitEnemy.data["heart"] == "T")
                hitEnemy.GetDeBuff(DeBuffType.dePower, 99, 1);
        }
        FatalAttackdetermination();

        CardEffectEnd();//卡片效果結束
    }

}