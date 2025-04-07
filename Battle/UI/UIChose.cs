using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIChose : UIBase
{
    public CardItem cardItem;
    public void Init(string cardId)
    {

        Dictionary<string, string> data = GameConfigManager.Instance.GetCardById(cardId);
        cardItem = GetComponent<CardItem>();
        cardItem.Init(data);
        //cardItem.OnBeginDrag
    }
}
