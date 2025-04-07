using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card52_steal : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"";
    }
    public override void OnPointDamageText()
    {
        damageText.text = $"";
    }
    public override void CardEffect()
    {
        //指定一敵人。獲得等同目標護甲，並以{0}%偷取少量金幣。之後，目標護甲全部消失。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        //獲得
        FightManager.Instance.shieldCount += CountDisappoint(hitEnemy.shield);

        int val = int.Parse(data["Arg0"]) + 5 * FightManager.Instance.buffsVal[(int)BuffType.Lucky]; // 機率
        int Rd = Random.Range(0, 100);
        if (val > Rd)
        {   //獲得金幣
            MyFuns.Instance.GetGold(int.Parse(hitEnemy.data["mingold"]));
        }

        hitEnemy.shield = 0;
        hitEnemy.updateShield();

        CardEffectEnd();//卡片效果結束
    }
}
