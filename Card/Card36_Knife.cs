using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card36_Knife :Card00_Attack
{
    // public TextMeshProUGUI damageText;
    // public override void OnBeginDrag(PointerEventData eventData) { }
    // public override void OnDrag(PointerEventData eventData) { }
    // public override void OnEndDrag(PointerEventData eventData) { }
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     AudioManager.Instance.PlayEffect("Cards/draw");
    //     UIManager.Instance.ShowUI<LineUI>("LineUI");//顯示曲線介面
    //     UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition);

    //     damageText = UIManager.Instance.GetUI<LineUI>("LineUI").transform.Find("endPoint/DamageText").GetComponent<TextMeshProUGUI>();

    //     damageText.text = $"{CountAttack("Arg0")}傷害";

    //     Cursor.visible = false; //隱藏滑鼠
    //     StopAllCoroutines(); //關閉所有協程
    //     StartCoroutine(OnMouseDownRight(eventData));
    // }
    // IEnumerator OnMouseDownRight(PointerEventData pData)
    // {
    //     while (true)
    //     {
    //         //按下滑鼠右鍵 跳出循環
    //         if (Input.GetMouseButton(1) || !FightManager.Instance.canUseCard)
    //         {
    //             break;
    //         }
    //         Vector2 pos;
    //         if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //             transform.parent.GetComponent<RectTransform>(),
    //             pData.position,
    //             pData.pressEventCamera,
    //             out pos
    //         ))
    //         {
    //             UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
    //             //射線檢測是否碰到怪物
    //             CheckRayToEnemy();
    //         }
    //         yield return null;
    //     }
    //     Cursor.visible = true; //跳出迴圈後顯示滑鼠
    //     UIManager.Instance.CloseUI("LineUI");
    // }
    // Enemy hitEnemy; //射線檢測到的敵人腳本
    // private void CheckRayToEnemy()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
    //     {
    //         hitEnemy = hit.transform.GetComponent<Enemy>();
    //         hitEnemy.OnSelect();
    //         damageText.text = PointMessage(CountAttack("Arg0"), hitEnemy); //之後可以動態計算傷害

    //         if (Input.GetMouseButtonDown(0)) //按下左鍵 使用
    //         {
    //             StopAllCoroutines(); //關閉所有協程
    //             Cursor.visible = true; //顯示滑鼠
    //             UIManager.Instance.CloseUI("LineUI");
    //             if (TryUse() == true)
    //             {
    //                 CardEffect();
    //             }
    //             damageText.text = $"{CountAttack("Arg0")}傷害";
    //             hitEnemy.UnOnSelect();
    //             hitEnemy = null;
    //         }
    //     }
    //     else
    //     {
    //         if (hitEnemy != null)
    //         {
    //             damageText.text = $"{CountAttack("Arg0")}傷害";
    //             hitEnemy.UnOnSelect();
    //             hitEnemy = null;
    //         }
    //     }
    // }
    // public override void CardEffect()
    // {
    //     //指定一敵人。造成{0}傷害。
    //     PlayEffect(hitEnemy.transform.position);//施放特效 (無須修改)
    //     AudioManager.Instance.PlayEffect(data["sound"]);//音效 (無須修改)
    //     int val = CountAttack("Arg0"); //傷害值
    //     hitEnemy.Hit(val); //造成傷害
    //     FatalAttackdetermination(); //確認傷害是否致死
    //     CardEffectEnd();//卡片效果結束
    // }
}
