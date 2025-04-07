using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class EndFight : MonoBehaviour
{
    bool next = false;
    private int seletedCard;
    private int allCardCount;
    void Start()
    {
        FightManager.Instance.onPlayerWin += WinEnd;
    }
    void OnDestroy()
    {
        FightManager.Instance.onPlayerWin -= WinEnd;
    }
    public void WinEnd()
    {

        FightManager.Instance.WinSettlement();

        StartCoroutine(Useing());
    }
    private void EndFightUseItem()
    {
        if (MyFuns.Instance.HaveItem(ItemData.LuckyCat)) //如果持有 "招財雕像" 道具
        {
            for (int i = 0; i < EnemyManager.Instance.allenemy; i++)
            {
                int gold = Random.Range(1, 4);
                MyFuns.Instance.GetGold(gold);
            }
        }
        if (MyFuns.Instance.HaveItem(ItemData.BloodNecklace)) //如果持有 "活血項鍊" 道具
        {
            for (int i = 0; i < EnemyManager.Instance.allenemy; i++)
            {
                int RestoreHp = Random.Range(1, 4);
                MyFuns.Instance.RestoreHp(RestoreHp);
            }
        }
        if (MyFuns.Instance.HaveItem(ItemData.LifeBarrier)) // 有生命屏障 失去給予的生命
        {
            FightManager.Instance.MaxHp -= 10;
        }
        if (MyFuns.Instance.HaveItem(ItemData.BloodBeastNecklace)) // 獸娘的血染項鍊 給予失去的生命
        {
            FightManager.Instance.MaxHp += 10;
        }
    }
    private void SpecialLevel() //特殊關卡給予
    {
        switch (GodManager.Instance.Res)
        {
            case 10021:
                //獲得某樣道具
                RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.BeastNecklace).ToString());
                RoleManager.Instance.SaveItemList();
                break;
            default:
                break;
        }
    }
    public IEnumerator Useing()
    {
        if (GodManager.Instance.isBattle) EndFightUseItem(); //結束戰鬥 使用道具(遺物)
        SpecialLevel();

        next = false;
        allCardCount = GameConfigManager.Instance.GetCardLines().Count;

        int chose_0 = MyFuns.Instance.pickOneCard("all"); //第一張 從N牌堆選取一張卡牌 ()

        int chose_1 = MyFuns.Instance.pickOneCard("all");  //第一張 從SR牌堆選取一張卡牌 ()

        int chose_2 = MyFuns.Instance.pickOneCard("all"); //第一張 從全牌堆選取一張卡牌 ()

        Transform canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform;
        var choseboard = Instantiate(Resources.Load("UI/choseboard"), canvesTf); //.GetComponent<Transform>(). SetAsFirstSibling()
        Transform choseboardTf = choseboard.GetComponent<Transform>();
        choseboard.GetComponent<choseboard>().Init(3, 2);

        var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly>();
        cardSHowOnly_0.Init(GameConfigManager.Instance.GetCardById(chose_0.ToString()));
        cardSHowOnly_0.onPointDown += OnCardSelected;
        //CardChose_0.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(0).transform.position.x, choseboardTf.GetChild(0).transform.position.y);
        CardChose_0.GetComponent<Transform>().position = new Vector2(choseboardTf.Find("CardChose0").transform.position.x, choseboardTf.GetChild(0).transform.position.y);

        var CardChose_1 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_1 = CardChose_1.AddComponent<CardItemShowOnly>();
        cardSHowOnly_1.Init(GameConfigManager.Instance.GetCardById(chose_1.ToString()));
        cardSHowOnly_1.onPointDown += OnCardSelected;
        CardChose_1.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(1).transform.position.x, choseboardTf.GetChild(1).transform.position.y);

        var CardChose_2 = Instantiate(Resources.Load("UI/CardChose"), canvesTf);
        var cardSHowOnly_2 = CardChose_2.AddComponent<CardItemShowOnly>();
        cardSHowOnly_2.Init(GameConfigManager.Instance.GetCardById(chose_2.ToString()));
        cardSHowOnly_2.onPointDown += OnCardSelected;
        CardChose_2.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(2).transform.position.x, choseboardTf.GetChild(2).transform.position.y);

        var btnChose_1 = Instantiate(Resources.Load("UI/BtnChose"), canvesTf);
        btnChose_1.GetComponent<Button>().onClick.AddListener(() => { GiveUpCard(0); });
        btnChose_1.GetComponentInChildren<TextMeshProUGUI>().text = "獲得2點最大生命值";
        btnChose_1.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(3).transform.position.x, choseboardTf.GetChild(3).transform.position.y);


        var btnChose_2 = Instantiate(Resources.Load("UI/BtnChose"), canvesTf);
        btnChose_2.GetComponent<Button>().onClick.AddListener(() => { GiveUpCard(1); });
        btnChose_2.GetComponentInChildren<TextMeshProUGUI>().text = "恢復10%總生命的生命值";
        btnChose_2.GetComponent<Transform>().position = new Vector2(choseboardTf.GetChild(4).transform.position.x, choseboardTf.GetChild(4).transform.position.y);

        while (!next)
        {
            yield return null;
        }
        Destroy(choseboard); //刪除面板

        if (seletedCard == 0)
        {
            FightManager.Instance.MaxHp += 2;
            MyFuns.Instance.RestoreHp(2);
        }
        else if (seletedCard == 1)
        {
            MyFuns.Instance.RestoreHp((int)(FightManager.Instance.MaxHp * 0.1f));
        }
        else
        {
            RoleManager.Instance.roleCard.cardList.Add(seletedCard.ToString());
        }


        Destroy(CardChose_0); //刪除面板
        Destroy(CardChose_1); //刪除面板
        Destroy(CardChose_2); //刪除面板
        Destroy(btnChose_1); //刪除面板
        Destroy(btnChose_2); //刪除面板
        RoleManager.Instance.SaveCardList();
        yield return new WaitForSeconds(0.1f);

        GodManager GM = GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>();
        GM.battleWin = true;
        FightManager.Instance.WinSettlement(); //生命存檔

        if (GodManager.Instance.isBossRoom) //是最終房間
        {
            PlayerPrefs.SetInt("Level" + GodManager.Instance.SaveData_ID, PlayerPrefs.GetInt("Level" + GodManager.Instance.SaveData_ID) + 1); //升級
            PlayerPrefs.DeleteKey("RoomDataSaver" + GodManager.Instance.SaveData_ID); //
        }
        yield return new WaitForSeconds(0.1f);
        //load場景
        SceneManager.LoadScene("dungeon");
        //var scene = SceneManager.GetSceneByName("dungeon");
    }
    public void OnCardSelected(int index)
    {
        if (!next)
        {
            seletedCard = index;
            next = true;
        }
    }
    public void GiveUpCard(int index)
    {
        if (!next)
        {
            this.seletedCard = index;
            next = true;
        }
    }
    private int PickCard(string type1) //輸入一個字串
    {
        bool noPick; //不可被選取
        int chose_;
        switch (type1)
        {
            case "all":
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity == "base") noPick = true; //當稀有度 是 基礎卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyN":
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "N") noPick = true; //當稀有度 不是 N卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyUR":
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "UR") noPick = true; //當稀有度 不是 UR卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlySR":
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "SR") noPick = true; //當稀有度 不是 SR卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyR":
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "R") noPick = true; //當稀有度 不是 R卡 不可被選為獎勵

                } while (noPick);
                break;
            default:
                do
                {
                    chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string choseString = chose_.ToString();
                    string Rarity = GameConfigManager.Instance.GetCardById(choseString)["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity == "base") noPick = true; //當稀有度 是 基礎卡 不可被選為獎勵

                } while (noPick);
                break;
        }
        return chose_;
    }
    private int PickCard(string type1, string type2) //輸入兩個字串
    {
        bool noPick; //不可被選取
        int chose_;
        do
        {
            chose_ = 1000 + Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
            noPick = false; //不可被選取(初始化)
            string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

            if (Rarity == "N" || Rarity == type1 || Rarity == type2) noPick = false; //當稀有度 是N卡 或 "type1" 或 "type2"可被選為獎勵
            else noPick = true; //除此之外 不可被選為獎勵

        } while (noPick);
        return chose_;
    }

}

