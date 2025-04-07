using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dungeonUI : UIBase
{
    public static dungeonUI Instance;
    private TextMeshProUGUI hpTxt, goldTxt, professionTxt;
    private Button explanationBtn;
    private Image hpImg;

    void Awake()
    {
        Instance = this;

        var hpTextTransform = transform.Find("hp/Image/Text");
        hpTxt = hpTextTransform.GetComponent<TextMeshProUGUI>();

        var hpFillTransform = transform.Find("hp/fill");
        hpImg = hpFillTransform.GetComponent<Image>();

        var goldTransform = transform.Find("hp/gold/Text");
        goldTxt = goldTransform.GetComponent<TextMeshProUGUI>(); //profession

        var professionTextTransform = transform.Find("hp/profession/Text");
        professionTxt = professionTextTransform.GetComponent<TextMeshProUGUI>();

        var ExplanationTransform = transform.Find("ExplanationBtn");
        explanationBtn = ExplanationTransform.GetComponent<Button>();
        explanationBtn.onClick.AddListener(() => UIManager_dungeon.Instance.ExplanationBtn());

        UpdateHp();
    }

    void OnDestroy()
    {
        //FightManager.OnInitialized -= UpdateHp;
    }

    private void UpdateHp()
    {
        int curHp = PlayerPrefs.GetInt("CurHP" + GodManager.Instance.SaveData_ID);
        int maxHp = PlayerPrefs.GetInt("MaxHP" + GodManager.Instance.SaveData_ID);
        hpTxt.text = $"HP:{curHp}/{maxHp}";
        hpImg.fillAmount = (float)curHp / maxHp;

        goldTxt.text = $"$:{PlayerPrefs.GetInt("Gold" + GodManager.Instance.SaveData_ID)}";
        professionTxt.text = $"{PlayerPrefs.GetString("profession" + GodManager.Instance.SaveData_ID)}";
    }
}
