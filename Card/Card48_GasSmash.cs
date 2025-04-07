using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card48_GasSmash : CardItem, IPointerDownHandler
{
    // 指定一敵人。給予目標<color=#7D7DFF>劇毒值</color>劇毒。目標劇毒10：給予的劇毒擃散。
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"給予劇毒";
    }
    public override void OnPointDamageText()
    {
        damageText.text = sp();//之後可以動態給予劇毒
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountPowerPoisoned(hitEnemy.deBuffsVal[(int)DeBuffType.poisoned]); //傷害值 (根據對手劇毒)
        if (hitEnemy.deBuffsVal[(int)DeBuffType.poisoned] >= 10)
        {
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.GetDeBuff(DeBuffType.poisoned, 3, val); //給予全體劇毒
            }
        }
        else
        {
            hitEnemy.GetDeBuff(DeBuffType.poisoned, 3, val); //給予劇毒
        }
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    private string sp()
    {
        if (hitEnemy.deBuffsVal[(int)DeBuffType.poisoned] >= 10)
        {
            return $"給予劇毒{hitEnemy.deBuffsVal[(int)DeBuffType.poisoned]} [擴散]";
        }
        else
        {
            return $"給予劇毒{hitEnemy.deBuffsVal[(int)DeBuffType.poisoned]} ";
        }
    }
}
