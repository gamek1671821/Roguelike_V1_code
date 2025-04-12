using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager
{
    public static RoleManager Instance = new RoleManager();
    public CardListData roleCard;
    public class CardListData
    {
        public List<string> cardList = new List<string>(); //儲存擁有的卡牌id
        public static CardListData InitRoomData()
        { //如果沒有資料，設定初始資料
            var cardData = new CardListData();
            return cardData;
        }
    }
    public ItemListData roleItem;
    public class ItemListData
    {
        public List<string> ItemList = new List<string>(); //儲存擁有的道具id
        public static ItemListData InitRoomData()
        { //如果沒有資料，設定初始資料
            var ItemData = new ItemListData();
            return ItemData;
        }
    }
    // 用戶訊息管理器 (擁有的卡牌 金幣...)
    public List<string> tempCardList = new List<string>(); // 戰鬥開始前儲存的卡表
    public void BattleStartCardList()
    {
        tempCardList.AddRange(Instance.roleCard.cardList); //儲存一份原始卡表
    }
    public void BattleEndCardList()
    {
        roleCard.cardList.Clear();
        roleCard.cardList.AddRange(tempCardList);
    }
    public void AddBaseCard() //新增基礎卡
    {
        //string profession = PlayerPrefs.GetString("profession" + GodManager.Instance.SaveData_ID);
        string profession = GodManager.Instance.profession;
        switch (profession)
        {
            case "Warrior":
                roleCard.cardList.Add("1003");//橫掃
                roleCard.cardList.Add("1005");//狂戰怒火
                roleCard.cardList.Add("1011");//盾斬
                break;
            case "Wise":
                roleCard.cardList.Add("1029");//禁忌知識
                roleCard.cardList.Add("1043");//光束魔箭
                roleCard.cardList.Add("1047");//煉化硬盾
                break;
            case "Thief":
                roleCard.cardList.Add("1037");//小刀包
                roleCard.cardList.Add("1031");//賭徒之力
                roleCard.cardList.Add("1026");//毒息殺
                break;
        }
        roleCard.cardList.Add("1000"); // 5張普通攻擊
        roleCard.cardList.Add("1000");
        roleCard.cardList.Add("1000");
        roleCard.cardList.Add("1000");

        roleCard.cardList.Add("1001");// 5張普通防禦
        roleCard.cardList.Add("1001");
        roleCard.cardList.Add("1001");
        roleCard.cardList.Add("1001");
        roleCard.cardList.Add("1002");//抽牌卡 (技能卡)


        roleCard.cardList.Add("1030");//寧息菸
        roleCard.cardList.Add("1049");//緊急針

        //以下為設定
        // roleCard.cardList.Add("1000"); // 5張普通攻擊
        // roleCard.cardList.Add("1001");
        // roleCard.cardList.Add("1002");//抽牌卡 (技能卡)
        // roleCard.cardList.Add("1003");//橫掃
        //roleCard.cardList.Add("1004");
        // roleCard.cardList.Add("1005");
        // roleCard.cardList.Add("1006");
        // roleCard.cardList.Add("1007");
        // roleCard.cardList.Add("1008");
        // roleCard.cardList.Add("1009");
        // roleCard.cardList.Add("1010");
        // roleCard.cardList.Add("1011");
        // roleCard.cardList.Add("1012");
        // roleCard.cardList.Add("1013");
        // roleCard.cardList.Add("1014");
        // roleCard.cardList.Add("1015");
        // roleCard.cardList.Add("1016");
        // roleCard.cardList.Add("1017");
        // roleCard.cardList.Add("1018");
        // roleCard.cardList.Add("1019");
        // roleCard.cardList.Add("1020");
        // roleCard.cardList.Add("1021");
        // roleCard.cardList.Add("1022");
        // roleCard.cardList.Add("1023");
        // roleCard.cardList.Add("1024");
        // roleCard.cardList.Add("1025");
        // roleCard.cardList.Add("1026");
        // roleCard.cardList.Add("1027");
        // roleCard.cardList.Add("1028");
        // roleCard.cardList.Add("1029");
        // roleCard.cardList.Add("1030");
        // roleCard.cardList.Add("1031");
        // roleCard.cardList.Add("1032");
        // roleCard.cardList.Add("1033");
        // roleCard.cardList.Add("1034");
        // roleCard.cardList.Add("1035");
        // roleCard.cardList.Add("1037");
        // roleCard.cardList.Add("1039");
        // roleCard.cardList.Add("1040");
        // roleCard.cardList.Add("1043");
        // roleCard.cardList.Add("1044");
        // roleCard.cardList.Add("1045");
        // roleCard.cardList.Add("1046");
        // roleCard.cardList.Add("1047");
        // roleCard.cardList.Add("1048");
        // roleCard.cardList.Add("1049");
        // roleCard.cardList.Add("1050");
        // roleCard.cardList.Add("1051");
        // roleCard.cardList.Add("1052");
        // roleCard.cardList.Add("1053");
        // roleCard.cardList.Add("1054");
        // roleCard.cardList.Add("1055");
        // roleCard.cardList.Add("1056");
        // roleCard.cardList.Add("1057");
        // roleCard.cardList.Add("1058");
        // roleCard.cardList.Add("1059");
        // roleCard.cardList.Add("1060");
        // roleCard.cardList.Add("1061");
        // roleCard.cardList.Add("1062");
        // roleCard.cardList.Add("1063");
        // roleCard.cardList.Add("1064");
        // roleCard.cardList.Add("1065");
        // roleCard.cardList.Add("1066");
        // roleCard.cardList.Add("1067");
        // roleCard.cardList.Add("1068");
        // roleCard.cardList.Add("1069");
        // roleCard.cardList.Add("1070");
        // roleCard.cardList.Add("1071");
        // roleCard.cardList.Add("1072");
        // roleCard.cardList.Add("1074");
        // roleCard.cardList.Add("1077");
        // roleCard.cardList.Add("1078");
        // roleCard.cardList.Add("1079");
        // roleCard.cardList.Add("1081");
        // roleCard.cardList.Add("1082");
        // roleCard.cardList.Add("1083");
        // roleCard.cardList.Add("1084");
        // roleCard.cardList.Add("1085");
        // roleCard.cardList.Add("1086");
        // roleCard.cardList.Add("1087");
        // roleCard.cardList.Add("1088");
        // roleCard.cardList.Add("1090");
        roleCard.cardList.Add("1091");
        roleCard.cardList.Add("1093");
        roleCard.cardList.Add("1094");
        roleCard.cardList.Add("1095");
        SaveCardList();
    }
    public void ChangeNewCard(string cardID, bool isDelete)
    {
        if (isDelete)
        {
            roleCard.cardList.Remove(cardID); //刪除
        }
        else
        {
            roleCard.cardList.Add(cardID); //新增 
        }
        SaveCardList();
    }
    public void SaveCardList()
    {
        PlayerPrefs.SetString("CardListDataSaver" + GodManager.Instance.SaveData_ID, JsonUtility.ToJson(roleCard));
    }
    public void LoadCardList()
    {
        roleCard = JsonUtility.FromJson<CardListData>(PlayerPrefs.GetString("CardListDataSaver" + GodManager.Instance.SaveData_ID));
    }
    public void SaveItemList()
    {
        PlayerPrefs.SetString("ItemListDataSaver" + GodManager.Instance.SaveData_ID, JsonUtility.ToJson(roleItem));
    }
    public void LoadItemList()
    {
        roleItem = JsonUtility.FromJson<ItemListData>(PlayerPrefs.GetString("ItemListDataSaver" + GodManager.Instance.SaveData_ID));
    }
    public List<string> TidyUpCard(List<string> waitToTidyUp)
    {
        List<string> SList = new List<string>(); //暫存全部卡牌
        SList.AddRange(waitToTidyUp);
        SList.Sort();
        return SList;
    }

}
