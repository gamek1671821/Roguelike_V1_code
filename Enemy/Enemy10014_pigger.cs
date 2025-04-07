using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10014_pigger : Enemy
{
    private Transform buff, attack1, attack2, health;
    public override void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("buff");
                //這裡可以撥放動畫
                break;
            case 2:
                ani.Play("attack");
                break;
            case 3:
                ani.Play("deffend");
                break;
            case 4:
                ani.Play("attack 1");
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
                GetBuff(BuffType.power, 99, 2); //獲得 1力量
                curHp += 20; //(int)((MaxHp - curHp) * 0.2f); //回復 20%已損失生命
                updateAllStatus();

                break;
            case 2:
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                FightManager.Instance.GetDeBuff(DeBuffType.disappoint, 1, 1); //給予破滅

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                NextIsRegulate(4);
                break;
            case 3:
                GetShield();
                curHp += (int)((MaxHp - curHp) * 0.2f); //回復 20%已損失生命
                FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 0.5f), this); //玩家扣血
                updateAllStatus();
                break;
            case 4:
                FightManager.Instance.GetPlayerHit(AttackCheck() * 2, this);
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
                buff.gameObject.SetActive(true);
                attack1.gameObject.SetActive(false);
                health.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(buff.gameObject, 20, "+2力 回復20生命", "white");
                break;
            case 2:
                buff.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                health.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "給予1回破滅");
                break;
            case 3:
                buff.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                health.gameObject.SetActive(true);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(health.gameObject, (int)(AttackCheck() * 0.5f), $"造成傷害並回復20%已損失生命");
                break;
            case 4:
                buff.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                health.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                ShowDamageTip(attack2.gameObject, AttackCheck() * 2);
                break;

        }
    }
    public override void setTf()
    {
        base.setTf();
        buff = actionObj.transform.Find("Buff");
        attack1 = actionObj.transform.Find("disappoint Attack");
        health = actionObj.transform.Find("health");
        attack2 = actionObj.transform.Find("attack_2");

    }
    public override void HideAction()
    {
        base.HideAction();
        buff.gameObject.SetActive(false);
        attack1.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        attack2.gameObject.SetActive(false);
    }
}
