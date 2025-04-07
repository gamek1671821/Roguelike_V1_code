using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10010_Catcher_B : Enemy
{
    private Transform attack0, attack1, attack2, attack3, defend0, buff;
    private bool canAttack6 = true;

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
            case 4:
                ani.Play("atk3");
                break;
            case 5:
                ani.Play("DEF0");
                break;
            case 6:
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
            case 1://造成傷害，貫穿造成中毒3中毒
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                { //貫穿造成中毒3中毒
                    FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 2, 2);
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //這裡可以撥放動畫
                break;
            case 2: //造成傷害，造成2燒傷
                FightManager.Instance.GetDeBuff(DeBuffType.burn, 2, 2);
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3: //造成傷害，根據劇毒獲得燒傷
                int poisoned = FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned];
                FightManager.Instance.GetDeBuff(DeBuffType.burn, 2, poisoned);
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                break;
            case 4:
                int burn = FightManager.Instance.deBuffsVal[(int)DeBuffType.burn];
                FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 2, burn);
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                break;
            case 5:
                GetShield();
                break;
            case 6:
                GetBuff(BuffType.power, 99, 2);
                GetBuff(BuffType.hard, 99, 2);
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
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack0.gameObject, AttackCheck(), "貫穿造成2回2毒");
                break;
            case 2:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "造成2回2燒傷");
                break;
            case 3:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack2.gameObject, AttackCheck(), "根據劇毒獲得2回燒傷");
                break;
            case 4:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(true);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack3.gameObject, AttackCheck(), "根據燒傷獲得2回劇毒");
                break;
            case 5:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(defend0.gameObject, DefendCheck(), "", "blue");
                break;
            case 6:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 2, "獲得2力量、2硬甲");
                break;
        }
    }
    public override void setTf()
    {

        attack0 = actionObj.transform.Find("attack_0");
        attack1 = actionObj.transform.Find("attack_1");
        attack2 = actionObj.transform.Find("attack_2");
        attack3 = actionObj.transform.Find("attack_3");

        defend0 = actionObj.transform.Find("defend0");
        buff = actionObj.transform.Find("buff");
    }
    public override void HideAction()
    {
        attack0.gameObject.SetActive(false);
        attack1.gameObject.SetActive(false);
        attack2.gameObject.SetActive(false);
        attack3.gameObject.SetActive(false);

        defend0.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);

    }

}
