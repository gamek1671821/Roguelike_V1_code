using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card40_BloodPowerAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
        damageText.text += sp();
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountDefend(CountAttack("Arg0")), hitEnemy); //之後可以動態計算傷害\
        damageText.text += sp();
    }
    public override void CardEffect()
    {
        //殘血時可以指定一敵人發動。造成{0}傷害。此傷害受力量、硬甲加成。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountDefend(CountAttack("Arg0")); //傷害值
        hitEnemy.Hit(val, false); //造成傷害
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override bool TryUse()
    {
          int cost = totalCost;
        int costPoisoned = FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned];
        bool isLowHp = FightManager.Instance.CurHp <= FightManager.Instance.MaxHp * 0.3f;
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("動點不足", Color.red);
            return false;
        }

        else if (!isLowHp) //不是低生命
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip($"尚未觸發殘血(30%生命) {(int)FightManager.Instance.MaxHp * 0.3f}", Color.red);
            return false;
        }
        else
        {
            //減少費用
            FightManager.Instance.CurMoveCount -= cost;
            //更新文本
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();
            //使用的卡牌刪除
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, data["isBanishCard"]);
            return true;
        }
    }
    private string sp()
    {
        if (FightManager.Instance.CurHp <= FightManager.Instance.MaxHp * 0.3f)
        {
            return $"{CRedT($"尚未觸發殘血(30%生命) {(int)FightManager.Instance.MaxHp * 0.3f}")}";
        }
        else
        {
            return "";
        }
    }
}
