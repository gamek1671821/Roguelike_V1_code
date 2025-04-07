
using UnityEngine;

public class Event001 : EventBase
{
    public override string CustomizedStory()
    {
        int curHp = PlayerPrefs.GetInt("CurHP" + GodManager.Instance.SaveData_ID);
        int maxHp = PlayerPrefs.GetInt("MaxHP" + GodManager.Instance.SaveData_ID);
        return $"{data["Story"]} <color=red>現在生命HP:{curHp}/{maxHp} </color>";
    }
}
