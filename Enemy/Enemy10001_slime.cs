using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10001_slime : Enemy
{
    private Transform attack0, deffend;
    public override void CustomizedDoAction_anim()
    {
        ani.Play("attack");
    }
    public override void CustomizedDoAction_attack()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 2:
                GetShield(); 
                break;
        }
    }
    public override void SetRendomAction()
    {
        type = Random.Range(1, 3);

        switch (type)
        {
            case 0:
                break;
            case 1:
                attack0.gameObject.SetActive(true);
                deffend.gameObject.SetActive(false);
                ShowDamageTip(attack0.gameObject, AttackCheck());
                break;
            case 2:
                attack0.gameObject.SetActive(false);
                deffend.gameObject.SetActive(true);
                ShowDamageTip(attack0.gameObject, DefendCheck(), "", "blue");

                break;

        }
    }
    public override void setTf()
    {
        attack0 = actionObj.transform.Find("attack");
        deffend = actionObj.transform.Find("defend");
    }
    public override void HideAction()
    {
        attack0.gameObject.SetActive(false);
        deffend.gameObject.SetActive(false);
    }
}

