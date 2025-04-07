using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform canvesTf;
    private List<UIBase> uiList = new List<UIBase>(); //儲存家載過的UI

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
    }
    public UIBase ShowUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui == null)
        {
            //List 中沒有，需要從Resources/UI資料夾中加載
            GameObject obj = Instantiate(Resources.Load("UI/" + uiName), canvesTf) as GameObject;
            //改名字
            obj.name = uiName;
            //添加需要的腳本 
            ui = obj.AddComponent<T>();
            //將新ui加到List
            uiList.Add(ui);
        }
        else
        {
            //顯示
            ui.Show();
        }
        return ui;
    }
    public void HideUI(string uiName)
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            ui.Hide();
        }
    }
    public void CloseAllUI()
    {
        for (int i = uiList.Count - 1; i >= 0; i--)
        {
            Destroy(uiList[i].gameObject);
        }
        uiList.Clear(); //清空List
    }
    public void CloseUI(string uiName)
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            uiList.Remove(ui);
            Destroy(ui.gameObject);
        }
    }
    //使用UI名稱從List中找對應腳本
    private UIBase Find(string uiName)
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i].name == uiName)
                return uiList[i];
        }
        return null;
    }
    //獲得某個介面的腳本
    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            return ui.GetComponent<T>();
        }
        return null;
    }
    //創建敵人頭部的行動圖示
    public GameObject CreatActiobIcon(string actionIcon)
    {
        GameObject obj = Instantiate(Resources.Load("UI/" + actionIcon), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位
        return obj;
    }
    //創建敵人底部的血量物體
    public GameObject CreatHpItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/HpItem"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位

        return obj;
    }
    public GameObject CreatBuffItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/BuffItem"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位

        return obj;
    }
    public GameObject CreatDeBuffItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/DeBuffItem"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位

        return obj;
    }
    public GameObject CreatStatusItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/StatusItem"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位

        return obj;
    }

    //提示介面
    public void showTip(string msg, Color color, System.Action callback = null)
    {
        GameObject obj = Instantiate(Resources.Load("UI/Tips"), canvesTf) as GameObject;
        Text text = obj.transform.Find("bg/Text").GetComponent<Text>();
        text.color = color;
        text.text = msg;
        Tween scale1 = obj.transform.Find("bg").DOScaleY(1, 0.4f);
        Tween scale2 = obj.transform.Find("bg").DOScaleY(0, 0.4f);

        Sequence seq = DOTween.Sequence();
        seq.Append(scale1);
        seq.AppendInterval(0.2f);
        seq.Append(scale2);
        seq.AppendCallback(delegate ()
        {
            // if (callback != null)
            // {
            //     callback();
            // }
            callback?.Invoke();
        });
        MonoBehaviour.Destroy(obj, 2);
    }
    /// <summary>
    /// 開啟名詞解釋
    /// </summary>
    public void ExplanationBtn()
    {
        GameObject obj = Instantiate(Resources.Load("UI/Explanation of terms"), canvesTf) as GameObject;
        obj.transform.SetAsLastSibling(); //設置在父級的最後位
    }
}
