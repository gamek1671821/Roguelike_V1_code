using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerStart : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.showTip("準備玩家回合", Color.green, delegate ()
        {
            MyFuns.Instance.ShowMessage($"<color=#7EFF4A>*切換回合</color>", MyFuns.MessageType.Item);

            bool isDeath = false;
            if (FightManager.Instance.deBuffsTurn[(int)DeBuffType.poisoned] > 0 && !isDeath) //有中毒狀態
            {
                isDeath = FightManager.Instance.InterHit_IsDeath(FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned]); //計算中毒傷害 同時判斷是否死亡
            }

            //恢復行動力 
            FightManager.Instance.CurMoveCount = FightManager.Instance.MaxMoveCount; //現有能量改為最大能量
            FightManager.Instance.CurMoveCount += FightManager.Instance.buffsVal[(int)BuffType.Speed] - FightManager.Instance.deBuffsVal[(int)DeBuffType.dizz];

            if (FightManager.Instance.buffsTurn[(int)BuffType.ForeverShield] <= 0) //當沒有不滅護甲
                FightManager.Instance.shieldCount = 0; //護甲

            if (MyFuns.Instance.HaveItem(ItemData.IronMaidenArmor) && !isDeath) //道具，鐵處女盔甲
            {
                MyFuns.Instance.ShowMessage($"觸發「鐵處女盔甲」", MyFuns.MessageType.Item);
                isDeath = FightManager.Instance.InterHit_IsDeath(1, true); //受到內傷 無視護甲。
                FightManager.Instance.shieldCount += 4;
            }


            if (FightManager.Instance.buffsTurn[(int)BuffType.crazyIntellect] > 0 && !isDeath) //當有瘋狂知識
            {
                int buffsVal = FightManager.Instance.buffsVal[(int)BuffType.crazyIntellect];
                FightManager.Instance.GetBuff(BuffType.intellect, 999, buffsVal);
                isDeath = FightManager.Instance.InterHit_IsDeath(2 * buffsVal, true);
            }
            if (FightManager.Instance.buffsTurn[(int)BuffType.crazyPower] > 0 && !isDeath) //當有惡魔契約
            {
                int buffsVal = FightManager.Instance.buffsVal[(int)BuffType.crazyPower];
                FightManager.Instance.GetBuff(BuffType.power, 999, buffsVal);
                isDeath = FightManager.Instance.InterHit_IsDeath(2 * buffsVal, true);
            }
            if (FightManager.Instance.buffsTurn[(int)BuffType.CondenseKnife] > 0 && !isDeath) //當有凝聚小刀
            {
                int buffsVal = FightManager.Instance.buffsVal[(int)BuffType.CondenseKnife];
                MyFuns.Instance.GetKnife(buffsVal); //放X張刀刃到牌頂
            }
            if (FightManager.Instance.buffsTurn[(int)BuffType.WindSpell] > 0 && !isDeath) //當有風行咒
            {
                int buffsVal = FightManager.Instance.buffsVal[(int)BuffType.WindSpell];
                FightManager.Instance.GetBuff(BuffType.light, buffsVal, 1);
            }




            if (isDeath) FightManager.Instance.ChangeType(FightType.Loss);
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadtePower();
            UIManager.Instance.GetUI<FightUI>("FightUI").UpadteDefense();
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();

            FightManager.Instance.De_BuffDown(true); // 開始時的 持續時間-1
            FightManager.Instance.TurnEndResetCount(); //重設卡牌計數器
            FightManager.Instance.ChangeType(FightType.Player);//切換到玩家回合
        });
    }
}
