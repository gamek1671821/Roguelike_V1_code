using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItemShowOnly : CardItem , IPointerDownHandler
{
    public CardItem howsCardEffect;
    public event System.Action<int> onPointDown;
    public bool onceClick = true;
   // public event System.Func<int , bool> onPointDown2;
    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }
     public override void OnDrag(PointerEventData eventData)
    {
      
    }
    public override void OnEndDrag(PointerEventData eventData)
    {

    }
    //創建卡牌使用過的特效
    public override void PlayEffect(Vector3 pos)
    {
        
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.01f);
        //transform.SetSiblingIndex(index);
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.black);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 1);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        var cardId = int.Parse(data["Id"]);
        onPointDown?.Invoke(cardId);
    }

    /// <summary>
    /// 計算攻擊力
    /// </summary>
    // public override int CountAttack(string arg)
    // {
    // }
    // public override int CountDefend(string arg)
    // {

    // }
    //  public override int CountIntellect(string arg)
    // {

    // }
}
