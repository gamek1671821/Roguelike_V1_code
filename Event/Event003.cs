using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Event003 : EventBase
{
    public override void CreatButtonOrNextStory()
    {
        ButtonSetting(Button0, "劍", 0).GetComponent<Button>().interactable = Button0Set();
        ButtonSetting(Button1, "盾", 1).GetComponent<Button>().interactable = Button1Set();
        ButtonSetting(Button2, "尖刺", 2).GetComponent<Button>().interactable = Button2Set();
        ButtonSetting(Button3, "魔法書", 3).GetComponent<Button>().interactable = Button3Set();
        ButtonSetting(Button4, "喵喵符", 4).GetComponent<Button>().interactable = Button4Set();
        ButtonSetting(Button5, "不需要", 5);

        storyBoard.GetComponentInChildren<TextMeshProUGUI>().text += $"\n持有金幣：{MyFuns.Instance.Gold()}";
    }

    public override void Button0()
    {
        if (!choseDone)
        {
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.Sword).ToString());
            RoleManager.Instance.SaveItemList();
            MyFuns.Instance.GetGold(-20);

            EndChoose();
        }
    }
    public override void Button1()
    {
        if (!choseDone)
        {
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.Shiled).ToString());
            RoleManager.Instance.SaveItemList();
            MyFuns.Instance.GetGold(-20);

            EndChoose();
        }
    }
    public override void Button2()
    {
        if (!choseDone)
        {
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.rebound).ToString());
            RoleManager.Instance.SaveItemList();
            MyFuns.Instance.GetGold(-20);

            EndChoose();
        }
    }
    public void Button3()
    {
        if (!choseDone)
        {
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.book).ToString());
            RoleManager.Instance.SaveItemList();
            MyFuns.Instance.GetGold(-20);

            EndChoose();
        }
    }
    public void Button4()
    {
        if (!choseDone)
        {
            RoleManager.Instance.roleItem.ItemList.Add(((int)ItemData.amulet).ToString());
            RoleManager.Instance.SaveItemList();
            MyFuns.Instance.GetGold(-20);

            EndChoose();
        }
    }
    public void Button5()
    {
        if (!choseDone)
        {
            EndChoose();
        }
    }
    public bool Button0Set() //設定是否可以點 
    {
        return !MyFuns.Instance.HaveItem(ItemData.Sword) && MyFuns.Instance.Gold() >= 20;
    }
    public bool Button1Set() //設定是否可以點
    {
        return !MyFuns.Instance.HaveItem(ItemData.Shiled) && MyFuns.Instance.Gold() >= 20;
    }
    public bool Button2Set() //設定是否可以點
    {
        return !MyFuns.Instance.HaveItem(ItemData.rebound) && MyFuns.Instance.Gold() >= 20;
    }
    public bool Button3Set() //設定是否可以點
    {
        return !MyFuns.Instance.HaveItem(ItemData.book) && MyFuns.Instance.Gold() >= 20;
    }
    public bool Button4Set() //設定是否可以點
    {
        return !MyFuns.Instance.HaveItem(ItemData.amulet) && MyFuns.Instance.Gold() >= 20;
    }
}
