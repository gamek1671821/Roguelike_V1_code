using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card79_FireAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountIntellect("Arg0")}傷害 [全體附加{CountPowerPoisoned("Arg1")}燃燒]";
    }
    public override void OnPointDamageText()
    {
        damageText.text = $"{PointMessage(CountIntellect("Arg0"), hitEnemy)}  [全體附加{CountPowerPoisoned("Arg1")}燃燒]"; //之後可以動態計算傷害
    }

    public override void CardEffect()
    {
        //指定一敵人。造成{0}法術傷害並對全部敵人附加{1}劇毒。結束時持有：此卡消失。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountIntellect("Arg0"); //傷害值
        hitEnemy.Hit(val, false); //造成傷害
        int val1 = int.Parse(data["Arg1"]); //燃燒
        foreach (var enemy in EnemyManager.Instance.enemyList)
        {
            enemy.GetDeBuff(DeBuffType.burn, 5, val1);//賦予
            //enemy.Hit(0 );
        }
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override void BeforeEndEffect()
    {
        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, "B"); //強制被移除
    }
}
