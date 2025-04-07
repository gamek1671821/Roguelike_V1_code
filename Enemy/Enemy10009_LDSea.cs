using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Enemy10009_LDSea : Enemy
{
    public Transform attack_0, attack_1, SAttackHead, SAttackPawL, SAttackPawR, SAttackStump, SAttackTail;

    public override void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("attack");
                //這裡可以撥放動畫
                break;
            case 2:
                ani.Play("attack_1");
                break;
            case 3:
                ani.Play("SAttack Head");
                break;
            case 4:
                ani.Play("SAttack Paw L");
                break;
            case 5:
                ani.Play("SAttack Paw R");
                break;
            case 6:
                ani.Play("SAttack Stump");
                break;
            case 7:
                ani.Play("SAttack Tail");
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
                    Heal(preHit);//回復貫穿傷害
                }
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                //這裡可以撥放動畫
                break;
            case 2://造成傷害，降低所有buff效果
                   //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //全部減少1
                for (int i = 0; i < FightManager.Instance.buffsVal.Count; i++)
                {
                    FightManager.Instance.buffsVal[i] -= 1;
                }
                FightManager.Instance.SetBuffItem();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3://造成傷害， 直擊時賦予2點暈眩 2回合
                if (HitIsBreck(AttackCheck()))
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 2, 2); //賦予 2回2暈
                }

                //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 4://造成傷害，獲得1力量、1硬甲，下次攻擊固定 
                   //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                GetBuff(BuffType.power, 99, 1);
                GetBuff(BuffType.hard, 99, 1);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                NextIsRegulate(5);
                break;
            case 5://造成傷害，獲得30護甲，下次攻擊固定 
                   //玩家扣血
                GetShield();
                updateShield();
                NextIsRegulate(6);
                break;
            case 6://造成2倍傷害
                   //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck() * 2, this);

                if (HitIsBreck(AttackCheck()))
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 3, 1); //賦予 3回1暈
                }

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 7:  //獲得1力量、1硬甲、1刺
                GetBuff(BuffType.power, 99, 1);
                GetBuff(BuffType.hard, 99, 1);
                GetBuff(BuffType.rebound, 99, 1);
                break;

        }
    }
    public override void HideAction()
    {
        attack_0.gameObject.SetActive(false);
        attack_1.gameObject.SetActive(false);
        SAttackHead.gameObject.SetActive(false);
        SAttackPawL.gameObject.SetActive(false);
        SAttackPawR.gameObject.SetActive(false);
        SAttackStump.gameObject.SetActive(false);
        SAttackTail.gameObject.SetActive(false);
    }

    public override void setTf()
    {
        attack_0 = actionObj.transform.Find("attack");
        attack_1 = actionObj.transform.Find("attack_1");
        SAttackHead = actionObj.transform.Find("SAttack Head");
        SAttackPawL = actionObj.transform.Find("SAttack Paw L");
        SAttackPawR = actionObj.transform.Find("SAttack Paw R");
        SAttackStump = actionObj.transform.Find("SAttack Stump");
        SAttackTail = actionObj.transform.Find("SAttack Tail");
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
                attack_0.gameObject.SetActive(true);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(attack_0.gameObject, AttackCheck(), "回復直擊傷害");
                break;
            case 2:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(true);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(attack_1.gameObject, AttackCheck(), "降低玩家所有buff效果");
                break;
            case 3:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(true);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(SAttackHead.gameObject, AttackCheck(), "直擊：賦予2回2暈眩");
                break;
            case 4:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(true);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(SAttackPawL.gameObject, AttackCheck(), "獲得1力量、硬甲，連擊");
                break;
            case 5:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(true);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(SAttackPawR.gameObject, DefendCheck(), "獲得護甲，連擊", "blue");
                break;
            case 6:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(true);
                SAttackTail.gameObject.SetActive(false);

                ShowDamageTip(SAttackStump.gameObject, 2 * AttackCheck(), "給予3回合1暈眩");
                break;
            case 7:
                attack_0.gameObject.SetActive(false);
                attack_1.gameObject.SetActive(false);
                SAttackHead.gameObject.SetActive(false);
                SAttackPawL.gameObject.SetActive(false);
                SAttackPawR.gameObject.SetActive(false);
                SAttackStump.gameObject.SetActive(false);
                SAttackTail.gameObject.SetActive(true);

                ShowDamageTip(SAttackTail.gameObject, 1, "獲得1力量、1硬甲、1刺", "green");
                break;


        }
    }
}
