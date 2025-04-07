using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card33_CardAttack : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //隨機摧毀3張手牌。對隨機造成3次{0}物理傷害。
            EffAndAudio();
            //使用效果
            int val = CountAttack("Arg0"); //傷害
            //隨機攻擊
            for (int i = 0; i < 3; i++) //攻擊次數
            {
                int remove = Random.Range(0, FightUI.Instance.handCardItemList.Count);
                Destroy(FightUI.Instance.handCardItemList[remove].gameObject); //物件摧毀
                FightUI.Instance.handCardItemList.RemoveRange(remove, 1); //卡牌移除1張 
                FightManager.Instance.thisTurnDestroyCount++; //移除計數器加一
                if (EnemyManager.Instance.enemyList.Count <= 0) break;
                Enemy enemy = EnemyManager.Instance.enemyList[Random.Range(0, EnemyManager.Instance.enemyList.Count)]; //隨機抽一個敵人
                enemy.Hit(val, true);
                FatalAttackdetermination(); //每次攻擊後 確認是否致死
            }
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//更新卡牌位置
                                                                             //刷新數值
            CardEffectEnd();//卡片效果結束

            Vector3 pos = Camera.main.transform.position;
            pos.y = 0;
            PlayEffect(pos);

        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變
        msgText.text = string.Format(data["Des"], CRedT(CountAttack("Arg0"))); // 字串
    }
    public override bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
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
        else if (FightUI.Instance.handCardItemList.Count < 4) //包含這張卡與三張卡 以上才能發動
        {
            AudioManager.Instance.PlayEffect("Effect/lose"); //使用失敗音效
            UIManager.Instance.showTip("手牌數量不足", UnityEngine.Color.red);
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
