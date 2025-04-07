using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Enemy10004_Muscomorph : Enemy
{
    private Transform deffend, attack0, poisoned_Attack;
    public override void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("defend");
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
                //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                //確認是否直擊
                if (HitIsBreck((int)(AttackCheck() * 0.75f)))
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 3, 2); //賦予 3回2毒
                }
                //玩家扣血
                FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 0.75f), this);

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
                deffend.gameObject.SetActive(true);
                attack0.gameObject.SetActive(false);
                poisoned_Attack.gameObject.SetActive(false);
                ShowDamageTip(deffend.gameObject, DefendCheck(), "", "blue");
                break;
            case 2:
                deffend.gameObject.SetActive(false);
                attack0.gameObject.SetActive(true);
                poisoned_Attack.gameObject.SetActive(false);
                ShowDamageTip(attack0.gameObject, AttackCheck());
                break;
            case 3:
                deffend.gameObject.SetActive(false);
                attack0.gameObject.SetActive(false);
                poisoned_Attack.gameObject.SetActive(true);
                ShowDamageTip(poisoned_Attack.gameObject, (int)(AttackCheck() * 0.75f), "直擊中毒");
                break;

        }
    }

    public override void OnSelect() //被選中
    {
        _meshRenderer.material.SetFloat("_Outline_Width", 3);
        //_meshRenderer.material.SetColor("_Outline_Color", Color.red);
        _meshRenderer.material.SetColor("_OtlColor", Color.red); ;
    }

    public override void UnOnSelect() //未被選中
    {
        _meshRenderer.material.SetFloat("_Outline_Width", 0);
        //_meshRenderer.material.SetColor("_Outline_Color", Color.black);
        _meshRenderer.material.SetColor("_OtlColor", Color.black); ;
    }
    public override void setTf()
    {
        deffend = actionObj.transform.Find("defend");
        attack0 = actionObj.transform.Find("attack");
        poisoned_Attack = actionObj.transform.Find("poisoned attack");
    }
    public override void HideAction()
    {
        attack0.gameObject.SetActive(false);
        deffend.gameObject.SetActive(false);
        poisoned_Attack.gameObject.SetActive(false);
    }
}
