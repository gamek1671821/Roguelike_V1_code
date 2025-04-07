using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class Card67_WarCard : CardItem
{
    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData)
    // 從牌組抽出3張牌。攻擊卡3：獲得2動點。防禦卡3：獲得1力量。技能卡2：(結束時持有：獲得下回合抽牌2)。
    {
        if (!UIManager.Instance.GetUI<FightUI>("FightUI").isMouseInHandZone() && TryUse())
        {
            EffAndAudio();
            //使用效果
            int val = int.Parse(data["Arg0"]); //抽卡數量
            MyFuns.Instance.DrawCard(val);
            if (FightManager.Instance.thisTurnAttackCount >= 3) //攻擊卡3
            {
                FightManager.Instance.CurMoveCount += 2;
            }
            if (FightManager.Instance.thisTurnDefendCount >= 3) //防禦卡3
            {
                int powerval = 1;
                if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
                {
                    MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                    powerval += 1;
                }
                FightManager.Instance.GetBuff(BuffType.power, 99, powerval);
            }

            CardEffectEnd();//卡片效果結束
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    public override void BeforeEndEffect()
    {
        if (FightManager.Instance.thisTurnSkillkCount >= 2) //技能卡2
        {
            FightManager.Instance.GetBuff(BuffType.Draw, 1, 1);
        }
    }
    public override void DragMsgChange()
    {//參數0會改變

        msgText.text = $"從牌組抽出3張牌。";

        if (FightManager.Instance.thisTurnAttackCount >= 3)  //攻擊卡3
        {
            msgText.text += $"{CRedT("攻擊卡3：獲得2動點。")}";
        }
        else
        {
            msgText.text += $"{CGrayT("攻擊卡3：獲得2動點。")}";
        }

        if (FightManager.Instance.thisTurnDefendCount >= 3) //防禦卡3
        {
            msgText.text += $"{CRedT("防禦卡3：獲得1力量。")}";
        }
        else
        {
            msgText.text += $"{CGrayT("防禦卡3：獲得1力量。")}";
        }

        if (FightManager.Instance.thisTurnSkillkCount >= 2) //技能卡2
        {
            msgText.text += $"{CRedT("技能卡2：(結束時持有：獲得下回合抽牌1)。")}";
        }
        else
        {
            msgText.text += $"{CGrayT("技能卡2：(結束時持有：獲得下回合抽牌1)。")}";
        }
    }
}
