using UnityEngine;
using UnityEngine.EventSystems;
public class Card23_CondenseChop : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //對隨機敵人造成{1}固定傷害，重複{0}次。之後，複製{0}張此卡於棄牌堆。次數與複製受智力加成。
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg1"]); //固定傷害
            //隨機攻擊
            for (int i = 0; i < CountIntellect("Arg0"); i++) //攻擊次數
            {
                if (EnemyManager.Instance.enemyList.Count <= 0) break;
                Enemy enemy = EnemyManager.Instance.enemyList[Random.Range(0, EnemyManager.Instance.enemyList.Count)]; //隨機抽一個敵人
                enemy.Hit(val, true);
                FatalAttackdetermination(); //每次攻擊後 確認是否致死
                FightCardManager.Instance.usedCardList.Add("1023");//每次攻擊 額外將此卡放入棄排堆
            }

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
        msgText.text = string.Format(data["Des"], CRedT(CountIntellect("Arg0")), CountAttack("Arg1"), CRedT(int.Parse(data["Arg1"]))); // 字串
    }
}
