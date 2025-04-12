using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class MyFuns
{
    public static MyFuns Instance = new MyFuns();
    public List<int> knifes = new List<int>() { 1036, 1038, 1041, 1042, 1054, 1055, 1056, 1073, 1075, 1076, 1089, 1092 };
    public Level level;
    public class Level
    {
        public List<int> levelDone = new List<int>();
        public static Level InitRoomData()
        { //如果沒有資料，設定初始資料
            var LevelData = new Level();
            LevelData.levelDone.Add(0);
            return LevelData;
        }
    }


    public void SaveLevel()
    {
        PlayerPrefs.SetString("LevelDataSaver" + GodManager.Instance.SaveData_ID, JsonUtility.ToJson(level));
    }
    public void LoadLevel()
    {
        level = JsonUtility.FromJson<Level>(PlayerPrefs.GetString("LevelDataSaver" + GodManager.Instance.SaveData_ID));
    }
    public void Init()
    {
        LoadLevel();
        if (level == null) //
        {
            level = Level.InitRoomData();
            SaveLevel();
        }
    }
    /// <summary>
    /// 根據卡片Id 返回 data
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public Dictionary<string, string> ChoseOneCard(object Id)
    {
        string idString = Id.ToString(); // 將參數統一轉換為字串
        return GameConfigManager.Instance.GetCardById(idString);
    }
    /// <summary>
    /// 根據Id跟"職業" 確認此卡是否包含此職業
    /// </summary>
    /// <param name="Id">請輸入int string</param>
    /// <param name="Profession"></param>
    /// <returns></returns>
    public bool CardCheckProfession(object Id, string Profession)
    {
        Dictionary<string, string> data = ChoseOneCard(Id);
        string[] ProfessioData = data["cardPool"].Split('=');
        return ProfessioData.Contains(Profession);

    }
    /// <summary>
    /// 取得一張卡(符合職業)
    /// </summary>
    /// <param name="pickCard"></param>
    /// <returns></returns>
    public int pickOneCard(string pickCard)
    {
        int cardID;
        do
        {
            cardID = FightManager.Instance.PickCard(pickCard); //隨機選擇全牌庫
            MyFuns.Instance.CardCheckProfession(cardID, GodManager.Instance.profession);
        } while (!MyFuns.Instance.CardCheckProfession(cardID, GodManager.Instance.profession)); //如果不是符合職業的卡 重新迴圈
        return cardID;
    }
    /// <summary>
    /// 抽出 "CardCount" 的卡牌
    /// </summary>
    /// <param name="CardCount"></param>
    public void DrawCard(int CardCount)
    {
        int drawLimit = Mathf.Clamp(CardCount, 0, FightCardManager.Instance.cardList.Count + FightCardManager.Instance.usedCardList.Count);
        UIManager.Instance.GetUI<FightUI>("FightUI").CreatCardItem(drawLimit);
    }
    /// <summary>
    /// 將卡牌 放到牌頂
    /// </summary>
    /// <param name="CardCount"></param>
    public void PutCardOnDeck(object cardID, bool drawOne = false)
    {
        string idString = cardID.ToString(); // 將參數統一轉換為字串
        FightCardManager.Instance.cardList.Add(idString); //牌堆上方追加
        if (drawOne) DrawCard(1);
    }
    public void GetGold(int Gold)
    {
        int totalGold = Mathf.Clamp(PlayerPrefs.GetInt("Gold" + GodManager.Instance.SaveData_ID) + Gold, 0, int.MaxValue);

        PlayerPrefs.SetInt("Gold" + GodManager.Instance.SaveData_ID, totalGold);
    }
    public int Gold()
    {
        return PlayerPrefs.GetInt("Gold" + GodManager.Instance.SaveData_ID);
    }
    public bool HaveItem(ItemData itemData)
    {
        return RoleManager.Instance.roleItem.ItemList.Contains(((int)itemData).ToString());
    }
    public void RestoreHp(int plusHP)
    {
        FightManager.Instance.CurHp += plusHP;
        if (FightManager.Instance.CurHp > FightManager.Instance.MaxHp)
        {
            FightManager.Instance.CurHp = FightManager.Instance.MaxHp;
        }

        UIManager.Instance.GetUI<FightUI>("FightUI")?.UpdateHp();
    }
    public void GetKnife(int Xcount)
    {
        for (int i = 0; i < Xcount; i++) //重複放 幾張刀刃到牌頂
        {
            int RR = UnityEngine.Random.Range(0, knifes.Count);
            PutCardOnDeck(knifes[RR]); //放一張隨機刀刃到牌頂
            DrawCard(1);
        }
    }
    public enum MessageType { Player, Enemy, Item }
    public void ShowMessage(string message, MessageType MType = MessageType.Player)
    {
        bool zeroDamage1 = message.Contains("擋下0傷害");
        bool zeroDamage2 = message.Contains("受到0傷害");
        if (!zeroDamage1 && !zeroDamage2)
        {
            switch (MType)
            {
                case MessageType.Player:
                    FightUI.Instance.damageText.text += $"\n{message}";
                    break;
                case MessageType.Enemy:
                    FightUI.Instance.damageText.text += $"\n<color=#4A7EFF>{message}</color>";
                    break;
                case MessageType.Item:
                    FightUI.Instance.damageText.text += $"\n<color=#A74AFF>{message}</color>";
                    break;
            }
        }
    }
    public Vector3 T2V(Transform input)
    {
        return new Vector3(input.position.x, input.position.y, input.position.z);
    }
}

