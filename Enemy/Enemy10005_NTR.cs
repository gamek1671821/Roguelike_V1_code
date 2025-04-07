using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10005_NTR : Enemy
{
    private Transform deffend, attack, attack1, BuffTf;
    public override void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("Block");
                break;
            case 2:
                ani.Play("attack");
                break;
            case 3:
                ani.Play("attack 1");
                break;
            case 4:
                ani.Play("Buff");
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
                GetShield();
                break;
            case 2:
                //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                int preHit = Mathf.Clamp(EnemyCheckHit((int)(AttackCheck() * 2)), 0, int.MaxValue);
                if (preHit == 0) FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 2), this);//沒有貫穿 傷害兩倍
                else FightManager.Instance.GetPlayerHit(FightManager.Instance.shieldCount + (int)(preHit * 0.5f), this);  //貫穿後剩餘傷害 回調
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
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
                deffend.gameObject.SetActive(true);
                attack.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                BuffTf.gameObject.SetActive(false);
                ShowDamageTip(deffend.gameObject, DefendCheck(), "", "blue");
                break;
            case 2:
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(true);
                attack1.gameObject.SetActive(false);
                BuffTf.gameObject.SetActive(false);
                ShowDamageTip(attack.gameObject, AttackCheck());
                break;
            case 3:
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                BuffTf.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "對護甲兩倍傷害");
                break;
            case 4:
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                BuffTf.gameObject.SetActive(true);
                ShowDamageTip(BuffTf.gameObject, 2);
                break;

        }
    }
    public override void setTf()
    {
        deffend = actionObj.transform.Find("defend");
        attack = actionObj.transform.Find("attack");
        attack1 = actionObj.transform.Find("breakAttack");
        BuffTf = actionObj.transform.Find("Buff");
    }
    public override void HideAction()
    {

        deffend.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        attack1.gameObject.SetActive(false);
        BuffTf.gameObject.SetActive(false);
    }

}