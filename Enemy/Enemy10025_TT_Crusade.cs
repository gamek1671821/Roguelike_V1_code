using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10025_TT_Crusade : Enemy
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
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                GetShield();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int total = (int)((shield + AttackCheck()) * 0.5f); //對護甲傷害減半
                int preHit2 = Mathf.Clamp(EnemyCheckHit(total), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, 2); //貫穿 暈眩
                    FightManager.Instance.shieldCount = 0;
                    FightManager.Instance.GetPlayerHit(preHit2 * 2, this); //貫穿後傷害回歸
                }
                else
                {
                    FightManager.Instance.GetPlayerHit(total, this);
                }
                NextIsRegulate(3);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                GetShield();
                GetBuff(BuffType.power, 99, 1);
                GetBuff(BuffType.hard, 99, 1);
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"獲得{DefendCheck()}護甲");
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck() + shield, $"根據護甲增傷。貫穿：1回2暈眩");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, DefendCheck(), "獲得1力1硬", "blue");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("shield attack");
        atk1 = actionObj.transform.Find("dizzAttack");
        buff = actionObj.transform.Find("shield buff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
    }
}
