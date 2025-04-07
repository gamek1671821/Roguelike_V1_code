using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy10016_Berserker : Enemy
{
    private Transform atk0, atk1, atk2, DEF0, buff;
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
                int preHit = Mathf.Clamp(EnemyCheckHit((int)(AttackCheck() * 2)), 0, int.MaxValue);
                if (preHit == 0) FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 2), this);//沒有貫穿 傷害加倍
                else
                {
                    FightManager.Instance.GetPlayerHit(FightManager.Instance.shieldCount + (int)(preHit * 0.5f), this);  //貫穿後剩餘傷害 回調
                    curHp += (int)(preHit * 0.5f); //回復貫穿傷害
                }
                updateHp();

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 2:
                int preHit2 = Mathf.Clamp(EnemyCheckHit(AttackCheck()), 0, int.MaxValue);
                if (preHit2 > 0)
                {
                    FightManager.Instance.GetDeBuff(DeBuffType.dizz, 1, 1); //給予暈眩
                }
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);

                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);

                break;
            case 3:
                GetBuff(BuffType.power, 2, Attack);
                NextIsRegulate(1);
                break;
            case 4:
                GetShield(); //回甲
                FightManager.Instance.GetPlayerHit((int)(AttackCheck() * 0.5f), this);
                updateAllStatus();
                break;
            case 5:
                foreach (var enemy in EnemyManager.Instance.enemyList)
                {
                    enemy.GetShield(); //回甲
                    enemy.curHp += AttackCheck();
                    enemy.GetBuff(BuffType.power, 2, 1);
                }
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
                atk2.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk0.gameObject, AttackCheck(), "傷害對護甲加倍，回復貫穿傷害");
                break;
            case 2:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(true);
                atk2.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk1.gameObject, AttackCheck(), "貫穿：給予1回1暈眩");
                break;
            case 3:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(true);
                DEF0.gameObject.SetActive(false);
                buff.gameObject.SetActive(false);
                ShowDamageTip(atk2.gameObject, Attack, "獲得等同基礎攻擊力的力量", "green");
                break;
            case 4:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(true);
                buff.gameObject.SetActive(false);
                ShowDamageTip(DEF0.gameObject, (int)(AttackCheck() * 0.5f), $"獲得{DefendCheck()}護甲");
                break;
            case 5:

                atk0.gameObject.SetActive(false);
                atk1.gameObject.SetActive(false);
                atk2.gameObject.SetActive(false);
                DEF0.gameObject.SetActive(false);
                buff.gameObject.SetActive(true);
                ShowDamageTip(buff.gameObject, 0, "給予隊友護甲、基於自身攻擊力的回復、1力量", "white");
                break;
        }
    }
    public override void setTf()
    {
        atk0 = actionObj.transform.Find("breakAttack");
        atk1 = actionObj.transform.Find("dizzAttack");
        atk2 = actionObj.transform.Find("Buff");
        DEF0 = actionObj.transform.Find("defend");
        buff = actionObj.transform.Find("AOEBuff");
    }
    public override void HideAction()
    {
        atk0.gameObject.SetActive(false);
        atk1.gameObject.SetActive(false);
        atk2.gameObject.SetActive(false);
        DEF0.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);

    }
}
