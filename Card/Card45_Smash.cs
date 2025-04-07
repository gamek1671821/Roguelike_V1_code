using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card45_Smash : CardItem, IPointerDownHandler
{
    
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{FightManager.Instance.shieldCount}傷害";
        damageText.text += sp();
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(FightManager.Instance.shieldCount, hitEnemy); //之後可以動態計算傷害
        damageText.text += sp();
    }
    public override void CardEffect()
    {
        //指定一敵人。造成<color=#7D7DFF>護甲值</color>的傷害。防禦卡3：附加1回合暈眩，但回合結束。不受任何加成影響。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = FightManager.Instance.shieldCount; //傷害值
        if (FightManager.Instance.thisTurnDefendCount >= 3) //防禦3
        {
            hitEnemy.GetDeBuff(DeBuffType.dizz, 1, 1);
        }
        hitEnemy.Hit(val , false); //造成傷害
        FatalAttackdetermination(); //確認傷害是否致死
        if (FightManager.Instance.thisTurnDefendCount >= 3 && EnemyManager.Instance.enemyList.Count > 0) //防禦3 且 有敵人
        {
            CardEffectEnd(false);//卡片效果結束
        }
        else
        {
            CardEffectEnd();//卡片效果結束
        }
    }
    private string sp()
    {
        if (FightManager.Instance.thisTurnDefendCount >= 3)
        {
            return "\n附加1回合暈眩且回合結束。";
        }
        else
        {
            return "";
        }
    }
}
