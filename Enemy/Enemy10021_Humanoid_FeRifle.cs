using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10021_Humanoid_FeRifle : Enemy
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
                FightManager.Instance.GetDeBuff(DeBuffType.target, 99, 2); // 給予針對 2
                //攝影機晃動
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"給予2針對");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
    }
}
