using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10022_ClazyFemale : Enemy
{
    private Transform atk0, atk1, atk2, buff, def;
    private int CardCount = 0;
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
                ani.Play("DEF0");
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
            case 1:
                int preHit = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.burn, 1, 5); //貫穿 燃燒
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.poisoned, 1, 5); //貫穿 中毒
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                int preHit3 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit3 > 0)
                {
                    Heal((int)(MaxHp * 0.02f * preHit3)); //回復最大生命 每貫穿1傷害 回復2%最大生命
                }
                break;
            case 4:
                GetShield();
                GetBuff(BuffType.rebound, 3, 2);
                break;
            case 5:
                GetShield();
                GetBuff(BuffType.intellect, 99, 2);
                GetBuff(BuffType.hard, 99, 2);
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
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), "貫穿：給予1回合5燃燒");
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "貫穿：給予1回合5劇毒");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                ShowDamageTip(atk2.gameObject, AttackCheck(), "每貫穿1：回復2%最大生命");
                break;
            case 4:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                def.gameObject.SetActive(false);
                ShowDamageTip(buff.gameObject, DefendCheck(), "獲得3回2尖刺", "blue");
                break;
            case 5:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(true);
                ShowDamageTip(def.gameObject, DefendCheck(), "獲得2智力、2硬甲", "blue");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("boom");
        atk1 = actionObj.transform.Find("poisoned attack");
        atk2 = actionObj.transform.Find("breakAttack");
        buff = actionObj.transform.Find("defend");
        def = actionObj.transform.Find("intellect");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        atk2.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
        def.gameObject.SetActive(false);
    }
}
