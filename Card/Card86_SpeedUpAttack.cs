using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card86_SpeedUpAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack(FightManager.Instance.CurMoveCount)}傷害"; //之後可以動態計算傷害
    }
    public override void OnPointDamageText()
    {
        damageText.text = $"{PointMessage(CountAttack(FightManager.Instance.CurMoveCount), hitEnemy)}"; //之後可以動態計算傷害
    }

    public override void CardEffect()
    {
        //<color=#7D7DFF>失去</color>所有動點，指定一敵人。造成<color=#7D7DFF>失去值</color>的物理傷害。獲得1回合<color=#7D7DFF>失去值+1</color>的加速。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        int power = FightManager.Instance.CurMoveCount;
        int val = CountAttack(power); //傷害值
        hitEnemy.Hit(val, false); //造成傷害
        FatalAttackdetermination(); //確認傷害是否致死
        FightManager.Instance.GetBuff(BuffType.Speed, 1, power + 1);
        FightManager.Instance.CurMoveCount = 0;
        CardEffectEnd(false);//卡片效果結束
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
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