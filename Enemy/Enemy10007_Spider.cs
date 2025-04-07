using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10007_Spider : Enemy
{
    private Transform deffend, attack, poisonedAttack;

    public override void CustomizedDoAction_anim()
    {
        //播放對應動畫
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("Block");
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
                GetShield();
                //這裡可以撥放動畫
                break;
            case 2:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0) FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 1, 3);//有貫穿 給予1回合 5中毒
                                                                                           //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                FightManager.Instance.GetPlayerHit(AttackCheck() + FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned] * 2, this); //給與傷害+角色中毒傷害

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
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(false);
                poisonedAttack.gameObject.SetActive(false);
                ShowDamageTip(deffend.gameObject, DefendCheck(), "", "blue");
                break;
            case 2:
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(false);
                poisonedAttack.gameObject.SetActive(false);
                ShowDamageTip(attack.gameObject, AttackCheck(), "直擊：1回3毒");
                break;
            case 3:
                deffend.gameObject.SetActive(false);
                attack.gameObject.SetActive(false);
                poisonedAttack.gameObject.SetActive(false);
                ShowDamageTip(poisonedAttack.gameObject, AttackCheck() + FightManager.Instance.deBuffsVal[(int)DeBuffType.poisoned] * 2, "中毒增傷");
                break;

        }
    }
    public override void setTf()
    {
        deffend = actionObj.transform.Find("defend");
        attack = actionObj.transform.Find("attack_3");
        poisonedAttack = actionObj.transform.Find("poisoned attack");
    }
    public override void HideAction()
    {
        deffend.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        poisonedAttack.gameObject.SetActive(false);
    }
}
