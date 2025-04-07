using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10020_TT_crossbow : Enemy
{
    private Transform atk0, atk1;
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
        }
    }
    public override void CustomizedDoAction_attack()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                FightManager.Instance.GetPlayerHit(AttackCheck()); //不被反彈
                FightManager.Instance.GetDeBuff(DeBuffType.target, 99, 2); // 給予針對 2
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                NextIsRegulate(2);
                break;
            case 2:
                FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 0.5f)); //一半傷害 //不被反彈
                FightManager.Instance.GetDeBuff(DeBuffType.target, 99, 2); // 給予針對 2   
                int burn = FightManager.Instance.deBuffsVal[(int)DeBuffType.target];
                FightManager.Instance.GetDeBuff(DeBuffType.burn, 1, burn); // 給予燃燒 針對值的燃燒   

                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"給予2針對");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                ShowDamageTip(atk1.gameObject, (int)(AttackCheck() * 0.5f), "給予2針對與1回合目標針對值的燃燒");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
        atk1 = actionObj.transform.Find("breakShoot");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
    }
}
