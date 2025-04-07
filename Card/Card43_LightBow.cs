using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card43_LightBow : CardItem, IPointerDownHandler
{
    // Start is called before the first frame update
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountIntellect("Arg0")}傷害";
        damageText.text += sp();
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountIntellect("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += sp();
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}法術傷害。技能後：此卡返回手牌。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountIntellect("Arg0"); //傷害值
        hitEnemy.Hit(val , false); //造成傷害

        if (checkLastCardType("skill")) NoThrowAway();

        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    private string sp()
    {
        if (checkLastCardType("skill"))
        {
            return "此卡返回手牌";
        }
        else
        {
            return "";
        }
    }
}
