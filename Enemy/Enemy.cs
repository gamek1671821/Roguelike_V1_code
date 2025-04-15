using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
// public enum ActionType
// {
//     None,
//     Defend,
//     Attack,
// } 
public class Enemy : MonoBehaviour
{
    public bool nextIsRegulate;
    private string[] nextIsValue;
    public int[] ignoreStep;
    public int nextIsStep;

    private AniInfo aniInfo;
    public Dictionary<string, string> data; //敵人數據表
    public int type;
    public GameObject hpItemObj;
    public GameObject actionObj;
    public GameObject buffObj;
    public GameObject deBuffObj;
    public GameObject statusObj;

    // UI相關
    //public Transform attackTf;
    //public Transform attackTf_1;
    //public Transform defendTf;
    public Text defendTxt;
    public Text hpTxt;
    public Image hpImg;
    //數值相關
    public int shield;
    public int defend;
    public int Attack;
    public int MaxHp;
    public int curHp;


    //buff相關
    public List<GameObject> buffsItem = new List<GameObject>();
    public List<GameObject> deBuffsItem = new List<GameObject>();
    public List<int> buffsTurn = new List<int>();
    public List<int> deBuffsTurn = new List<int>();
    public List<int> buffsVal = new List<int>();
    public List<int> deBuffsVal = new List<int>();
    public SkinnedMeshRenderer _meshRenderer;
    public Animator ani;
    private int actionCount = 0; //動作次數

