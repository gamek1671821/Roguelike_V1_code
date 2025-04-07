using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowAllCard : MonoBehaviour
{
    private TextMeshProUGUI cardCount;
    public void Start()
    {
        cardCount = GetComponentInChildren<TextMeshProUGUI>();
        var roleManager = RoleManager.Instance;
        roleManager.LoadCardList();
        if (roleManager.roleCard == null)
        {
            roleManager.roleCard = RoleManager.CardListData.InitRoomData();
            roleManager.AddBaseCard(); //基礎卡牌
        }
        else
        {
            roleManager.LoadCardList();
        }
        cardCount.text = roleManager.roleCard.cardList.Count.ToString();
    }
}
