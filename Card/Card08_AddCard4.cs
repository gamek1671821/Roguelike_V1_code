using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card08_AddCard4 : CardItem, IPointerDownHandler
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
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        //指定一名敵人，從牌組抽出{0}張卡發動。從牌組抽出{1}張卡，造成{1}傷害
        int val = CountAttack("Arg1"); //傷害值
        penetrate = hitEnemy.Hit(val, false);
        FatalAttackdetermination();

        val = int.Parse(data["Arg0"]) + int.Parse(data["Arg1"]); //抽卡數量
        MyFuns.Instance.DrawCard(val);
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();

        FatalAttackdetermination(); //確認傷害是否致死

        CardEffectEnd();//卡片效果結束
    }
}
