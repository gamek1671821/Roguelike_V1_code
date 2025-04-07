using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//整個遊戲配置表的管理器
public class GameConfigManager
{
    public static GameConfigManager Instance = new GameConfigManager();
    public GameConfigData cardData; //卡牌表
    private GameConfigData enemyData; //敵人表
    private GameConfigData levelData; //關卡表
    private GameConfigData cardTypeData;//卡牌類型表
    private GameConfigData eventData; //事件表
    private GameConfigData itemData; //道具表

    private TextAsset textAsset;
    //初始化配置文件 (txt文件 儲存到內存中)
    public void Init()
    {
        textAsset = Resources.Load<TextAsset>("Data/card");
        cardData = new GameConfigData(textAsset.text);

        textAsset = Resources.Load<TextAsset>("Data/enemy");
        enemyData = new GameConfigData(textAsset.text);

        textAsset = Resources.Load<TextAsset>("Data/level");
        levelData = new GameConfigData(textAsset.text);
        //Debug.Log(textAsset);
        textAsset = Resources.Load<TextAsset>("Data/cardType");
        cardTypeData = new GameConfigData(textAsset.text);

        textAsset = Resources.Load<TextAsset>("Data/event");
        eventData = new GameConfigData(textAsset.text);

        textAsset = Resources.Load<TextAsset>("Data/item");
        itemData = new GameConfigData(textAsset.text);
    }

    public List<Dictionary<string, string>> GetCardLines()
    {
        return cardData.GetLines();
    }
    public List<Dictionary<string, string>> GetEnemyLines()
    {
        return enemyData.GetLines();
    }
    public List<Dictionary<string, string>> GetlevelLines()
    {
        return levelData.GetLines();
    }
    public Dictionary<string, string> GetCardById(string id)
    {
        return cardData.GetOneById(id);
    }
    public Dictionary<string, string> GetEnemyById(string id)
    {
        return enemyData.GetOneById(id);
    }
    public Dictionary<string, string> GetlevelById(string id)
    {
        return levelData.GetOneById(id);
    }
    public Dictionary<string, string> GetCardTypeById(string id)
    {
        return cardTypeData.GetOneById(id);
    }
    public Dictionary<string, string> GetEventById(string id)
    {
        return eventData.GetOneById(id);
    }
    public Dictionary<string, string> GetItemById(string id)
    {
        return itemData.GetOneById(id);
    }


}
