using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Fight : MonoBehaviour
{
    private List<string> inputSequence = new List<string>(); // 暫存輸入的按鍵
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isNumberKeyPressed = false;
        // 檢測數字按鍵 0-9 的輸入
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                inputSequence.Add(i.ToString());
                Debug.Log($"輸入: {i}");
                isNumberKeyPressed = true; // 確認有數字按鍵被按下
            }
        }
        if (Input.anyKeyDown && !isNumberKeyPressed && !Input.GetKeyDown(KeyCode.Return))
        {
            inputSequence.Clear();
        }

        // 按下 Enter 鍵時判斷整個序列
        if (Input.GetKeyDown(KeyCode.Return)) // Return 表示 Enter 鍵
        {
            CheckInput();
        }
    }
    private void CheckInput()
    {
        // 將輸入轉換為字符串
        string inputString = string.Join("", inputSequence).Trim();

        Debug.Log($"玩家輸入: {inputString}");
        int idString = int.Parse(inputString); // 將參數統一轉換為字串
        if (idString >= 1000 && idString <= 1069)
        {
            MyFuns.Instance.PutCardOnDeck(idString);
            MyFuns.Instance.DrawCard(1);
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//更新卡牌位置
        }
        else
        {
            Debug.Log("輸入錯誤，請再試一次！");
        }

        // 清空輸入序列
        inputSequence.Clear();
    }
}
