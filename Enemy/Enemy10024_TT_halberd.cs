using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10024_TT_halberd : Enemy
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
                FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 1.5f), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    GetShield();
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                GetBuff(BuffType.power, 99, 2);
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
                ShowDamageTip(atk0.gameObject, (int)(AttackCheck() * 1.5f));
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), $"貫穿：獲得{DefendCheck()}護甲");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 2, "獲得2力量", "green");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
        atk1 = actionObj.transform.Find("breakAttack");
        buff = actionObj.transform.Find("Buff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
    }
}
