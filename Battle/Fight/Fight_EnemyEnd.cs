using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyEnd : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.showTip("敵人回合結束", Color.yellow, delegate ()
        {
            //Debug.Log("敵人數量" + EnemyManager.Instance.enemyList.Count);
            for (int i = 0; i < EnemyManager.Instance.enemyList.Count; i++)
            {
                Enemy nowEnemy = EnemyManager.Instance.enemyList[i];
                bool isDeath = false;
                if (nowEnemy.deBuffsTurn[(int)DeBuffType.burn] > 0 && !isDeath) //有燃燒狀態
                {
                    isDeath = nowEnemy.InterHit_UnDeath(nowEnemy.deBuffsVal[(int)DeBuffType.burn]); //計算燃燒傷害 不判斷是否死亡
                }
                nowEnemy.hitBox.enabled = nowEnemy.buffsTurn[(int)BuffType.Lurk] <= 0; //潛伏 

                nowEnemy.De_BuffDown(false); //不是回合開始時，全部buff下降
            }
            FightManager.Instance.FatalAttackdetermination(); //判斷死亡

            if (EnemyManager.Instance.enemyList.Count <= 0)
                FightManager.Instance.ChangeType(FightType.Win);
            else
                //切換到玩家回合
                FightManager.Instance.ChangeType(FightType.Player_Start);
        });
    }
    public override void OnUpdate()
    {

    }
}
