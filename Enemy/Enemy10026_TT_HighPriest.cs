using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10026_TT_HighPriest : Enemy
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
                    Heal((int)((MaxHp - curHp) * preHit * 0.02f)); //每貫穿傷害 回復2%已損生命
                }

                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int poisoned = buffsVal[(int)BuffType.intellect];
                FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 3, 2 + poisoned);
                FightManager.Instance.GetDeBuff(DeBuffType.burn, 3, 2 + poisoned);

                NextIsRegulate(3);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                GetShield();
                GetBuff(BuffType.intellect, 99, 2);
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"每貫穿1：回復2%已損生命");
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, buffsVal[(int)BuffType.intellect] + 2, $"給予3回合{buffsVal[(int)BuffType.intellect] + 2}中毒、燃燒", "white");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, DefendCheck(), "獲得2智", "blue");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("plusHP attack");
        atk1 = actionObj.transform.Find("curse");
        buff = actionObj.transform.Find("shield buff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
    }
}
