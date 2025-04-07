using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10006_DK : Enemy
{
    private Transform attack0, attack1, attack2;
    private bool canAttack6 = true;

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
                ani.Play("attack1");
                break;
            case 3:
                if (canAttack6)
                    ani.Play("attack2");
                else
                    ani.Play("gethit3");
                break;
        }
    }
    public override void CustomizedDoAction_attack()
    {
        switch (type)
        {
            case 0:
                break;
            case 1://造成傷害，並提高1力量，消耗 10生命
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                GetBuff(BuffType.power, 999, 1);
                curHp = Mathf.Clamp(curHp - 10, 1, MaxHp);//
                updateHp();
                //這裡可以撥放動畫
                break;
            case 2: //造成傷害，並提高1力量，傷害對護甲減半，回復貫穿傷害
                int preHit = Mathf.Clamp(EnemyCheckHit((int)(AttackCheck() * 0.5f)), 0, int.MaxValue);
                if (preHit == 0) FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 0.5f), this);//沒有貫穿 傷害減半
                else
                {
                    FightManager.Instance.GetPlayerHit(FightManager.Instance.shieldCount + (int)(preHit * 2f), this);  //貫穿後剩餘傷害 回調
                    curHp = Mathf.Clamp(curHp + (int)(preHit * 2f), 1, MaxHp);//回復貫穿傷害
                }
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                if (canAttack6)
                //造成3倍傷害 ，並提高3力量，消耗 30生命，下次使用失敗
                {
                    FightManager.Instance.GetPlayerHit(AttackCheck() * 3, this);
                    GetBuff(BuffType.power, 999, 3);
                    curHp = Mathf.Clamp(curHp - 30, 1, MaxHp);//
                    updateHp();

                    //攝影機晃動
                    Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                    canAttack6 = false;

                    NextIsRegulate(3); // 
                }
                else
                {
                    canAttack6 = true;
                }
                break;
            case 4:
                GetBuff(BuffType.power, 999, 2);
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
                ShowDamageTip(attack0.gameObject, AttackCheck(), "+1力 -10生命");
                break;
            case 2:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "+1力 -10生命 護甲減傷50% 回復貫穿傷害");
                break;
            case 3:
                attack0.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                if (canAttack6)
                    ShowDamageTip(attack2.gameObject, 3 * AttackCheck(), "+3力 -30生命");
                else
                    ShowDamageTip(attack2.gameObject, 0);
                break;
        }
    }
    public override void setTf()
    {
        attack0 = actionObj.transform.Find("attack");
        attack1 = actionObj.transform.Find("attack_2");
        attack2 = actionObj.transform.Find("attack_3");
    }
    public override void HideAction()
    {
        attack0.gameObject.SetActive(false);
        attack1.gameObject.SetActive(false);
        attack2.gameObject.SetActive(false);
    }

}
