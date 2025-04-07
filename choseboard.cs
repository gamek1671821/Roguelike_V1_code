using System.Collections.Generic;
using UnityEngine;

public class choseboard : MonoBehaviour
{
    public List<GameObject> cardChoseT5;
    public List<GameObject> cardChoseT10;
    public List<GameObject> btnChose;
    public List<GameObject> other;

    public void Init(int cardCount, int btuCount, int otherCount = 0)
    {
        if (cardCount <= 5)
        {
            foreach (GameObject card in cardChoseT10)
            {
                card.SetActive(false);
            }

            for (int i = 0; i < cardChoseT5.Count; i++)
            {
                cardChoseT5[i].SetActive(i < cardCount);
            }
        }
        else
        {
            foreach (GameObject card in cardChoseT5)
            {
                card.SetActive(false);
            }
            for (int i = 0; i < cardChoseT10.Count; i++)
            {
                cardChoseT10[i].SetActive(i < cardCount);
            }
        }
        for (int i = 0; i < btnChose.Count; i++)
        {
            btnChose[i].SetActive(i < btuCount);
        }
        for (int i = 0; i < other.Count; i++)
        {
            other[i].SetActive(i < otherCount);
        }
    }
}
