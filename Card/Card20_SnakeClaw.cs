using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card20_SnakeClaw : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
        damageText.text += "\n此傷害對護甲減半";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
        damageText.text += "\n此傷害對護甲減半";
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害，附加{1}回合2倍直擊的[劇毒]。傷害對護甲減半。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountAttack("Arg0"); //傷害值
        int preHit = Mathf.Clamp(hitEnemy.CheckHit((int)(val * 0.5f)), 0, int.MaxValue); //預先計算會貫穿多少傷害 打在護甲上 傷害減半
        if (preHit == 0) hitEnemy.Hit((int)(val * 0.5f), false); //沒有貫穿 
        else
        {
            penetrate = hitEnemy.Hit(hitEnemy.shield + preHit * 2, false);
            int deBuffTurn = int.Parse(data["Arg1"]); //buff持續時間 
            hitEnemy.GetDeBuff(DeBuffType.poisoned, deBuffTurn, CountPowerPoisoned(preHit * 2)); //給予中毒 (貫穿傷害的兩倍再加上猛毒)
        }
        FatalAttackdetermination();

        CardEffectEnd();//卡片效果結束
    }
    public override string PointMessage(int damage, Enemy enemy)
    {
        string mess;
        int baseCount = (int)(damage * 0.5f) - enemy.shield;
        if (baseCount >= 0) mess = $"<color=red>{damage}</color>傷害 [貫穿]";
        else mess = $"<color=red>{baseCount}</color>傷害";

        return mess;
    }
}
