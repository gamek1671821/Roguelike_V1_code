using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fight_Back : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.showTip("即將返回地城", Color.green, delegate ()
        {
            GodManager GM = GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>();
            GM.battleWin = true;

            FightManager.Instance.WinSettlement(); //生命存檔
            //load場景
            SceneManager.LoadScene("dungeon");
            var scene = SceneManager.GetSceneByName("dungeon");

        });
    }
    public override void OnUpdate()
    {

    }
}
