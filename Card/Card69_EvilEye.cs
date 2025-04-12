using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card69_EvilEye : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
         damageText.text = $"獲得等同目標基礎攻擊力的護甲";
    }
    public override void OnPointDamageText()
    {
       damageText.text = $"獲得 {CountDisappoint(hitEnemy.Attack)} 護甲"; //
    }
    public override void CardEffect()
    {
        //指定一敵人。造成{0}傷害。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        FightManager.Instance.shieldCount += CountDisappoint(hitEnemy.Attack); //獲得等同基礎攻擊的護甲

        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override bool TryUse()
    {
        string useCard = FightManager.Instance.lastCardType;
        bool canUse = useCard == "10007" || useCard == "10008"; // 法術後
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
            UIManager.Instance.showTip("上一動不是法術", UnityEngine.Color.red);
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
}
