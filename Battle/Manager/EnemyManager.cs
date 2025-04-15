using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 關卡生成與敵人管理器,
/// </summary>
public class EnemyManager
{
    public static EnemyManager Instance = new EnemyManager();
    public Dictionary<string, string> data;
    public List<Enemy> enemyList; //存儲的戰鬥中的敵人
    public int allenemy;
    /// <summary>
    /// 加載敵人資源
    /// </summary>
    /// <param name="id">關卡id</param>
    string[] enemyOrEventIds;
    string[] enemyPos;
    public bool EnemySetDone;
    public void LoadRes(string id)
    {
        EnemySetDone = false;
        //
        enemyList = new List<Enemy>();
        //讀取關卡表
        data = GameConfigManager.Instance.GetlevelById(id);

        enemyOrEventIds = data["EnemysIds"].Split('=');//讀取敵人訊息  或是 事件訊息
        GameObject.FindGameObjectWithTag("manager").GetComponent<GodManager>().isBattle = data["isBattle"] == "Y";
        if (data["isBattle"] == "Y")
        { //讀取敵人訊息 

            enemyPos = data["Pos"].Split('='); //敵人位置訊息
            for (int i = 0; i < enemyOrEventIds.Length; i++)
            {
                string enemyId = enemyOrEventIds[i];
                string[] posArr = enemyPos[i].Split(',');
                //敵人位置
                float x = float.Parse(posArr[0]);
                float y = float.Parse(posArr[1]);
                float z = float.Parse(posArr[2]);
                //根據敵人Id獲得單個敵人訊息
                Dictionary<string, string> enemyData = GameConfigManager.Instance.GetEnemyById(enemyId);

                GameObject obj = Object.Instantiate(Resources.Load(enemyData["Model"])) as GameObject; //從資源路徑加載敵人模型

                Enemy enemy = obj.AddComponent(System.Type.GetType(enemyData["Script"])) as Enemy;//添加腳本
                enemy.Init(enemyData);// 儲存敵人訊息
                enemyList.Add(enemy);
                //Debug.Log(enemy);
                obj.transform.position = new Vector3(x, y, z);

            }

        }
        else
        {
            int rChoose = Random.Range(0, enemyOrEventIds.Length); // 隨機從關卡陣列中取出其中一
            string eventSt = enemyOrEventIds[rChoose]; //從關卡陣列中取出其中這個隨機數

            Transform canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform;
            Dictionary<string, string> eventData = GameConfigManager.Instance.GetEventById(eventSt); //載入 eventSt 的資訊

            GameObject obj = Object.Instantiate(Resources.Load($"Event/{eventData["UIName"]}"), canvesTf) as GameObject; //從資源路徑加載面板

            EventBase item = obj.AddComponent(System.Type.GetType(eventData["Script"])) as EventBase; //載入每個事件的專屬腳本
            item.Init(eventData);
        }
        EnemySetDone = true;
        allenemy = enemyList.Count; //儲存開始時 敵人數量
    }
    //移除敵人
    public void DeleteEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count <= 0)
        {
            FightManager.Instance.ChangeType(FightType.Win);
        }
    }
    //執行存活著的怪物的行為
    public IEnumerator DoAllEnemyAction()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            yield return FightManager.Instance.StartCoroutine(enemyList[i].DoAction());
        }

        if (FightManager.Instance.nowFightType == FightType.Enemy)
            //切換到 怪物行動結束
            FightManager.Instance.ChangeType(FightType.Enemy_End);
    }
}
