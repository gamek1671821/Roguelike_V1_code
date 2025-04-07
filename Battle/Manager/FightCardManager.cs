using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class FightCardManager
{
    public static FightCardManager Instance = new FightCardManager();
    public List<string> cardList; //牌堆
    public List<string> usedCardList; //棄牌堆
    public List<string> banishCardList; //除外區
    public List<string> limitCardList; //只能使用一次的卡
    public void Init()
    {
        cardList = new List<string>();
        usedCardList = new List<string>();
        banishCardList = new List<string>();
        limitCardList = new List<string>();
        //定義臨時集合
        List<string> tempList = new List<string>();
        //將玩家的卡牌儲存到臨時集合
        tempList.AddRange(RoleManager.Instance.roleCard.cardList);
        while (tempList.Count > 0)
        {
            //隨機下標
            int tempIndex = Random.Range(0, tempList.Count);
            //添加到牌堆
            cardList.Add(tempList[tempIndex]);
            //臨時集合刪除
            tempList.RemoveAt(tempIndex);
        }
        // Debug.Log(cardList.Count);
    }
    public void ResetCardList()
    {
        //定義臨時集合
        List<string> tempList = new List<string>();
        //將棄牌堆複製卡牌儲存到臨時集合
        tempList.AddRange(usedCardList);
        while (tempList.Count > 0)
        {
            //隨機下標
            int tempIndex = Random.Range(0, tempList.Count);
            //添加到牌堆
            cardList.Add(tempList[tempIndex]);
            //臨時集合刪除
            tempList.RemoveAt(tempIndex);
        }
        usedCardList.Clear();
    }
    // 是否有卡
    public bool HasCard()
    {
        return cardList.Count > 0;
    }
    public string DrawCard()
    {
        string id = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);
        return id;
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    /// <param name="cardList">哪個牌堆要洗牌</param>
    public void shuffle()
    {
        //定義臨時集合
        List<string> tempList = new List<string>();
        //將牌堆的牌 加到臨時集合中
        tempList.AddRange(cardList);
        while (tempList.Count > 0)
        {
            //隨機下標
            int tempIndex = Random.Range(0, tempList.Count);
            //添加到牌堆
            cardList.Add(tempList[tempIndex]);
            //臨時集合刪除
            tempList.RemoveAt(tempIndex);
        }
    }
}
