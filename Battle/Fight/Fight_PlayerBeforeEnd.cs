using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerBeforeEnd : FightUnit
{
    public System.Action degOnPlayerTurnEnd;
    public Fight_PlayerBeforeEnd(System.Action onPlayerTurnEnd) //建構子(委派)
    {
        degOnPlayerTurnEnd = onPlayerTurnEnd;
    }
    public override void Init()
    {
        UIManager.Instance.showTip("結算回合", Color.green, delegate ()
        {
            int handCardCount = FightUI.Instance.handCardItemList.Count;
            for (int i = 0; i < handCardCount; i++)
            {
                if (FightUI.Instance.handCardItemList[i].data["EndTurnEffect"] == "T") //如果有回合結束效果
                {
                    FightUI.Instance.handCardItemList[i].BeforeEnd();
                }
            }
            FightManager.Instance.ExecuteTasks();
            FightManager.Instance.TurnEndEffect();
            //切換到敵人回合開始前
            FightManager.Instance.ChangeType(FightType.Player_End);
        });
    }
}