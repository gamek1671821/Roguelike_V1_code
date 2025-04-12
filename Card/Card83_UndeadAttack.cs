using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card83_UndeadAttack : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {
        damageText.text = $"即死"; //之後可以動態計算傷害
    }
    public override void OnPointDamageText()
    {
        if (hitEnemy.data["Undead"] == "F")
        {
            damageText.text = $"目標不是不死生命"; //之後可以動態計算傷害
        }
        else
        {
            damageText.text = $"即死"; //之後可以動態計算傷害
        }
    }


    public override void CardEffect()
    {
        //指定一敵人。即死。僅能對血肉生命體使用。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)

        hitEnemy.curHp = 0;
        hitEnemy.Hit(0, true); //造成傷害

        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override void BeforeEndEffect()
    {
        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, "B"); //
    }

    public override bool TryUse()
    {
         int cost = totalCost;
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
        else if (hitEnemy.data["Undead"] == "F")
        {
            //費用不足
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("目標不是不死生命", Color.red);
            return false;
        }
        else
        {
            FightManager.Instance.canUseCard = false;
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