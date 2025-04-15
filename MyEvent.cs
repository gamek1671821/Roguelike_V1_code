using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MyEvent
{
    public static MyEvent Instance = new MyEvent();
    public void Event005_SP()
    {
        MyFuns.Instance.level.levelDone.Add(5);
        MyFuns.Instance.SaveLevel();
        RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.BeastNecklace).ToString());
        RoleManager.Instance.SaveItemList();
        Debug.Log("成功觸發");
    }
    public void TriggerEventByName(string methodName)
    {
        MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (method != null)
        {
            method.Invoke(this, null); // 無參數呼叫
        }
        else
        {
            Debug.Log($"找不到方法：{methodName}");
        }
    }

}
