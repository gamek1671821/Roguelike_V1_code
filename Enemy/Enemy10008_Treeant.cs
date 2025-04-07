using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10008_Treeant : Enemy
{
    private Transform buff, attack1, attack2;
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
                ani.Play("attack_1");
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
                GetBuff(BuffType.power, 999, 1); //獲得 1力量
                GetBuff(BuffType.hard, 999, 1); //獲的 1硬甲
                curHp += (int)((MaxHp - curHp) * 0.2f); //(int)((MaxHp - curHp) * 0.2f); //回復 20%已損失生命
                updateAllStatus();
                break;
            case 2:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0) curHp += 3 * preHit; //　有貫穿　回復貫穿傷害

                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                GetShield();
                FightManager.Instance.GetPlayerHit(AttackCheck(), this); //玩家扣血
                                                                         
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
                buff.gameObject.SetActive(true);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(buff.gameObject, 1, "+1力 +1硬 回復20%已損生命", "green");
                break;
            case 2:
                buff.gameObject.SetActive(false);
                attack1.gameObject.SetActive(true);
                attack2.gameObject.SetActive(false);
                ShowDamageTip(attack1.gameObject, AttackCheck(), "回復貫穿傷害");
                break;
            case 3:
                buff.gameObject.SetActive(false);
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(true);
                ShowDamageTip(attack2.gameObject, AttackCheck(), $"回復{DefendCheck()}護盾");
                break;

        }
    }
    public override void setTf()
    {
        buff = actionObj.transform.Find("Buff");
        attack1 = actionObj.transform.Find("plusHP attack");
        attack2 = actionObj.transform.Find("block attack");

    }
    public override void HideAction()
    {
        buff.gameObject.SetActive(false);
        attack1.gameObject.SetActive(false);
        attack2.gameObject.SetActive(false);
    }
}
