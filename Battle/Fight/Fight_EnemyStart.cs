using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyStart : FightUnit
{
    public void CountPoisoned()
    {//計算腐蝕狀態
        for (int i = 0; i < EnemyManager.Instance.enemyList.Count; i++)
        {
            Enemy nowEnemy = EnemyManager.Instance.enemyList[i];
            bool isDeath = false;

            if (nowEnemy.deBuffsTurn[(int)DeBuffType.poisoned] > 0 && !isDeath) //有腐蝕狀態
            {
                isDeath = nowEnemy.InterHit_UnDeath(nowEnemy.deBuffsVal[(int)DeBuffType.poisoned]); //計算腐蝕傷害  不判斷是否死亡
                if (FightManager.Instance.buffsTurn[(int)BuffType.crazyPosion] > 0) //玩家擁有猛毒生命體
                    MyFuns.Instance.RestoreHp(1 + (int)(nowEnemy.deBuffsVal[(int)DeBuffType.poisoned] / 10f));
            }
            nowEnemy.De_BuffDown(true); //回合開始時，全部buff下降
        }
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
    }

    public override void Init()
    {
        if (FightManager.Instance.skipEnemyTurn)//如果有跳過敵人回合
        {
            UIManager.Instance.showTip("跳過敵人回合", Color.yellow, delegate ()
            {
                CountPoisoned();

                FightManager.Instance.FatalAttackdetermination(); //判斷死亡

                if (EnemyManager.Instance.enemyList.Count <= 0)
                    FightManager.Instance.ChangeType(FightType.Win);
                else
                {
                    FightManager.Instance.ChangeType(FightType.Enemy_End); //直接到敵人回合結束
                    FightManager.Instance.skipEnemyTurn = false;
                }
            });
        }
        else
        {
            UIManager.Instance.showTip("準備敵人回合", Color.yellow, delegate ()
            {
                CountPoisoned();

                FightManager.Instance.FatalAttackdetermination(); //判斷死亡

                if (EnemyManager.Instance.enemyList.Count <= 0)
                    FightManager.Instance.ChangeType(FightType.Win);
                else
                    FightManager.Instance.ChangeType(FightType.Enemy); //切換到敵人回合

            });
        }
    }
}
