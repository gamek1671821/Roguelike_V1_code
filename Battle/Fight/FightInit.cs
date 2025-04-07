using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;

//卡牌戰鬥初始化
public class FightInit : FightUnit
{
    private bool isWin = false;
    private bool ItemSetDone = false;
    private MonoBehaviour context;
    public FightInit(MonoBehaviour context)
    {
        this.context = context;
    }

    public GameObject Spawn(GameObject prefab, Transform transform)
    {
        return context != null ? Object.Instantiate(prefab, transform) : null;
    }
    public override void Init()
    {


        //初始化戰鬥數值
        FightManager.Instance.Init();
        //切換BGM
        AudioManager.Instance.PlayBGM("battle");
        //生成敵人 
        int Res = GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>().Res;
        EnemyManager.Instance.LoadRes(Res.ToString()); //用關卡ID讀取敵人

        RoleManager.Instance.BattleStartCardList();//複製一份戰鬥卡牌表
        FightCardManager.Instance.Init();

        _ = InitV2(); // `_ =` 代表不等待結果，直接執行 (類似IEnumerator 協程)


    }
    public async Task InitV2()
    {
        while (!EnemyManager.Instance.EnemySetDone)
        {
            await Task.Delay(1); // 等待 1 毫秒，避免無限迴圈卡住主線程
        }
        if (GodManager.Instance.isBattle) CheckItem();
        while (!ItemSetDone)
        {
            await Task.Delay(1);
        }
        if (!isWin)
        {
            var managerObject = GameObject.FindGameObjectWithTag("manager");
            if (managerObject != null && managerObject.GetComponent<GodManager>().isBattle)
            {
                FightManager.Instance.ChangeType(FightType.Player_Start);
                UIManager.Instance.ShowUI<FightUI>("FightUI");
                Transform canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
                Spawn(Resources.Load("UI/Item") as GameObject, canvesTf);
            }
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    private void CheckItem()  //戰鬥開始時觸發的道具
    {
        if (MyFuns.Instance.HaveItem(ItemData.Sword)) //如果持有 "劍" 道具
        {
            FightManager.Instance.GetBuff(BuffType.power, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.Shiled)) //如果持有 "盾" 道具
        {
            FightManager.Instance.GetBuff(BuffType.hard, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.rebound)) //如果持有 "尖刺" 道具
        {
            FightManager.Instance.GetBuff(BuffType.rebound, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.book)) //如果持有 "魔法書" 道具
        {
            FightManager.Instance.GetBuff(BuffType.intellect, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.amulet)) //如果持有 "喵喵符" 道具
        {
            FightManager.Instance.GetBuff(BuffType.Lucky, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.PowerGem)) //如果持有 "活動寶石" 道具
        {
            FightManager.Instance.MaxMoveCount += 1; //最大力量+1
        }
        if (MyFuns.Instance.HaveItem(ItemData.DynamicShiled)) //如果持有 "動態護甲" 道具
        {
            FightManager.Instance.shieldCount += 5; //獲得5護甲
        }
        if (MyFuns.Instance.HaveItem(ItemData.sneakAttack)) //如果持有 "影之雕像" 道具
        {
            FightManager.Instance.sneakAttack = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.Relics))
        {
            FightManager.Instance.Relics = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.LifeBarrier))//生命屏障
        {
            FightManager.Instance.LifeBarrier = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.AvoidOrb))//閃避寶珠
        {
            FightManager.Instance.AvoidOrb = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.PoisonousBook))//猛毒提煉書
        {
            FightManager.Instance.GetBuff(BuffType.powerpoisoned, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.FlashPotion))//速戰藥劑
        {
            FightManager.Instance.GetBuff(BuffType.power, 99, 3);
        }
        if (MyFuns.Instance.HaveItem(ItemData.FailPotion))//混濁藥劑
        {
            int Rd, RdType;

            do
            {
                Rd = Random.Range(0, 101);
                RdType = Random.Range(0, 6); //有6種能力要抽取
                if (Rd >= 50)
                {
                    switch (RdType)
                    {
                        case 0:
                            FightManager.Instance.GetBuff(BuffType.power, 99, 1);
                            break;
                        case 1:
                            FightManager.Instance.GetBuff(BuffType.hard, 99, 1);
                            break;
                        case 2:
                            FightManager.Instance.GetBuff(BuffType.intellect, 99, 1);
                            break;
                        case 3:
                            FightManager.Instance.GetBuff(BuffType.rebound, 99, 1);
                            break;
                        case 4:
                            FightManager.Instance.GetBuff(BuffType.powerpoisoned, 99, 1);
                            break;
                        case 5:
                            FightManager.Instance.GetBuff(BuffType.Lucky, 99, 1);
                            break;
                    }
                }
                FightManager.Instance.FailPotion += 3;
            } while (Rd >= 50);
        }
        ItemSetDone = true;
        if (MyFuns.Instance.HaveItem(ItemData.BeastNecklace)) //如果持有 "獸娘的項鍊" 道具
        {
            FightManager.Instance.GetBuff(BuffType.Lucky, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.BloodBeastNecklace))//獸娘的血染項鍊
        {
            FightManager.Instance.BloodBeastNecklace = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.Chili) || MyFuns.Instance.HaveItem(ItemData.HalfChili))//獸人族辣椒瓶 (或是半瓶)
        {
            FightManager.Instance.GetBuff(BuffType.power, 1, 3);
            FightManager.Instance.GetDeBuff(DeBuffType.dePower, 1, 3);
            FightManager.Instance.GetBuff(BuffType.Speed, 1, 2);
            FightManager.Instance.GetBuff(BuffType.Draw, 1, 2);
        }
        if (MyFuns.Instance.HaveItem(ItemData.DragonBone))//龍骨
        {
            FightManager.Instance.GetBuff(BuffType.DragonPower, 99, 1);
        }
        if (MyFuns.Instance.HaveItem(ItemData.DarkRedNecklace))//暗紅色項鍊
        {
            FightManager.Instance.DarkRedNecklace = true;
        }
        if (MyFuns.Instance.HaveItem(ItemData.CrazyBeastNecklace))//野獸之力項鍊
        {
            FightManager.Instance.CrazyBeastNecklace = true;
        }
    }
}
