using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card32_Attack2 : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"{CountAttack("Arg0")}傷害";
    }
    public override void OnPointDamageText()
    {
        damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = CountAttack("Arg0"); //傷害值
        if (FightManager.Instance.thisTurnAttackCount >= 3) //當攻擊3次以上
            hitEnemy.Hit(2 * val, false); //造成兩倍傷害
        else
            hitEnemy.Hit(val, false); //造成傷害
        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override bool TryUse()
    {
        string useCard = FightManager.Instance.lastCardType;
        bool canUse = useCard == "10001" || useCard == "10004";
         int cost = totalCost;
        if (!FightManager.Instance.canUseCard)
        {
            UIManager.Instance.showTip("等待其他卡片效果結束", UnityEngine.Color.red);
            return false;
        }
        else if (cost > FightManager.Instance.CurMoveCount)
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("動點不足", UnityEngine.Color.red);
            return false;
        }
        else if (!canUse)
        {
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("上一動不是攻擊", UnityEngine.Color.red);
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
    public override string PointMessage(int damage, Enemy enemy)
    {
        string mess;

        if (FightManager.Instance.thisTurnAttackCount >= 3) //當攻擊3次以上
        {
            int baseCount = 2 * damage - enemy.shield;
            if (baseCount >= 0) mess = $"<color=red>{damage}</color>傷害，2次 [貫穿]";
            else mess = $"<color=red>{damage}</color>傷害，2次";
        }
        else
        {
            int baseCount = damage - enemy.shield;
            if (baseCount >= 0) mess = $"<color=red>{damage}</color>傷害 [貫穿]";
            else mess = $"<color=red>{damage}</color>傷害";
        }

        return mess;
    }
}