    private int loopFirst;
    private TextMeshProUGUI damageText;
    public SphereCollider hitBox;

    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
    }
    void Start()
    {
        nextIsValue = data["ActionIgnore"].Split('='); //敵人位置訊息
        ignoreStep = nextIsValue
            .Where(str => int.TryParse(str, out _)) // 過濾掉無效的字串
            .Select(int.Parse) // 轉換為整數
            .ToArray();

        _meshRenderer = transform.Find("OutLine").GetComponent<SkinnedMeshRenderer>();
        //damageText = GameObject.Find("damageText").GetComponent<TextMeshProUGUI>();

        ani = transform.GetComponent<Animator>();
        aniInfo = GetComponent<AniInfo>();

        type = 0;
        hpItemObj = UIManager.Instance.CreatHpItem();
        actionObj = UIManager.Instance.CreatActiobIcon(data["actionIcon"]);
        buffObj = UIManager.Instance.CreatBuffItem();
        deBuffObj = UIManager.Instance.CreatDeBuffItem();
        statusObj = UIManager.Instance.CreatStatusItem();

        setTf();

        defendTxt = hpItemObj.transform.Find("fangyu/Text").GetComponent<Text>();
        hpTxt = hpItemObj.transform.Find("moneyTxt").GetComponent<Text>();
        hpImg = hpItemObj.transform.Find("fill").GetComponent<Image>();
        //設置血條 行動力位置
        hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position + (Vector3.up) * 0.3f);
        actionObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position - (Vector3.right) * 0.2f + (Vector3.up) * 0.6f);
        buffObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position + (Vector3.up) * 1.4f);
        deBuffObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position + (Vector3.up) * 1.1f);
        statusObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position + (Vector3.up) * 0.2f);

        //初始化數值
        Attack = int.Parse(data["Attack"]);   //
        curHp = int.Parse(data["Hp"]);
        MaxHp = curHp;
        shield = int.Parse(data["shield"]); //初始護甲
        defend = int.Parse(data["Defend"]);

        nextIsRegulate = data["NextIsRegulate"] == "Y";
        nextIsStep = int.Parse(data["loopFirst"]);

        updateHp();
        updateShield();
        ResetBuff(); //初始化buff狀態
        ResetDeBuff();//初始化debuff狀態
        SetRendomAction();//設定第一次動作
        StatusChange();

        hitBox = GetComponent<SphereCollider>();
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
    private void SetDeBuffItem()
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

    public void updateHp()
    {
        hpTxt.text = curHp + "/" + MaxHp;
        hpImg.fillAmount = (float)curHp / (float)MaxHp;
    }
    public void updateShield()
    {
        defendTxt.text = shield.ToString();
    }
    public void updateAllStatus()
    {
        updateHp();
        updateShield();
    }
    public virtual void OnSelect() //被選中
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.red); ;
        // _meshRenderer.material.SetFloat("_Outline_Width", 50);
        //   _meshRenderer.material.SetColor("_Outline_Color", Color.red);
    }

    public virtual void UnOnSelect() //未被選中
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.black);
        //  _meshRenderer.material.SetFloat("_Outline_Width", 0);
        // _meshRenderer.material.SetColor("_Outline_Color", Color.black);
    }
    /// <summary>
    /// 預先處理玩家對敵人傷害 (傷害與護盾差)
    /// </summary>
    public int CheckHit(int val)
    {//penetrate  貫穿 
        return (val - shield);
    }
    /// <summary>
    /// 預先處理敵人對玩家的傷害
    /// </summary>
    /// <returns></returns>
    public int EnemyCheckHit(int val)
    {
        return val - FightManager.Instance.shieldCount;
    }
    public int Hit(int val, bool NonSpecify)
    {
        int penetrate ; //紀錄貫穿多少傷害
        if (!NonSpecify) //如果不是非指定攻擊  (會受到反傷與針對)
        {
            val += deBuffsVal[(int)DeBuffType.target];
        }
        if (shield >= val) //非貫穿
        {
            shield -= val;
            ani.Play("hit", 0, 0);
            MyFuns.Instance.ShowMessage($"{data["Name"]}擋下{val}傷害", MyFuns.MessageType.Enemy);
            penetrate = 0;
        }
        else
        {
            val = val - shield;
            shield = 0;
            curHp -= val;
            MyFuns.Instance.ShowMessage($"{data["Name"]}受到{val}傷害", MyFuns.MessageType.Enemy);
            penetrate = val;
        }

        if (data["Dragon"] == "T")
        {
            int dragonDamage = FightManager.Instance.buffsVal[(int)BuffType.DragonPower]; //提高受到的傷害
            InterHit_UnDeath(dragonDamage, true); // 額外受到滅龍傷害
            MyFuns.Instance.ShowMessage($"{data["Name"]}受到{val}滅龍傷害", MyFuns.MessageType.Enemy);
        }
        int deathLine = 0;
        if (FightManager.Instance.DarkRedNecklace) //如果有暗紅色項鍊
        {
            deathLine = (int)(MaxHp * 0.1f); //死亡線改為 10%
        }


        //死亡判定
        if (curHp <= deathLine)
        {
            curHp = 0; //生命歸零
            MyFuns.Instance.ShowMessage($"擊殺{data["Name"]} (死亡線：{deathLine})");
            DieSetting();
        }
        else
        {
            ani.Play("hit", 0, 0);
            if (!NonSpecify) FightManager.Instance.GetPlayerHit(buffsVal[(int)BuffType.rebound]);
        }
        updateShield();
        updateHp();
        return penetrate;
    }
    public bool InterHit_IsDeath(int val, bool closeMessgae = false)
    {
        curHp -= val;
        if (!closeMessgae)
            MyFuns.Instance.ShowMessage($"{data["Name"]}受到{val}傷害", MyFuns.MessageType.Enemy);
        if (curHp <= 0)
        {
            DieSetting();

            updateShield();
            updateHp();
            Hit(0, true);
            return true;
        }
        else
        {
            updateShield();
            updateHp();
            Hit(0, true);
            return false;
        }
    }
    public bool InterHit_UnDeath(int val, bool closeMessgae = false)
    {
        curHp -= val;
        if (!closeMessgae)
            MyFuns.Instance.ShowMessage($"{data["Name"]}受到{val}傷害", MyFuns.MessageType.Enemy);
        if (curHp <= 0)
        {
            DieSetting();

            updateShield();
            updateHp();
            return true;
        }
        else
        {
            updateShield();
            updateHp();
            return false;
        }
    }
    private void DieSetting()
    {
        curHp = 0;
        ani.Play("die");
        int gold = Random.Range(int.Parse(data["mingold"]), int.Parse(data["maxgold"]) + 1); //隨機獲得金錢
        MyFuns.Instance.GetGold(gold);
        MyFuns.Instance.ShowMessage($"獲得{gold}金幣");
        //EnemyManager.Instance.DeleteEnemy(this);
        Destroy(gameObject, 1);
        Destroy(actionObj);
        Destroy(hpImg);
    }

    public IEnumerator DoAction()
    {
        HideAction();
        if (deBuffsTurn[(int)DeBuffType.dizz] <= 0) //如果沒有暈眩 
        //播放對應動畫
        {
            CustomizedDoAction_anim();
            //等待某個時間執行對應動作
            yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length * 0.2f);
            CustomizedDoAction_attack();
        }
        else
        {
            nextIsRegulate = false; //解除固定 
        }
        SetRendomAction();
    }
    public virtual void CustomizedDoAction_anim()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                ani.Play("attack");
                //這裡可以撥放動畫
                break;
            case 2:
                ani.Play("attack");
                break;
        }
        //等待某個時間執行對應動作
    }
    public virtual void CustomizedDoAction_attack()
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                shield += DefendCheck();
                updateShield();
                //這裡可以撥放動畫
                break;
            case 2:
                //玩家扣血
                FightManager.Instance.GetPlayerHit(AttackCheck(), this);
                //攝影機晃動
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
        }
    }
    public virtual void SetRendomAction() { }
    public virtual void setTf() { }
    //關閉怪物頭上的行動標示
    public virtual void HideAction() { }
    public virtual int AttackCheck()
    {//攻擊扣除負面效果
        return Mathf.Clamp(Attack + buffsVal[(int)BuffType.power] - (deBuffsVal[(int)DeBuffType.dePower] + deBuffsVal[(int)DeBuffType.deIntellect]), 0, 9999);
    }
    public virtual bool HitIsBreck(int Attack)
    {
        return Attack > FightManager.Instance.shieldCount;
    }
    public virtual int DefendCheck()
    {
        return Mathf.Clamp(defend + buffsVal[(int)BuffType.hard] - deBuffsVal[(int)DeBuffType.deHard], 0, 9999);
    }
    public void StatusChange()
    {
        statusObj.transform.GetChild(0).Find("val").GetComponent<TextMeshProUGUI>().text = AttackCheck().ToString();
        statusObj.transform.GetChild(1).Find("val").GetComponent<TextMeshProUGUI>().text = DefendCheck().ToString();
    }
    public void ShowDamageTip(GameObject object_, int Input, string ex = null, string Color = "red")
    {
        object_.GetComponentInChildren<TextMeshProUGUI>().text = $"<color={Color}>{Input}</color>  {ex}";
    }
    public void NextIsRegulate(int step)
    {
        nextIsRegulate = true; //固定下一個動作
        nextIsStep = step; // 
    }
    public void Heal(int hp)
    {
        curHp = Mathf.Clamp(curHp + hp, 0, MaxHp);
        updateHp();
    }
    public void GetShield(int shieldCount = -1)
    {
        if (shieldCount == -1)
        {
            shield += DefendCheck();
        }
        else
        {
            shield += shieldCount;
        }
        updateShield();
    }
}