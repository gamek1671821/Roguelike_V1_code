using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class LoginUI : UIBase
{
  private void Start()
  {
    //Regisater("bg/startBtn").onClick = onStartGameBtn;
      FightManager.Instance.ChangeType(FightType.Init);   //
    // 關閉
    Close();
  }
  
  private void onStartGameBtn(GameObject obj, PointerEventData pData)
  {
    FightManager.Instance.ChangeType(FightType.Init);   //
    // 關閉
    Close();
  }
}

