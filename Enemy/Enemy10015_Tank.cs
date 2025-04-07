using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10015_Tank : Enemy
{
    private Transform atk0, atk1, DEF0;
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
                ani.Play("DEF0");
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
                int preHit = Mathf.Clamp(EnemyCheckHit((int)(AttackCheck() * 2)), 0, int.MaxValue);
                if (preHit == 0) FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 2), this);//沒有貫穿 傷害加倍
                else
                {
                    FightManager.Instance.GetPlayerHit(FightManager.Instance.shieldCount + (int)(preHit * 0.5f), this);  //貫穿後剩餘傷害 回調
                    FightManager.Instance.GetDeBuff(DeBuffType.burn, 1, (int)(preHit * 0.5f)); //給予 貫穿傷害的燃燒
                }
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                FightManager.Instance.GetDeBuff(DeBuffType.disappoint, 1, 1); //給予破滅

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 3:
                GetShield();
                updateAllStatus();
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

                atk0.gameObject.SetActive(true);
                atk1.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), "傷害對護甲加倍，造成1回合貫穿的燃燒");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                DEF0.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "給予1回合破滅");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(true);
                ShowDamageTip(DEF0.gameObject, DefendCheck(), "", "blue");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("breakShoot");
        atk1 = actionObj.transform.Find("Shotgun");
        DEF0 = actionObj.transform.Find("defend");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        DEF0.gameObject.SetActive(false);
    }
}
