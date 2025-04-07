using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//遊戲配置表類 每個對象對應一個txt配置表
public class GameConfigData
{
    private List<Dictionary<string, string>> dataDic; //儲存配置表中的所有數據
    public GameConfigData(string str)
    {
        dataDic = new List<Dictionary<string, string>>();
        //換行切割
        string[] lines = str.Split('\n');
        //第一行是儲存數據的類型
        string[] title = lines[0].Trim().Split('\t'); // tab切割
                                                      //從第三行下標2開始迴圈，第二行是解釋說明
        for (int i = 2; i < lines.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] tempArr = lines[i].Trim().Split('\t');
            for (int j = 0; j < tempArr.Length; j++)
            {
                dic.Add(title[j], tempArr[j]);
            }
            dataDic.Add(dic);
        }
    }
    public List<Dictionary<string, string>> GetLines()
    {
        return dataDic;
    }
    public Dictionary<string, string> GetOneById(string id)
    {
        for (int i = 0; i < dataDic.Count; i++)
        {
            Dictionary<string, string> dic = dataDic[i];
            if (dic["Id"] == id)
            {
                return dic;
            }
        }
        return null;
    }
}