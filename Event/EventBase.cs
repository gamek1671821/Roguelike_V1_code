using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EventBase : MonoBehaviour
{
    choseboard cb;
    public Transform canvesTf;
    public bool choseDone;
    public GameObject EndBord;
    public UnityEngine.Object storyBoard;
    public Dictionary<string, string> data;
    public void Init(Dictionary<string, string> eventData)
    {
        choseDone = false;
        data = eventData.ToDictionary(entry => entry.Key, entry => entry.Value); // 複製 字典
        cb = GetComponent<choseboard>();
        cb.Init(int.Parse(data["CardCount"]), int.Parse(data["ChooseCount"]), 1); //設定有幾個按鈕
        canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform;

        //if (data["haveStory"] == "T") //不使用

        CreatStoryBoard(); //如果有故事 且 沒有故事鏈

        CreatButtonOrNextStory();
    }

    private void CreatStoryBoard()
    {
        storyBoard = Instantiate(Resources.Load("UI/" + data["StoryBoradType"]), canvesTf);
        if (data["StoryChain"] == "F")
        {
            storyBoard.GetComponentInChildren<TextMeshProUGUI>().text = CustomizedStory();
        }
        EndBord = storyBoard.GetComponent<Transform>().Find("EndBord").gameObject; //
        EndBord.SetActive(false);
    }
    public void EndBordShow(string txt)
    {
        choseDone = true;
        EndBord.SetActive(true);
        EndBord.transform.Find("EndStory").GetComponent<TextMeshProUGUI>().text = txt;
        EndBord.transform.Find("closeBtn").GetComponent<Button>().onClick.AddListener(() => { _ = InitV2(); });
    }
    public virtual string CustomizedStory()
    {
        return data["Story"];
    }

    public virtual void CreatButtonOrNextStory()
    {
        ButtonSetting(Button0, "恢復10%總生命的生命值", 0);
        ButtonSetting(Button1, "獲得2點最大生命值", 1);
    }

    public UnityEngine.Object ButtonSetting(Action action, string ButtonText, int BtnZoneId)
    {
        var btnChose_ = Instantiate(Resources.Load("UI/BtnChose"), canvesTf);
        btnChose_.GetComponent<Button>().onClick.AddListener(() => { action(); });
        btnChose_.GetComponentInChildren<TextMeshProUGUI>().text = ButtonText;
        btnChose_.GetComponent<Transform>().position = new Vector2(transform.Find("Btn zone" + BtnZoneId).transform.position.x, transform.Find("Btn zone" + BtnZoneId).transform.position.y);

        return btnChose_;
    }
    public virtual void Button0()
    {
        if (!choseDone)
        {
            MyFuns.Instance.RestoreHp((int)(FightManager.Instance.MaxHp * 0.1f));
            EndChoose();
        }
    }

    public virtual void Button1()
    {
        if (!choseDone)
        {
            FightManager.Instance.MaxHp += 2;
            MyFuns.Instance.RestoreHp(2);

            EndChoose();
        }
    }
    public virtual void Button2()
    {

    }
    public void EndChoose(bool FightBack = false)
    {
        choseDone = true;
        if (!FightBack)
            _ = InitV2(); // `_ =` 代表不等待結果，直接執行 (類似IEnumerator 協程)
    }

    private async Task InitV2()
    {
        await Task.Delay(1); // 等待 1 毫秒，避免無限迴圈卡住主線程
        FightManager.Instance.ChangeType(FightType.Back);
    }
}
