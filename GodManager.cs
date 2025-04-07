using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodManager : MonoBehaviour
{
    public static GodManager Instance;
    public int SaveData_ID;
    public int playerRoom;
    public int Res;
    public bool battleWin;
    private RoleManager roleManager;
    public bool isBattle;
    public bool isBossRoom;
    public string profession;
    public

    void Awake()
    {
        GameObject[] SS = GameObject.FindGameObjectsWithTag("manager");
        if (SS.Length > 1)
            Destroy(this.gameObject);

        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //設定持有卡牌
        GameConfigManager.Instance.Init(); //讀取文字檔
        MyFuns.Instance.Init();

    }
    public void LoadCard()
    {
        roleManager = RoleManager.Instance;
        roleManager.LoadCardList();
        if (roleManager.roleCard == null)
        {
            roleManager.roleCard = RoleManager.CardListData.InitRoomData();
            roleManager.AddBaseCard(); //基礎卡牌
        }
        else
        {
            roleManager.LoadCardList();
        }
    }

    public void LoadItem()
    {
        roleManager = RoleManager.Instance;
        roleManager.LoadItemList();
        if (roleManager.roleItem == null)
        {
            roleManager.roleItem = RoleManager.ItemListData.InitRoomData();


            // roleManager.roleItem.ItemList.Add(((int)ItemData.Violence).ToString());
            // roleManager.roleItem.ItemList.Add(((int)ItemData.PoisonousBook).ToString());
            // roleManager.roleItem.ItemList.Add(((int)ItemData.FlashPotion).ToString());
            // roleManager.roleItem.ItemList.Add(((int)ItemData.FailPotion).ToString());
            // roleManager.SaveItemList();
        }
        else
        {
            roleManager.LoadItemList();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("chose");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerPrefs.SetInt("Level" + GodManager.Instance.SaveData_ID, 1);
        }

    }
}