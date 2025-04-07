using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 遊戲入口腳本
/// </summary>
public class GameApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初始化配置表管理氣 改為在GM執行
        //GameConfigManager.Instance.Init();
        //初始化音頻管理氣
        AudioManager.Instance.Init();
        //初始化用戶訊息 改為在GM執行
        //RoleManager.Instance.Init();
        //顯示loginUI
        UIManager.Instance.ShowUI<LoginUI>("LoginUI");
        //播放bgm
        AudioManager.Instance.PlayBGM("bgm1");
        //測試
        string name = GameConfigManager.Instance.GetCardById("1001")["Name"];
        print(name);
    }

    // Update is called once per frame
}
