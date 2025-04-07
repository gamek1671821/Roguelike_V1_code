using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_dungeon : UIManager
{
    public static new UIManager_dungeon Instance;
    public void Awake()
    {
        Instance = this;
        canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
    }
    public GameObject CreatCardIcon()
    {
        GameObject obj = Instantiate(Resources.Load("UI/BookIcon"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位
        return obj;
    }
    public GameObject CreatCardBook()
    {
      
        GameObject obj = Instantiate(Resources.Load("UI/Book"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位
        return obj;
    }
}
