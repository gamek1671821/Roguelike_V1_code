using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointShow_BookIcon : PointShow
{
    public List<GameObject> books = new List<GameObject>();
    [System.Serializable]
    public enum ShowType
    {
        cardList, battleCardList, usedCardList, banishCardList, limitCardList
    }
    public ShowType showType;
    public override void OnPointerClick(PointerEventData eventData)
    {
        var canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
        int cardCount = 0;
        List<string> SList = new List<string>(); //暫存全部卡牌
        bool TidyUp = false;
        switch (showType)
        {
            case ShowType.cardList:
                cardCount = RoleManager.Instance.roleCard.cardList.Count; //持有卡牌總數
                SList.AddRange(RoleManager.Instance.roleCard.cardList); //
                TidyUp = true;
                break;
            case ShowType.battleCardList:
                cardCount = FightCardManager.Instance.cardList.Count; //持有卡牌總數
                SList.AddRange(FightCardManager.Instance.cardList);
                SList.Reverse();
                break;
            case ShowType.usedCardList:
                cardCount = FightCardManager.Instance.usedCardList.Count; //持有卡牌總數
                SList.AddRange(FightCardManager.Instance.usedCardList);
                SList.Reverse();
                break;
            case ShowType.banishCardList:
                cardCount = FightCardManager.Instance.banishCardList.Count; //持有卡牌總數
                SList.AddRange(FightCardManager.Instance.banishCardList);
                SList.Reverse();
                break;
            case ShowType.limitCardList:
                cardCount = FightCardManager.Instance.limitCardList.Count; //持有卡牌總數
                SList.AddRange(FightCardManager.Instance.limitCardList);
                SList.Reverse();
                break;
        }

        int page = cardCount / 21; //每一頁有21張牌

        for (int i = 0; i <= page; i += 0)
        {
            var paper = Instantiate(Resources.Load("UI/paper"), canvesTf) as GameObject;
            books.Add(paper);
            var bookCard = books[i].GetComponentInChildren<BookCard>();
            bookCard.OpenBook(i, this, SList, TidyUp);
            i++;
        }
        for (int i = 1; i < books.Count; i++)
        {
            books[i].SetActive(false);
        }
    }
    // public override void OnPointerEnter(PointerEventData eventData)
    // {
    //     var canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
    //     var book = Instantiate(Resources.Load("UI/Book"), canvesTf);
    //     book.GetComponent<BookCard>().OpenBook();
    // }
}
