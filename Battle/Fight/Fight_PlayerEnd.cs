using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerEnd : FightUnit
{

    //public System.Action degOnPlayerTurnEnd;
    //   public Fight_PlayerEnd(System.Action onPlayerTurnEnd) //建構子(委派)
    // {
    //     degOnPlayerTurnEnd = onPlayerTurnEnd;
    // }
    public override void Init()
    {
        UIManager.Instance.showTip("玩家回合結束", Color.green, delegate ()
        {
            //刪除所有卡排

            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveAllCards();
            bool isDeath = false;
            if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.deIntellect] > 0 && !isDeath) //有智壞狀態
            {
                FightManager.Instance.GetBuff(BuffType.intellect, 0, -FightManager.Instance.deBuffsVal[(int)DeBuffType.deIntellect]);
            }
            if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.deHard] > 0 && !isDeath) //有崩甲狀態
            {
                FightManager.Instance.GetBuff(BuffType.hard, 0, -FightManager.Instance.deBuffsVal[(int)DeBuffType.deHard]);
            }
            if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.dePower] > 0 && !isDeath) //有脫力狀態
            {
                FightManager.Instance.GetBuff(BuffType.power, 0, -FightManager.Instance.deBuffsVal[(int)DeBuffType.dePower]);
            }

            if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.burn] > 0 && !isDeath) //有燃燒狀態
            {
                isDeath = FightManager.Instance.InterHit_IsDeath(FightManager.Instance.deBuffsVal[(int)DeBuffType.burn]); //計算燃燒傷害 同時判斷是否死亡
            }


            if (isDeath) FightManager.Instance.ChangeType(FightType.Loss);

            FightManager.Instance.De_BuffDown(false); // 不是開始時的 持續時間-1
            //切換到敵人回合開始前
            FightManager.Instance.ChangeType(FightType.Enemy_Start);
        });
    }
}
