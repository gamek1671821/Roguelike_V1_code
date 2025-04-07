using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyTurn :FightUnit
{
     public override void Init()
    {
        UIManager.Instance.showTip("敵人回合" , Color.yellow ,delegate()
        {
            FightManager.Instance.StartCoroutine(EnemyManager.Instance.DoAllEnemyAction());
        });

    }
    public override void OnUpdate()
    {
        
    }
}

