using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10018_TT_Archer : Enemy
{
    private Transform atk0, atk1, atk2;
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
                FightManager.Instance.GetPlayerHit(AttackCheck()); //不被反彈
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                FightManager.Instance.GetPlayerHit(AttackCheck()); //不被反彈
                GetBuff(BuffType.Lurk, 1, 1); //潛伏1回合 (無法被指定)

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                NextIsRegulate(3);
                break;
            case 3:
                Enemy enemy = EnemyManager.Instance.enemyList[Random.Range(0, EnemyManager.Instance.enemyList.Count)]; //隨機抽一個敵人
                enemy.Heal((int)(enemy.MaxHp * 0.1f)); //回復最大生命 10%
                enemy.GetBuff(BuffType.power, 2, 1); //獲得下回合 1力量
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
                ShowDamageTip(atk0.gameObject, AttackCheck(), "貫穿：給予1回合5燃燒");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                atk2.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "獲得1回合潛伏");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                ShowDamageTip(atk2.gameObject, 1, "給予隨機友軍回復10%最大生命，2回合2力量", "white");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("attack");
        atk1 = actionObj.transform.Find("attack_2");
        atk2 = actionObj.transform.Find("plusHP attack");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        atk2.gameObject.SetActive(false);

    }
}
