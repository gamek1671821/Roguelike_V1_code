using UnityEngine.EventSystems;
public class Card03_RangeAttack : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {   //對全部敵人造成{0}傷害。
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = CountAttack("Arg0");
            //發動效果 {對全部敵人造成傷害}
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.Hit(val , true); //疊加紀錄 貫穿傷害
            }
            FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)

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
