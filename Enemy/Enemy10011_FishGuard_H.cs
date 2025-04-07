using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10011_FishGuard_H : Enemy
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
            case 2:
                ani.Play("atk0");
                break;
            case 3:
            case 4:
                ani.Play("atk1");
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
            case 1://造成傷害，貫穿造成凍潮
            case 2:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                { //貫穿造成中毒3中毒
                    FightManager.Instance.GetDeBuff(DeBuffType.paralysis, 5, 1);
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                break;
            case 3: //造成傷害，根據劇毒獲得燒傷
            case 4: //造成傷害，根據凍潮造成額外傷害
                FightManager.Instance.GetDeBuff(DeBuffType.burn, 99, 2);
                FightManager.Instance.GetPlayerHit(AttackCheck() * (int)((1 + 0.5f) * FightManager.Instance.deBuffsVal[(int)DeBuffType.paralysis]), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
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
            case 2:
                attack0.gameObject.SetActive(true);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack0.gameObject, AttackCheck(), "貫穿造成5回合1麻痺");
                break;
            case 3:
            case 4:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack2.gameObject, AttackCheck() * (int)((1 + 0.5f) * FightManager.Instance.deBuffsVal[(int)DeBuffType.paralysis]), "根據麻痺造成額外傷害");
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
                ShowDamageTip(buff.gameObject, 2, "獲得2力量、2硬甲", "green");
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
