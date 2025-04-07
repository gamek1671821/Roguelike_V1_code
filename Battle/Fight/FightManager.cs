using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum FightType
{
    None, Init, Player_Start, Player, Player_Before_End, Player_End, Enemy_Start, Enemy, Enemy_End, Win, Loss, Back
}
public enum BuffType
{
    power, hard, intellect, rebound, powerpoisoned, Lucky, ForeverShield, crazyIntellect, crazyPower, crazyPosion, CondenseKnife, Draw, Speed, Lurk, DragonPower
}
public enum DeBuffType
{
    burn, poisoned, dePower, deHard, deIntellect, dizz, paralysis, disappoint, deRebound, target
}

/// <summary>
/// 戰鬥管理器
/// </summary>
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public event Action onPlayerWin, onPlayerEnd; // 
    public FightUnit fightUnit; //戰鬥單元 
    public FightType nowFightType;
    public int MaxHp;//最大血量
    public int CurHp;//當前血量
    public int MaxMoveCount; //最大動點
    public int CurMoveCount;//當前動點
    public int shieldCount;//防禦值
    public GameObject buffObj, deBuffObj;
    public List<GameObject> deBuffsItem = new List<GameObject>();
    public List<GameObject> buffsItem = new List<GameObject>();
    public List<int> buffsTurn = new List<int>();
    public List<int> deBuffsTurn = new List<int>();
    public List<int> buffsVal = new List<int>();
    public List<int> deBuffsVal = new List<int>();

    public bool canUseCard;
    //等待填入 buff表，跳過敵人回合 
    public bool skipEnemyTurn;

    public bool sneakAttack, Relics, LifeBarrier, AvoidOrb, BloodBeastNecklace, DarkRedNecklace, CrazyBeastNecklace;
    public int FailPotion;

    public string lastCardType = ""; //上一張打出的卡牌種類
    public int thisTurnBanishCardCount, lastTurnnBanishCardCount = 0; // 消耗的卡牌總數
    public int thisTurnAttackCount, lastTurnnAttackCount = 0; //攻擊的卡牌總數
    public int thisTurnDefendCount, lastTurnDefendCount = 0; //防禦的卡牌總數
    public int thisTurnSkillkCount, lastTurnSkillkCount = 0; //技能的卡牌總數
    public int thisTurnMagickCount, lastTurnMagickCount = 0; //法術的卡牌總數
    public int thisTurnCardCount, lastTurnCardkCount = 0; //卡牌總數
    public int thisTurnDestroyCount, lastTurnDestroyCount = 0;//摧毀的卡牌總數 

    public static event Action OnInitialized; // 初始化完成事件
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        OnInitialized?.Invoke(); // 初始化完成時觸發事件
        Debug.Log("FightManager.Instance is on");
    }
    public void Init()
    {
        // 設定玩家能力初始值
        MaxHp = PlayerPrefs.GetInt("MaxHP" + GodManager.Instance.SaveData_ID);
        if (MaxHp <= 0)
            MaxHp = 60;
        CurHp = PlayerPrefs.GetInt("CurHP" + GodManager.Instance.SaveData_ID);
        if (CurHp <= 0 || CurHp > MaxHp)
            CurHp = MaxHp;
        MaxMoveCount = 4;
        CurMoveCount = 4;
        shieldCount = 0;
        canUseCard = true;

        skipEnemyTurn = false; //初始化不跳過
        buffObj = UIManager.Instance.CreatBuffItem();
        deBuffObj = UIManager.Instance.CreatDeBuffItem();
        //buffObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position + (Vector3.up) * 0.5f);
        ResetBuff();
        ResetDeBuff();

        //
        //CardEffectEnd();
    }
    public void GetBuff(BuffType buffType, int turn, int val = 0)
    {
        int buffId = (int)buffType;
        Transform tf = buffObj.transform.GetChild(buffId);
        tf.gameObject.SetActive(true);
        if (buffsTurn[buffId] < turn)
        {
            buffsTurn[buffId] = turn;
        }
        buffsVal[buffId] += val;
        SetBuffItem();
    }
    public void GetDeBuff(DeBuffType DeBuffType, int turn, int val = 0)
    {
        int DeBuffId = (int)DeBuffType;
        Transform tf = deBuffObj.transform.GetChild(DeBuffId);
        tf.gameObject.SetActive(true);
        if (deBuffsTurn[DeBuffId] < turn)
        {
            deBuffsTurn[DeBuffId] = turn;
        }
        deBuffsVal[DeBuffId] += val;
        SetDeBuffItem();
    }
    private void ResetBuff()
    {
        for (int i = 0; i < buffObj.transform.childCount; i++)
        {
            Transform tf = buffObj.transform.GetChild(i);
            buffsItem.Add(tf.gameObject);
            buffsTurn.Add(0);//初始化
            buffsVal.Add(0); //初始化
        }
        SetBuffItem();
    }
    private void ResetDeBuff()
    {
        for (int i = 0; i < deBuffObj.transform.childCount; i++)
        {
            Transform tf = deBuffObj.transform.GetChild(i);
            deBuffsItem.Add(tf.gameObject);
            deBuffsTurn.Add(0);//初始化
            deBuffsVal.Add(0); //初始化
        }
        SetDeBuffItem();
    }
    public void SetBuffItem()
    {
        for (int i = 0; i < buffsItem.Count; i++)
        {
            if (buffsTurn[i] <= 0)
            {
                buffsVal[i] = 0;
            }
            else if (buffsVal[i] <= 0)
            {
                buffsTurn[i] = 0;
            }
            buffsItem[i].transform.Find("turn").GetComponent<TextMeshProUGUI>().text = buffsTurn[i].ToString();
            buffsItem[i].transform.Find("val").GetComponent<TextMeshProUGUI>().text = buffsVal[i].ToString();
            buffsItem[i].SetActive(buffsTurn[i] > 0);
        }
    }
    public void SetDeBuffItem()
    {
        for (int i = 0; i < deBuffsItem.Count; i++)
        {
            if (deBuffsTurn[i] <= 0)
            {
                deBuffsTurn[i] = 0;
                deBuffsVal[i] = 0;
            }
            else if (deBuffsVal[i] <= 0)
            {
                deBuffsTurn[i] = 0;
            }
            deBuffsItem[i].transform.Find("turn").GetComponent<TextMeshProUGUI>().text = deBuffsTurn[i].ToString();
            deBuffsItem[i].transform.Find("val").GetComponent<TextMeshProUGUI>().text = deBuffsVal[i].ToString();
            deBuffsItem[i].SetActive(deBuffsTurn[i] > 0);
        }
    }
    public void De_BuffDown(bool isTurnStart)
    {
        List<int> startDownBuff = new List<int>
        {
            (int)BuffType.Speed
        };
        List<int> startDownDeBuff = new List<int>
        {
            (int)DeBuffType.dizz,
            (int)DeBuffType.poisoned,
        };


        if (isTurnStart)
        {
            for (int i = 0; i < startDownBuff.Count; i++)
            {
                buffsTurn[startDownBuff[i]] -= 1;
            }
            for (int i = 0; i < startDownDeBuff.Count; i++)
            {
                deBuffsTurn[startDownDeBuff[i]] -= 1;
            }
        }
        else
        {
            for (int i = 0; i < buffsItem.Count; i++)
            {
                if (!startDownBuff.Contains(i)) // 不包含在開始時 的buff
                {
                    buffsTurn[i] -= 1;
                }
            }
            for (int i = 0; i < deBuffsItem.Count; i++)
            {
                if (!startDownDeBuff.Contains(i)) // 不包含在開始時 的debuff
                {
                    deBuffsTurn[i] -= 1;
                }
            }
        }
        SetBuffItem();
        SetDeBuffItem();
    }
    public void ChangeType(FightType type)
    {
        //Debug.Log("changeTYPE" + type.ToString());
        switch (type)
        {
            case FightType.None:
                nowFightType = FightType.None;
                break;
            case FightType.Init:
                fightUnit = new FightInit(this);
                nowFightType = FightType.Init;
                break;
            case FightType.Player_Start:
                fightUnit = new Fight_PlayerStart();
                nowFightType = FightType.Player_Start;
                break;
            case FightType.Player:
                fightUnit = new Fight_PlayerTurn();
                nowFightType = FightType.Player;
                break;
            case FightType.Player_Before_End:
                fightUnit = new Fight_PlayerBeforeEnd(OnPlayerEndTurn);
                nowFightType = FightType.Player_End;
                break;
            case FightType.Player_End:
                fightUnit = new Fight_PlayerEnd();
                nowFightType = FightType.Player_End;
                break;
            case FightType.Enemy_Start:
                fightUnit = new Fight_EnemyStart();
                nowFightType = FightType.Enemy_Start;
                break;
            case FightType.Enemy:
                fightUnit = new Fight_EnemyTurn();
                nowFightType = FightType.Enemy;
                break;
            case FightType.Enemy_End:
                fightUnit = new Fight_EnemyEnd();
                nowFightType = FightType.Enemy_End;
                break;
            case FightType.Win:
                fightUnit = new Fight_Win(OnPlayerWin);
                nowFightType = FightType.Win;
                break;
            case FightType.Loss:
                fightUnit = new Fight_Loss();
                nowFightType = FightType.Loss;
                break;
            case FightType.Back:
                fightUnit = new Fight_Back();
                nowFightType = FightType.Back;
                break;
        }
        fightUnit.Init();
    }

    //玩家受傷邏輯
    public void GetPlayerHit(int hit, Enemy enemy = null)
    {
        if (enemy != null) //受到來自敵人攻擊
        {
            hit += deBuffsVal[(int)DeBuffType.target];
            if (AvoidOrb)
            {
                AvoidOrb = false;
                int avoid = buffsVal[(int)BuffType.Lucky] * 3;
                int Random = UnityEngine.Random.Range(0, 101);
                if (avoid >= Random) //閃避
                {
                    MyFuns.Instance.ShowMessage($"閃避！");
                    return;
                }
            }
        }

        //扣除護頓
        if (shieldCount >= hit)
        {
            shieldCount -= hit;
            if (enemy != null)
                MyFuns.Instance.ShowMessage($"擋下{enemy.data["Name"]}的{hit}傷害 ");
            else
                MyFuns.Instance.ShowMessage($"擋下{hit}傷害");
        }
        else
        {

            if (enemy != null && DarkRedNecklace) // 受到來自敵人攻擊 && 如果有暗紅色項鍊
            {
                hit += 2;
                MyFuns.Instance.ShowMessage($"暗紅色項鍊觸發");
            }
            if (enemy != null && CrazyBeastNecklace) // 受到來自敵人攻擊 && 野獸之力項鍊
            {
                hit += 1;
                MyFuns.Instance.ShowMessage($"野獸之力項鍊觸發");
            }

            hit = hit - shieldCount;
            shieldCount = 0;

            MyFuns.Instance.RestoreHp(-hit);

            if (enemy != null)
                MyFuns.Instance.ShowMessage($"受到{enemy.data["Name"]}的{hit}傷害 ");
            else
                MyFuns.Instance.ShowMessage($"受到{hit}傷害 ");

            if (CurHp <= 0)
            {
                CurHp = 0;
                ChangeType(FightType.Loss); //切換到戰敗狀態
            }
        }
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
        UIManager.Instance.GetUI<FightUI>("FightUI").UpadteDefense();

        if (CurHp > 0) //總生命還大於0  (攻擊後事件)
        {
            if (buffsVal[(int)BuffType.rebound] > 0) //當擁有反彈
            {
                if (enemy != null)
                {
                    enemy.Hit(buffsVal[(int)BuffType.rebound], true); //給予反彈傷害
                    FatalAttackdetermination(); //確認傷害是否致死
                }
            }
        }

    }
    public bool InterHit_IsDeath(int hit, bool ignoreDefense = false)
    {
        // hit = hit - DefenseCount;
        // DefenseCount = 0;
        MyFuns.Instance.RestoreHp(-hit);
        MyFuns.Instance.ShowMessage($"失去{hit}生命");
        if (CurHp <= 0)
        {
            CurHp = 0;
            ChangeType(FightType.Loss); //切換到戰敗狀態
        }
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
        UIManager.Instance.GetUI<FightUI>("FightUI").UpadteDefense();
        if (CurHp <= 0) return true;
        else return false;
    }
    private void Update()
    {
        if (fightUnit != null)
        {
            fightUnit.OnUpdate();
        }
    }

    internal object StartCoroutine(IEnumerable enumerable)
    {
        throw new NotImplementedException();
    }
    public void WinSettlement()
    {
        PlayerPrefs.SetInt("MaxHP" + GodManager.Instance.SaveData_ID, MaxHp);
        PlayerPrefs.SetInt("CurHP" + GodManager.Instance.SaveData_ID, CurHp);
    }
    public void OnPlayerWin()
    {
        if (onPlayerWin != null) //當沒有人註冊 = null
            onPlayerWin.Invoke();
    }
    public void OnPlayerEndTurn()
    {
        if (onPlayerEnd != null) //當沒有人註冊 = null
            onPlayerEnd.Invoke();
    }
    public void TurnEndResetCount()
    {
        lastTurnnBanishCardCount = thisTurnBanishCardCount;
        lastTurnnAttackCount = thisTurnAttackCount;
        lastTurnDefendCount = thisTurnDefendCount;
        lastTurnSkillkCount = thisTurnSkillkCount;
        lastTurnMagickCount = thisTurnMagickCount;
        lastTurnCardkCount = thisTurnCardCount;
        lastTurnDestroyCount = thisTurnDestroyCount;

        thisTurnBanishCardCount = 0;
        thisTurnAttackCount = 0;
        thisTurnDefendCount = 0;
        thisTurnSkillkCount = 0;
        thisTurnMagickCount = 0;
        thisTurnCardCount = 0;
        thisTurnDestroyCount = 0;

        UpdateCardUsed();
    }
    public int PickCard(string type1) //輸入一個字串
    {
        int allCardCount = GameConfigManager.Instance.GetCardLines().Count;
        Debug.Log($"allCardCount:{allCardCount}");
        bool noPick; //不可被選取
        int chose_;
        switch (type1)
        {
            case "all":
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    if (GameConfigManager.Instance == null)
                    {
                        Debug.LogError("GameConfigManager.Instance 尚未初始化");
                    }
                    string Rarity = "";
                    try
                    {
                        Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"取得卡片稀有度時發生錯誤: {ex.Message}");
                        Debug.LogError($"GameConfigManager.Instance: {GameConfigManager.Instance}");
                        Debug.LogError($"chose_: {chose_}");
                        Debug.LogError($"回傳的卡片資料: {Rarity}");
                    }

                    if (Rarity == "base") noPick = true; //當稀有度 是 基礎卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyN":
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "N") noPick = true; //當稀有度 不是 N卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyUR":
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "UR") noPick = true; //當稀有度 不是 UR卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlySR":
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "SR") noPick = true; //當稀有度 不是 SR卡 不可被選為獎勵

                } while (noPick);
                break;
            case "onlyR":
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity != "R") noPick = true; //當稀有度 不是 R卡 不可被選為獎勵

                } while (noPick);
                break;
            default:
                do
                {
                    chose_ = 1000 + UnityEngine.Random.Range(0, allCardCount); //從全牌堆選取一張卡牌 ()
                    noPick = false; //不可被選取(初始化)
                    string Rarity = GameConfigManager.Instance.GetCardById(chose_.ToString())["Rarity"];// 獲得這張卡的稀有度

                    if (Rarity == "N" || Rarity == type1) noPick = false; //當稀有度 是N卡與 "type1" 可被選為獎勵
                    else noPick = true; //除此之外 不可被選為獎勵

                } while (noPick);
                break;
        }
        return chose_;
    }
    /// <summary>
    /// 確認卡片傷害是否致死
    /// </summary>
    public void FatalAttackdetermination()
    {
        List<Enemy> em = new List<Enemy>();
        foreach (var enemy in EnemyManager.Instance.enemyList) //卡片是否致死
        {
            if (enemy.curHp <= 0)
            {
                em.Add(enemy);
            }
        }
        foreach (var enemy in em)
        {
            EnemyManager.Instance.DeleteEnemy(enemy);
        }
    }
    public void CardEffectEnd(bool turnContinue = true)
    {
        canUseCard = turnContinue;
        //刷新數值
        TurnEndEffect();
    }
    public void TurnEndEffect()
    {//刷新數值
        FightUI fightUI = UIManager.Instance.GetUI<FightUI>("FightUI");
        fightUI.UpdateHp();
        fightUI.UpadtePower();
        fightUI.UpadteDefense();
        fightUI.UpdateCardCount();
        fightUI.UpdateNoCardCount();
        fightUI.UpdateCardItemPos();//更新卡牌位置
        SetBuffItem();//更新buff狀態
        SetDeBuffItem();
        UpdateCardUsed();
    }
    public void UpdateCardUsed()
    {
        FightUI.Instance.UsedCard.text = $"使用:{thisTurnCardCount}\n攻擊:{thisTurnAttackCount}\n防禦:{thisTurnDefendCount}\n技能:{thisTurnSkillkCount}\n法術:{thisTurnMagickCount}\n消耗:{thisTurnBanishCardCount}\n摧毀:{thisTurnDestroyCount}\n上一張:{cardType(lastCardType)}";
    }

    private string cardType(string input)
    {
        switch (input)
        {
            case "10000":
                return "全部";
            case "10001":
                return "攻擊";
            case "10002":
                return "防禦";
            case "10003":
                return "永續";
            case "10004":
                return "攻擊/消耗";
            case "10005":
                return "技能";
            case "10006":
                return "技能/消耗";
            case "10007":
                return "法術";
            case "10008":
                return "法術/消耗";
            case "10009":
                return "防禦/消耗";
            default:
                return null;
        }

    }
    private Queue<Action> taskQueue = new Queue<Action>();

    // 添加函式到待辦處
    public void TurnEndList(Action task)
    {
        taskQueue.Enqueue(task);
        Debug.Log("Task added to queue.");
    }

    // 逐一執行所有函式
    public void ExecuteTasks()
    {
        while (taskQueue.Count > 0)
        {
            var task = taskQueue.Dequeue();
            task?.Invoke(); // 執行函式
        }
    }
}
