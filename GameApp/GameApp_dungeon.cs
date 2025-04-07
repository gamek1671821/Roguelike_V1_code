using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameApp_dungeon : MonoBehaviour
{
    TextMeshProUGUI hpTxt;
    // Start is called before the first frame update
    void Start()
    {
        UIManager_dungeon.Instance.ShowUI<dungeonUI>("dungeonUI");

    }
}
