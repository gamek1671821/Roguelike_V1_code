using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 介面 基類
/// </summary>
public class UIBase : MonoBehaviour
{
    //註冊事件
    public UIEventTrigger Regisater(string name)
    {
       Transform tf = transform.Find(name);
       return UIEventTrigger.Get(tf.gameObject);
    }
    // 顯示UI
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    //隱藏UI
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    //銷毀UI
    public virtual void Close()
    {
        UIManager.Instance.CloseUI(gameObject.name);
    }
}

