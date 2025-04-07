using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class choseManager : MonoBehaviour
{
    public Button warriorBtn, wiseBtn, thiefBtn;
    public Button[] saveBtns;
    public Button explanationBtn;
    public TextMeshProUGUI text;
    public GameObject choseBord;

    public void Start()
    {
        warriorBtn.onClick.AddListener(() => ChoseProfession("Warrior"));
        wiseBtn.onClick.AddListener(() => ChoseProfession("Wise"));
        thiefBtn.onClick.AddListener(() => ChoseProfession("Thief"));
        explanationBtn.onClick.AddListener(() => UImessgae_chose.Instance.ExplanationBtn());
        choseBord.SetActive(false);
        //int id = PlayerPrefs.GetInt("SaveData_ID");
        for (int i = 0; i < saveBtns.Length; i++)
        {
            int capturedIndex = i; // 捕捉當前的 i 值
            saveBtns[capturedIndex].onClick.AddListener(() => ChoseSaveId(capturedIndex));
            SetBtn(saveBtns[capturedIndex], capturedIndex);
            //saveBtns[i].interactable = (i != id);
        }
        text.text = $"選擇你的存檔";
    }
    public void ChoseProfession(string input)
    {
        switch (input)
        {
            case "Warrior":
                GodManager.Instance.profession = "Warrior";
                PlayerPrefs.SetString("profession" + GodManager.Instance.SaveData_ID, "Warrior");
                PlayerPrefs.SetInt("MaxHP" + GodManager.Instance.SaveData_ID, 70);
                PlayerPrefs.SetInt("CurHP" + GodManager.Instance.SaveData_ID, 70);

                break;
            case "Wise":
                GodManager.Instance.profession = "Wise";
                PlayerPrefs.SetString("profession" + GodManager.Instance.SaveData_ID, "Wise");
                PlayerPrefs.SetInt("MaxHP" + GodManager.Instance.SaveData_ID, 60);
                PlayerPrefs.SetInt("CurHP" + GodManager.Instance.SaveData_ID, 60);
                break;
            case "Thief":
                GodManager.Instance.profession = "Thief";
                PlayerPrefs.SetString("profession" + GodManager.Instance.SaveData_ID, "Thief");
                PlayerPrefs.SetInt("MaxHP" + GodManager.Instance.SaveData_ID, 65);
                PlayerPrefs.SetInt("CurHP" + GodManager.Instance.SaveData_ID, 65);
                break;
        }
        GodManager.Instance.LoadCard();
        GodManager.Instance.LoadItem();
        SceneManager.LoadScene("dungeon");
    }
    public void ChoseSaveId(int id)
    {
        GodManager.Instance.SaveData_ID = id;

        if (PlayerPrefs.GetString("profession" + id) == "")
        {
            choseBord.SetActive(true);
        }
        else
        {
            text.text = $"存檔{id} 職業：{PlayerPrefs.GetString("profession" + id)}";
            GodManager.Instance.profession = PlayerPrefs.GetString("profession" + id);
            GodManager.Instance.LoadCard();
            GodManager.Instance.LoadItem();
            SceneManager.LoadScene("dungeon");
        }
    }
    private void SetBtn(Button btn, int id)
    {
        string professionB = PlayerPrefs.GetString("profession" + id);
        if (professionB != "")
        {
            ColorBlock colorBlock = btn.colors;
            colorBlock.normalColor = Color.red;
            btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text += $"職業：{professionB}";
        }
    }
    /// <summary>
    /// 開啟名詞解釋
    /// </summary>
    private void ExplanationBtn()
    {
        var canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
        GameObject obj = MonoBehaviour.Instantiate(Resources.Load("UI/Explanation of terms"), canvesTf) as GameObject;
        obj.transform.SetAsFirstSibling(); //設置在父級的第一位
    }
}
