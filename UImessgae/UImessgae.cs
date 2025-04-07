using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UImessgae : MonoBehaviour, IPointerClickHandler
{

    public string[] input;
    public void OnPointerClick(PointerEventData eventData)
    {
        string Xinput = input[0];

        for (int i = 1; i < input.Length; i++)
        {
            Xinput += $"\n{input[i]}";
        }
        FightUI.Instance.MessageStartFadeOutText(Xinput);
    }

    
}
