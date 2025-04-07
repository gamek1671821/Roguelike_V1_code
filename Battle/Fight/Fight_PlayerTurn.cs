using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.showTip("玩家回合", Color.green, delegate ()
        {
            if (FightManager.Instance.buffsTurn[(int)BuffType.CondenseKnife] > 0) //當有凝聚小刀
            {
                MyFuns.Instance.DrawCard(FightManager.Instance.buffsVal[(int)BuffType.CondenseKnife]);  //額外抽1 
            }

            if (GodManager.Instance.isBattle) UseItem();

            int draw = FightManager.Instance.buffsVal[(int)BuffType.Speed] - FightManager.Instance.deBuffsVal[(int)DeBuffType.paralysis];
            int drawCard = Mathf.Clamp(6 + draw, 0, int.MaxValue);
            MyFuns.Instance.DrawCard(drawCard);
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();
            FightManager.Instance.canUseCard = true;
        });
    }
    public override void OnUpdate()
    {

    }
    private void SS()
    {
        if (FightCardManager.Instance.HasCard() == false)
        {
            FightCardManager.Instance.Init();
            //更新棄牌堆數量
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateNoCardCount();
        }
    }
    private void UseItem()
    {
        if (FightManager.Instance.sneakAttack) //當有 特殊道具給予的 偷襲攻擊
        {
            FightManager.Instance.sneakAttack = false;
            MyFuns.Instance.ShowMessage($"觸發「影之雕像」", MyFuns.MessageType.Item);
            foreach (var enemy in EnemyManager.Instance.enemyList)
            {
                enemy.Hit(5 , true);
            }
            FightManager.Instance.FatalAttackdetermination(); //在全部攻擊完後才計算死亡 (否則敵人數量會改變)
        }
        if (FightManager.Instance.Relics) // 有遺物共鳴器 給予的回復生命
        {
            FightManager.Instance.Relics = false;
            MyFuns.Instance.ShowMessage($"觸發「遺物共鳴器」", MyFuns.MessageType.Item);
            int plusHP = (int)(GameObject.FindGameObjectWithTag("item").GetComponent<ItemManager>().items.Count / 5);

            MyFuns.Instance.RestoreHp(plusHP);
        }
        if (FightManager.Instance.LifeBarrier) // 有生命屏障 (給予的回復生命)
        {
            FightManager.Instance.LifeBarrier = false;
            FightManager.Instance.MaxHp += 10;
            MyFuns.Instance.RestoreHp(10);
        }
        if (FightManager.Instance.FailPotion > 0)
        {
            FightManager.Instance.InterHit_IsDeath(FightManager.Instance.FailPotion, true);
            MyFuns.Instance.ShowMessage($"觸發「混濁藥劑」{FightManager.Instance.FailPotion / 3}次", MyFuns.MessageType.Item);
            FightManager.Instance.FailPotion = 0;
        }
        if (FightManager.Instance.BloodBeastNecklace) // 有獸娘的血染項鍊 給予的回復生命
        {
            FightManager.Instance.BloodBeastNecklace = false;
            FightManager.Instance.MaxHp -= 10;
        }
    }

}
