using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card84_StartCoin : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void DamageText()
    {

    }
    public override void OnPointDamageText()
    {

    }

    public override void CardEffect()
    {
        //指定一敵人，投擲出1硬幣。50%獲得{0}力量、50%目標獲得{0}力量。幸運3：機率改為100%。
        PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
        AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
        int val = int.Parse(data["Arg0"]); //力量
        int dice = Random.Range(0, 101);

        int powerval = val;

        if (FightManager.Instance.buffsVal[(int)BuffType.Lucky] >= 3)
        {
            FightManager.Instance.GetBuff(BuffType.power, 99, powerval); //必定獲得力量
        }
        else
        {
            if (dice >= 50)
            {
                if (FightManager.Instance.CrazyBeastNecklace) //野獸之力項鍊
                {
                    MyFuns.Instance.ShowMessage($"觸發野獸之力項鍊");
                    powerval += val;
                }
                FightManager.Instance.GetBuff(BuffType.power, 99, powerval); //獲得力量
            }
            else
            {
                hitEnemy.GetBuff(BuffType.power, 99, val); //敵人獲得力量
            }
        }

        FatalAttackdetermination(); //確認傷害是否致死
        CardEffectEnd();//卡片效果結束
    }
    public override void BeforeEndEffect()
    {
       // UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this, "B"); //強制被移除
    }
}