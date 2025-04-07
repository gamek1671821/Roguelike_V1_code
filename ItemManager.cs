using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public enum ItemData
{
    Sword, Shiled, rebound, book, amulet, PowerGem, DynamicShiled, sneakAttack, LuckyCat, BloodNecklace, Relics, PainArmband, ArmorPotion, Magicloak, IronMaidenArmor, LifeBarrier, AvoidOrb, Violence, PoisonousBook, FlashPotion,
    FailPotion, BeastNecklace, BloodBeastNecklace, Chili, DarkBeastNecklace, HalfChili, DragonBone, ImprisonNecklace, DarkRedNecklace, CrazyBeastNecklace

}
public class ItemManager : MonoBehaviour
{
    public GameObject currentUI; // 追蹤當前 UI 物件
    public GameObject item;
    public List<GameObject> items;
    private int destroyTime = 3;
    public void Start()
    {
        for (int i = 0; i < RoleManager.Instance.roleItem.ItemList.Count; i++)
        {
            GameObject it = Instantiate(item, this.transform);
            it.AddComponent<ItemShow>().Init(GameConfigManager.Instance.GetItemById(RoleManager.Instance.roleItem.ItemList[i]), this);
            items.Add(Instantiate(it));
        }
    }
    public void ShowItemText(PointerEventData eventData, string story, int fontSize)
    {
        if (currentUI != null)
        {
            CancelInvoke(nameof(ClearCurrentUI));
            Destroy(currentUI);
        }

        var canvesTf = GameObject.FindGameObjectWithTag("World_Canves").transform; //尋找世界畫布
        currentUI = Instantiate(Resources.Load("UI/ItemText") as GameObject, canvesTf); //Resources.Load("UI/Explanation of terms")
        TextMeshProUGUI msgText = currentUI.GetComponentInChildren<TextMeshProUGUI>();
        msgText.text = $"{story}";
        msgText.fontSize = fontSize; //設定文字敘述字體大小
        // 取得 RectTransform
        RectTransform rectTransform = currentUI.GetComponent<RectTransform>();
        // 轉換螢幕座標到 UI 坐標 (適用於 Canvas Render Mode 設為 Screen Space - Overlay)
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        rectTransform.anchoredPosition = localPoint;

        Invoke(nameof(ClearCurrentUI), destroyTime);
    }
    public void ClearCurrentUI()
    {
        Destroy(currentUI);
        currentUI = null;
    }
}
