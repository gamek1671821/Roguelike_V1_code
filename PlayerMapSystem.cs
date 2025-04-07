using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMapSystem : MonoBehaviour
{
  public GameObject mainCamera;
  public LayerMask roomLayer; //
  private RoomGenerator roomGenerator;
  public Room_Base room_Base; //當下所在的位置
  private bool canMove = true;
  public void SetRoomGenerator(RoomGenerator roomGenerator)
  {
    this.roomGenerator = roomGenerator;
  }
  public void InitPlayerMapSystem(Vector3 pos)
  {
    mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    mainCamera.transform.position = new Vector3(transform.position.x, 130, transform.position.z);
    MovePlayer(pos);
  }
  private void MovePlayer(Vector3 pos) //被玩家操控 跟 房間生成完時
  {
    transform.position = pos; // 將玩家地圖物件放到[這個位置pos]
    mainCamera.transform.position = new Vector3(transform.position.x, 130, transform.position.z);
    Collider[] colliders = Physics.OverlapSphere(pos, 0.2f, roomLayer); //
    room_Base = colliders[0].gameObject.GetComponent<Room_Base>(); //取得[這個位置]的房間訊息
    LetGo(room_Base);
  }
  public void LetGo(Room_Base thisRoom)
  {
    if (!thisRoom.isClearRoom && thisRoom.roomId != 0) //已經通關 = false
    {
      Random.InitState(thisRoom.roomRandomSeed); //設定隨機種子
      GameObject GM = GameObject.FindGameObjectWithTag("manager");
      GM.GetComponent<GodManager>().Res = thisRoom.levelId;
      GM.GetComponent<GodManager>().playerRoom = thisRoom.roomId;
      GM.GetComponent<GodManager>().isBossRoom = thisRoom.type.Contains(Room_Base.Type.endRoom); //包含 最終房間，設定成 boss關卡
      //load場景
      SceneManager.LoadScene("battleScene");
      var scene = SceneManager.GetSceneByName("battleScene");

      //  SceneManager.sceneLoaded += (Scene sc , LoadSceneMode loadSceneMode) => 
      //  {
      //     SceneManager.SetActiveScene(scene);
      //  };
    }
    //切換場景
  }
  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.A) && canMove)
    {
      ButtonA();
      //向左移動
    }
    if (Input.GetKeyDown(KeyCode.D) && canMove)
    {
      ButtonD();
      //向右移動
    }
    if (Input.GetKeyDown(KeyCode.W) && canMove)
    {
      ButtonW();
      //向上移動
    }
    if (Input.GetKeyDown(KeyCode.S) && canMove)
    {
      ButtonS();
      //向上移動
    }
  }
  private void Start()
  {
    GameObject.Find("ButtonA").GetComponent<Button>().onClick.AddListener(() => { ButtonA(); });
    GameObject.Find("ButtonD").GetComponent<Button>().onClick.AddListener(() => { ButtonD(); });
    GameObject.Find("ButtonW").GetComponent<Button>().onClick.AddListener(() => { ButtonW(); });
    GameObject.Find("ButtonS").GetComponent<Button>().onClick.AddListener(() => { ButtonS(); });
  }
  private void ButtonA()
  {
    if (room_Base.doorLeft_ison)
    {
      MovePlayer(this.transform.position + new Vector3(-roomGenerator.x_Offset, 0, 0));
      canMove = false;
      Invoke(nameof(ResetCanMove), 0.1f);
    }
  }
  private void ButtonD()
  {
    if (room_Base.doorRight_ison)
    {
      MovePlayer(this.transform.position + new Vector3(roomGenerator.x_Offset, 0, 0));
      canMove = false;
      Invoke(nameof(ResetCanMove), 0.1f);
    }
  }
  private void ButtonW()
  {
    if (room_Base.doorUp_ison)
    {
      MovePlayer(this.transform.position + new Vector3(0, 0, roomGenerator.z_Offset));
      canMove = false;
      Invoke(nameof(ResetCanMove), 0.1f);
    }
  }
  private void ButtonS()
  {
    if (room_Base.doorDown_ison)
    {
      MovePlayer(this.transform.position + new Vector3(0, 0, -roomGenerator.z_Offset));
      canMove = false;
      Invoke(nameof(ResetCanMove), 0.1f);
    }
  }
  private void ResetCanMove()
  {
    canMove = true;
  }
}
