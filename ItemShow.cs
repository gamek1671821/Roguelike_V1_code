using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShow : MonoBehaviour, IPointerClickHandler
{
    private ItemManager itemManager;
    public Dictionary<string, string> data; //道具表
    public void Init(Dictionary<string, string> data, ItemManager itemManager)
    {
        this.data = data;
        GetComponent<Image>().sprite = Resources.Load<Sprite>(data["Icon"]); //設定圖示
        this.itemManager = itemManager;
    }

    public void OnPointerClick(PointerEventData eventData )
    {
        //滑鼠點擊位置 / 文本 / 文字大小
       itemManager.ShowItemText(eventData , $"{data["NameCH"]} : {data["story"]}\n{data["effect"]}" , int.Parse(data["Size"]) ); 
    }

}
