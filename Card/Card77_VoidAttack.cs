using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card77_VoidAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountIntellect("Arg0")}傷害 [附加1回合暈眩]";
    }
    public override void OnPointDamageText()
    {
        damageText.text = $"{PointMessage(CountIntellect("Arg0"), hitEnemy)}  [附加1回合暈眩]"; //之後可以動態計算傷害
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}法術傷害並附加1回合暈眩。結束時持有：此卡消失。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountIntellect("Arg0"); //傷害值
        hitEnemy.Hit(val, false); //造成傷害
        hitEnemy.GetDeBuff(DeBuffType.dizz, 1, 1);//賦予1暈眩
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override void BeforeEndEffect()
    {
        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, "B"); //
    }
}
