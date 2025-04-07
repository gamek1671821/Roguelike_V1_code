using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string Input;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
        //throw new System.NotImplementedException();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //  Debug.Log("OnPointerEnter");
        //throw new System.NotImplementedException();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //  Debug.Log("OnPointerExit");
        // throw new System.NotImplementedException();
    }
}
