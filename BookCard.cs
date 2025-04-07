using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BookCard : MonoBehaviour
{
    PointShow_BookIcon pointShow_BookIcon_;
    public page pagex;
    [System.Serializable]
    public class page
    {
        public int thisPage;
        public Button closeButton;
        public Button nextButton;
        public Button previousButton;
    }
    public GameObject close;
    public void OpenBook(int page, PointShow_BookIcon pointShow_BookIcon ,List<string> CardList , bool TidyUp = true)
    {
        pointShow_BookIcon_ = pointShow_BookIcon;
        //  List<string> CardList = new List<string>();
        if(TidyUp) CardList = RoleManager.Instance.TidyUpCard(CardList); //選擇是否 排列卡堆
        int allPage = CardList.Count / 21;
        for (int i = 0 + 21 * page; i < Mathf.Clamp(21 + (21 * page), 0, CardList.Count); i++)
        {
            //var choseboard = Instantiate(Resources.Load("UI/choseboard")); //.GetComponent<Transform>(). SetAsFirstSibling()
            var CardChose_0 = Instantiate(Resources.Load("UI/CardChose"), this.transform);
            var cardSHowOnly_0 = CardChose_0.AddComponent<CardItemShowOnly_Book>();
            cardSHowOnly_0.Init(GameConfigManager.Instance.GetCardById(CardList[i]));
        }

        pagex.thisPage = page;
        pagex.closeButton.onClick.AddListener(() => { CloseBook(); });
        pagex.nextButton.onClick.AddListener(() => { NextPage(); });
        pagex.previousButton.onClick.AddListener(() => { PreviousPage(); });
    }
    public void CloseBook() // 按鈕
    {
        for (int i = 0; i < pointShow_BookIcon_.books.Count; i++)
        {
            Destroy(pointShow_BookIcon_.books[i]);
        }
        pointShow_BookIcon_.books.Clear();
    }
    public void NextPage() // 按鈕
    {
        for (int i = 0; i < pointShow_BookIcon_.books.Count; i++)
        {
            var page = pagex.thisPage + 1;
            if (page >= pointShow_BookIcon_.books.Count)
            {
                page = 0;
            }
            pointShow_BookIcon_.books[i].SetActive(i == page);
        }
    }
    public void PreviousPage() // 按鈕
    {
        for (int i = 0; i < pointShow_BookIcon_.books.Count; i++)
        {
            var page = pagex.thisPage - 1;
            if (page < 0)
            {
                page = pointShow_BookIcon_.books.Count - 1;
            }
            pointShow_BookIcon_.books[i].SetActive(i == page);
        }
    }
}
