using UnityEngine.EventSystems;

public class Card06_ShieldAttack : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            //對全部敵人造成<color=#7D7DFF>護甲值</color>的傷害。之後<color=#7D7DFF>失去</color>所有護甲。每1點傷害，5%機率附加1回合暈眩。只受硬甲加成。
            EffAndAudio();
            //使用效果
            int val = CountDefend(FightManager.Instance.shieldCount);
            //發動效果 {對全部敵人造成傷害}
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                if (val >= 10)
                    enemy.GetDeBuff(DeBuffType.dizz, 1, 1);
                enemy.Hit(val , true);
            }
            //護甲歸零
            FightManager.Instance.shieldCount = 0;
            FatalAttackdetermination();

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
