using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10019_TT_Commander : Enemy
{
    private Transform atk0, atk1, atk2;
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
                ani.Play("atk2");
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
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                GetShield();
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.paralysis, 1, 2); //貫穿 麻痺
                }
                FightManager.Instance.GetPlayerHit(AttackCheck());

                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, 2); //貫穿 暈眩
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
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
                atk2.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"獲得{DefendCheck()}護甲");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                atk2.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "貫穿：給予1回合2麻痺");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                ShowDamageTip(atk2.gameObject, AttackCheck(), "貫穿：給予1回合2暈眩");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
        atk1 = actionObj.transform.Find("paralysisattack");
        atk2 = actionObj.transform.Find("dizzAttack");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        atk2.gameObject.SetActive(false);

    }
}
