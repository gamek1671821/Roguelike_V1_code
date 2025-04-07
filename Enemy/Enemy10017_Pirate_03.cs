using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10017_Pirate_03 : Enemy
{
    private Transform atk0, atk1, buff;
    private int CardCount = 0;
    public override void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("atk0");
                //這裡可以撥放動畫
                break;
            case 2:
                ani.Play("atk1");
                break;
            case 3:
                ani.Play("buff");
                break;
        }
    }
    public override void CustomizedDoAction_attack()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 1, 5); //貫穿 中毒
                    MyFuns.Instance.GetGold(-10);
                }
                FightManager.Instance.GetPlayerHit(AttackCheck());
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    if (FightCardManager.Instance.cardList.Count > 0)
                    {
                        int randomIndex = Random.Range(0, FightCardManager.Instance.cardList.Count);
                        FightCardManager.Instance.cardList.RemoveAt(randomIndex); //隨機移除一張卡
                        CardCount++;
                    }
                }
                if (CardCount >= 3)
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck() * 3, this);
                }
                else
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                }

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 3:
                GetBuff(BuffType.power, CardCount, Attack);
                NextIsRegulate(1);
                break;
        }
    }
    public override void SetRendomAction()
    {
        if (nextIsRegulate)//下一個有固定
        {
            type = nextIsStep;
            nextIsRegulate = false; //解除固定
        }
        else
        {
            do //隨機
            {
                type = Random.Range(1, int.Parse(data["Actions"]) + 1);
            } while (ignoreStep.Contains(type));
        }


        switch (type)
        {
            case 0:
                break;
            case 1:

                atk0.gameObject.SetActive(true);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), "貫穿：給予1回5毒，並偷走10金幣");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                if (CardCount >= 3)
                {
                    ShowDamageTip(atk1.gameObject, AttackCheck(), "貫穿：偷走牌堆中的1張卡。總偷取超過3，傷害3倍");
                }
                else
                {
                    ShowDamageTip(atk1.gameObject, AttackCheck() * 3, "貫穿：偷走牌堆中的1張卡。總偷取3以上，傷害3倍");
                }
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(buff.gameObject, CardCount + 1, "獲得偷走的牌數+1的力量" ,"green");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("poisoned attack");
        atk1 = actionObj.transform.Find("steal");
        buff = actionObj.transform.Find("Buff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);

    }
}
