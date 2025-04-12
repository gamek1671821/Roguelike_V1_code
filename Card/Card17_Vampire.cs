using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card17_Vampire : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg1")}傷害";
        damageText.text += "\n此傷害對護甲減半";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg1"), hitEnemy); //之後可以動態計算傷害
        damageText.text += "\n此傷害對護甲減半";
    }
    public override void CardEffect()
    {
        //指定一名敵人，失去{0}點生命發動。造成{1}點傷害。回復直擊的生命。此傷害對護甲減半。

        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int val = CountAttack("Arg1"); //傷害值
        int preHit = Mathf.Clamp(hitEnemy.CheckHit((int)(val * 0.5f)), 0, int.MaxValue); //預先計算會貫穿多少傷害 打在護甲上 傷害減半
        if (preHit == 0) hitEnemy.Hit((int)(val * 0.5f) , false); //沒有貫穿 
        else
        {
            hitEnemy.Hit(hitEnemy.shield + preHit * 2 , false);
        }
        MyFuns.Instance.RestoreHp(preHit * 2);
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
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
    public override bool TryUse()
    {
        int cost = totalCost;
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost >= FightManager.Instance.CurHp)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("生命不足", Color.red);
            return false;
        }
        else
        {
            //減少費用 -> 生命
           FightManager.Instance.InterHit_IsDeath(cost, true);
            MyFuns.Instance.ShowMessage($"失去{cost}生命");
            //更新文本 -> 更新生命文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
}
