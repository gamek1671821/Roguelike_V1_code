using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event002 : EventBase
{
    public override void CreatButtonOrNextStory()
    {
        ButtonSetting(Button0, "隨機摧毀卡組一張卡", 0).GetComponent<Button>().interactable = Button0Set();

        ButtonSetting(Button1, "隨機將一張卡加入牌組", 1);

        ButtonSetting(Button2, "隨機複製卡組一張卡(不含基礎卡)", 2);

        ButtonSetting(Button3, "不需要", 3);
    }

    public override void Button0()
    {
        if (!choseDone)
        {
            if (RoleManager.Instance.roleCard.cardList.Count > 1)
            {//必須大於1張牌才能移除
                int indexToRemove = Random.Range(0, RoleManager.Instance.roleCard.cardList.Count); // 隨機選擇 要移除索引  的值; // 要移除索引  的值
                string removeCard = RoleManager.Instance.roleCard.cardList[indexToRemove]; //提前取得要移除的卡片
                var data = GameConfigManager.Instance.GetCardById(removeCard);//取得卡片資訊
                string txt = $"{data["Name"]} 被移除";

                RoleManager.Instance.roleCard.cardList.RemoveAt(indexToRemove);
                RoleManager.Instance.SaveCardList();

                EndBordShow(txt);
            }
        }
    }
    public override void Button1()
    {
        if (!choseDone)
        {
            int cardID = MyFuns.Instance.pickOneCard("all"); //選取一張卡，
            var data = GameConfigManager.Instance.GetCardById(cardID.ToString());//取得卡片資訊
            string txt = $"取得{data["Name"]} 效果：{string.Format(data["Des"], data["Arg0"], data["Arg1"], data["Arg2"])}";

            RoleManager.Instance.roleCard.cardList.Add(cardID.ToString());
            RoleManager.Instance.SaveCardList();

            EndBordShow(txt);
        }
    }
    public override void Button2()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        if (!choseDone)
        {
            int indexToCopy;
            do
            {
                indexToCopy = Random.Range(0, RoleManager.Instance.roleCard.cardList.Count); // 要複製的索引 的值

                string a = RoleManager.Instance.roleCard.cardList[indexToCopy];
                data = MyFuns.Instance.ChoseOneCard(a);

            } while (data["Rarity"] == "base"); //如果是基礎卡 重新迴圈
            if (data["Rarity"] == "base")
            {
                Debug.Log("迴圈條件被觸發！");
            }
            else
            {
                Debug.Log("條件不成立，迴圈結束。");
            }

            indexToCopy = Random.Range(0, RoleManager.Instance.roleCard.cardList.Count); // 要複製的索引 的值


            string txt = $"取得{data["Name"]} 效果：{string.Format(data["Des"], data["Arg0"], data["Arg1"], data["Arg2"])}";

            RoleManager.Instance.roleCard.cardList.Add(RoleManager.Instance.roleCard.cardList[indexToCopy]);
            RoleManager.Instance.SaveCardList();

            EndBordShow(txt);
        }
    }
    public void Button3()
    {
        if (!choseDone)
        {
            EndChoose();
        }
    }
    public bool Button0Set() //設定是否可以點
    {
        return RoleManager.Instance.roleCard.cardList.Count > 1;
    }
}
