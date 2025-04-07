using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card12_corrosion : CardItem, IPointerDownHandler
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
        //指定一敵人。造成{0}傷害。傷害對護甲加倍。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountIntellect("Arg0"); //傷害值

        int preHit = Mathf.Clamp(hitEnemy.CheckHit(val * 2), 0, int.MaxValue); //預先計算兩倍傷害打在護甲上會貫穿多少
        if (preHit == 0) hitEnemy.Hit(val * 2, false); //沒有貫穿 兩倍傷害打在護甲
        else
        {
            hitEnemy.shield = 0;
            hitEnemy.Hit((int)(preHit / 2), false); //貫穿後傷害 兩倍傷害回調
        }

        FatalAttackdetermination();

        CardEffectEnd();//卡片效果結束
    }
    public override string PointMessage(int damage, Enemy enemy)
    {
        string mess;
        int baseCount = damage * 2 - enemy.shield;
        if (baseCount >= 0) mess = $"<color=red>{damage}</color>傷害 [貫穿]";
        else mess = $"<color=red>{damage}</color>傷害";

        return mess;
    }
}
