using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//曲線介面
public class LineUI : UIBase
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetStartPos(Vector2 pos)
    {
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
    }
    public void SetEndPos(Vector2 pos)
    {
        transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = pos;
        Vector3 startPos = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition; //開始點
        Vector3 endPos = pos;//終點
        Vector3 midPos = Vector3.zero;//中間點
        midPos.y = (startPos.y + endPos.y) * 0.5f;
        midPos.x = startPos.x;
        Vector3 dir = (endPos - startPos).normalized;//計算開始點與終點的方向
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;//弧度轉角度
        angle -= 90;
        transform.GetChild(transform.childCount - 1).eulerAngles = new Vector3(0, 0, angle); //設置終點角度
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = GetBezier(startPos , midPos , endPos , i /(float)transform.childCount);
            if( i != transform.childCount -1 )
            {
                dir = (transform.GetChild(i+1).GetComponent<RectTransform>().anchoredPosition - transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition).normalized;
                angle =Mathf.Atan2(dir.y , dir.x) * Mathf.Rad2Deg;
                transform.GetChild(i).eulerAngles = new Vector3(0,0,angle);
            }
        }

    }
    public Vector3 GetBezier(Vector3 start, Vector3 mid, Vector3 end, float t)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * mid + t * t * end;
    }
}
