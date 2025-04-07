using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImessgae_chose : UIManager
{
    public static new UImessgae_chose Instance;
    public void Awake()
    {
        Instance = this;
        canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
        ChoseUI();
    }
    /// <summary>
    /// 開啟名詞解釋
    /// </summary>
    public void ChoseUI()
    {
        GameObject obj = Instantiate(Resources.Load("UI/choseUI"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位
    }
}
