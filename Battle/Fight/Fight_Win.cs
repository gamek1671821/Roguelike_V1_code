using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fight_Win : FightUnit
{   
    public System.Action degOnPlayerWin;
    public Fight_Win(System.Action onPlayerWin) //建構子(委派)
    {
        degOnPlayerWin = onPlayerWin;
    }
    public override void Init()
    {
        UIManager.Instance.showTip("勝利", Color.green, delegate ()
        {
            if (degOnPlayerWin != null)
                degOnPlayerWin.Invoke();
            //FightManager.Instance.OnPlayerWin(); 
            //GameObject.FindGameObjectWithTag("win").GetComponent<EndFight>().WinEnd();
        });
        
    }
    public override void OnUpdate()
    {

    }
  }