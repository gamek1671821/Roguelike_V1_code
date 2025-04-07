using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10023_00_Tpose : Enemy
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
                    FightManager.Instance.GetDeBuff(DeBuffType.disappoint, 1, 2); //貫穿  破滅
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
                    FightManager.Instance.GetDeBuff(DeBuffType.paralysis, 1, 2); //貫穿 麻痺
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case 3:
                int preHit3 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit3 > 0)
                {
                    GetShield();
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                break;
            case 4:
                GetShield();
                GetBuff(BuffType.hard, 99, 2);
                break;
            case 5:
                Enemy enemy = EnemyManager.Instance.enemyList[Random.Range(0, EnemyManager.Instance.enemyList.Count)]; //隨機抽一個敵人
                enemy.Heal((int)(enemy.MaxHp * 0.1f)); //回復最大生命 10%
                enemy.GetBuff(BuffType.intellect, 3, 2); //獲得下回合 1智力
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), "貫穿：給予1回合破滅");
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "貫穿：給予1回合2麻痺");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                ShowDamageTip(atk2.gameObject, AttackCheck(), "貫穿：給予1回合2暈眩");
                break;
            case 4:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                def.gameObject.SetActive(false);
                ShowDamageTip(def.gameObject, DefendCheck(), "獲得2硬甲", "blue");
                break;
            case 5:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                def.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 2, "給予隨機友軍回復10%最大生命，3回合2智力");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack_1");
        atk1 = actionObj.transform.Find("paralysisattack");
        atk2 = actionObj.transform.Find("dizzAttack");
        def = actionObj.transform.Find("defend");
        buff = actionObj.transform.Find("Buff");
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
