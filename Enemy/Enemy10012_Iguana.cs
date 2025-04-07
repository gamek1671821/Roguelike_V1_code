using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Enemy10012_Iguana : Enemy
{
    private Transform attack0, defend0, buff;

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
                ani.Play("DEF0");
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
            case 1://造成傷害，並回復傷害
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit == 0) FightManager.Instance.GetPlayerHit(AttackCheck(), this);//沒有貫穿
                else
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck(), this);//
                    curHp = Mathf.Clamp(curHp + preHit, 1, MaxHp);//回復貫穿傷害
                }
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                //這裡可以撥放動畫
                break;

            case 2://獲得護甲 
                GetShield();
                NextIsRegulate(6);
                break;
            case 3://造成2倍傷害
                   //無法被指定
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                GetShield();

                GetBuff(BuffType.Lurk, 2, 1); //賦予 2回1潛伏 
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;

        }
    }

    public override void SetRendomAction()
    {
        type = Random.Range(1, int.Parse(data["Actions"]) + 1);

        switch (type)
        {
            case 0:
                break;
            case 1:
                attack0.gameObject.SetActive(true);
                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack0.gameObject, AttackCheck(), "貫穿回復");
                break;

            case 2:
                attack0.gameObject.SetActive(false);
                defend0.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(defend0.gameObject, DefendCheck(), "", "blue");
                break;
            case 3:
                attack0.gameObject.SetActive(false);
                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 2, "獲得2回潛伏", "green");
                break;
        }
    }
    public override void setTf()
    {
        attack0 = actionObj.transform.Find("attack_0");
        defend0 = actionObj.transform.Find("defend0");
        buff = actionObj.transform.Find("buff");
    }
    public override void HideAction()
    {
        attack0.gameObject.SetActive(false);
        defend0.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);

    }



}
