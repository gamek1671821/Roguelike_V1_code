using UnityEngine;
using UnityEngine.EventSystems;
public class Card22_BearSlap : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //對全部敵人。造成{0}傷害。傷害對護甲加倍。
            EffAndAudio();
            //使用效果
            int val = CountAttack("Arg0");
            //發動效果 {對全部敵人造成傷害}
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                int preHit = Mathf.Clamp(enemy.CheckHit((int)(val * 2)), 0, int.MaxValue); //預先計算會貫穿多少傷害 打在護甲上 加倍
                if (preHit == 0) enemy.Hit(val * 2 , true); //沒有貫穿 傷害兩倍
                else enemy.Hit(enemy.shield + (int)(preHit * 0.5f) , true);  //貫穿後剩餘傷害 回調
            }
            FatalAttackdetermination();

            CardEffectEnd();//卡片效果結束
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
}
