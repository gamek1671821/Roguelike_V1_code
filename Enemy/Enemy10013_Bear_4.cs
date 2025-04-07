using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10013_Bear_4 : Enemy
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
            case 1://造成傷害，貫穿造成1回合"1破滅"
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.disappoint, 2, 1);
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 2: //造成傷害，對護甲造成雙倍傷害
                int preHit1 = Mathf.Clamp(EnemyCheckHit(AttackCheck()) * 2, 0, int.MaxValue);
                if (preHit1 > 0) //貫穿
                {
                    FightManager.Instance.shieldCount = 0;
                    FightManager.Instance.GetPlayerHit((int)(preHit1 / 2), this); //貫穿後傷害回歸
                }
                else
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck() * 2, this); //
                }

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3: //造成傷害，貫穿回復
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 == 0) FightManager.Instance.GetPlayerHit(AttackCheck(), this);//沒有貫穿
                else
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck(), this);//
                    curHp = Mathf.Clamp(curHp + preHit2, 1, MaxHp);//回復貫穿傷害
                }
                updateHp();
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 4: //造成傷害，貫穿造成1回合2暈眩
                int preHit3 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit3 == 0) FightManager.Instance.GetPlayerHit(AttackCheck(), this);//沒有貫穿
                else
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck(), this);//
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, 2);
                }
                
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 5:
                GetBuff(BuffType.power, 99, 3);
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
                ShowDamageTip(attack0.gameObject, AttackCheck(), "貫穿造成1回合");
                break;
            case 2:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "對護甲造成雙倍傷害");
                break;
            case 3:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                attack3.gameObject.SetActive(false);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack2.gameObject, AttackCheck(), "回復貫穿傷害");
                break;
            case 4:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(true);

                defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(attack3.gameObject, AttackCheck(), "貫穿造成1回合2暈眩");
                break;
            case 5:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                attack3.gameObject.SetActive(false);

                //defend0.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 2, "獲得3力量", "green");
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
