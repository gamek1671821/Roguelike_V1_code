using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10027_TT_King : Enemy
{
    private Transform atk0, atk1, buff;
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
                    foreach (var enemy in EnemyManager.Instance.enemyList)
                    {
                        enemy.GetBuff(BuffType.power, 3, 1);
                    }
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
                    foreach (var enemy in EnemyManager.Instance.enemyList)
                    {
                        enemy.GetBuff(BuffType.hard, 3, 1);
                    }
                }

                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 3:
                foreach (var enemy in EnemyManager.Instance.enemyList)
                {
                    enemy.GetShield(DefendCheck()); //根據此角色的防禦力 給予友軍護盾
                }
                NextIsRegulate(1);
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
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), $"貫穿：友軍全體獲得3回1力量");
                break;
            case 2:
                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), $"貫穿：友軍全體獲得3回1硬甲");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, DefendCheck(), $"友軍全體獲得{DefendCheck()}護甲", "blue");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
        atk1 = actionObj.transform.Find("defend");
        buff = actionObj.transform.Find("AOEBuff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
    }
}
