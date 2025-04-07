using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Video;

namespace AStar
{
    public class AStar
    {
        
        private static float x_Offset;
        private static float z_Offset;
        private static LayerMask roomLayer;
        /// <summary>
        /// 需要處理的節點
        /// </summary>
        private static List<Node> processingNodes = new List<Node>();
        /// <summary>
        /// 已經走過的房間
        /// </summary>
        private static HashSet<GameObject> walkedGameObject = new HashSet<GameObject>();

        /// <summary>
        ///  初始化判斷房間存在相關的變數
        /// </summary>
        /// <param name="x_Offset"></param>
        /// <param name="z_Offset"></param>
        /// <param name="roomLayer"></param>
        public static void Init(float x_Offset, float z_Offset, LayerMask roomLayer)
        {
            AStar.x_Offset = x_Offset;
            AStar.z_Offset = z_Offset;
            AStar.roomLayer = roomLayer;
        }
        /// <summary>
        /// 傳入位置 判斷是否有房間存在
        /// 且可以傳入是否走過來標記此房間是否走過
        /// </summary>
        /// <param name="position"></param>
        /// <param name="go"></param>
        /// <param name="isCheckWalked"></param>
        /// <returns></returns>
        private static bool TryGetGameObjectAt(Vector3 position, out GameObject go, bool isCheckWalked = true)
        {

            go = null; //要存放碰到的roomLayer 物件
            var obj = Physics.OverlapSphere(position, 0.2f, roomLayer); // 傳入的 position 位置是否有 roomLayer 物件
            if (obj != null && obj.Length > 0)  // 判斷該位置是否有 roomLayer 物件
            {
                go = obj[0].gameObject; // 將碰到的 roomLayer 物件放到 變數中
                if (!isCheckWalked) // 
                {
                    return true;
                }
                else
                {
                    if (walkedGameObject.Contains(go))
                    { // HashSet 或 List 中是否含 
                        return false;
                    }
                    else
                    {
                        walkedGameObject.Add(go);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// A*演算法
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public static List<GameObject> AStarAlg(Vector3 startPosition, Vector3 endPosition)
        {
            // 清除cache
            processingNodes.Clear();
            walkedGameObject.Clear();
            
            TryGetGameObjectAt(startPosition, out GameObject startRoom, false);
            TryGetGameObjectAt(endPosition, out GameObject endRoom, false);
            if (startRoom == null || endRoom == null) // 防呆 (正常不會沒有初始與結束房間)
            {
                Debug.LogError("起始房間 結束房間為空 請確認是否有房間存在");
                return new List<GameObject>();
            }

            // 起始節點
            Node node = new Node(); // 建立起始節點
            node.walkPath = new List<GameObject>(); // 起始節點 的 走過的路徑 = 一個新的List 
            node.walkPath.Add(startRoom); // 路徑List 加上 起始房間
            node.walkedLen = 0; // 起始節點 走過的步數 = 0
            node.disToTarget = CalculateDis(startPosition, endPosition); // 計算與終點的距離 
            node.currentGameObject = startRoom; //目前的room 是 起始房間

            // A*演算法
            var result = AStarExec(startRoom, endRoom, node); // 開始遞迴

            // 清除cache
            processingNodes.Clear();
            walkedGameObject.Clear();
            return result;
        }
        
        /// <summary>
        /// A*演算法核心
        /// </summary>
        /// <param name="startRoom"></param>
        /// <param name="endRoom"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static List<GameObject> AStarExec(GameObject startRoom, GameObject endRoom, Node node)
        {
            // 若走道終點就結束
            if (node.currentGameObject == endRoom) //如果當前的 room 是 endRoom
            {
                return node.walkPath; //將路徑回傳
            }
            else
            {
                // 上下左右 依序產出節點
                // up 
                if (TryGetGameObjectAt(node.currentGameObject.transform.position + new Vector3(0, 0, z_Offset), out GameObject upRoom))
                {//當前room的位置"上方"有房間
                    if (!node.walkPath.Contains(upRoom))
                    { //如果走過的路徑中沒有"上方的房間"
                        //新增一個節點
                        Node upNode = Node.GenerateNode(node, upRoom, CalculateDis(node.currentGameObject.transform.position, upRoom.transform.position), CalculateDis(upRoom.transform.position, endRoom.transform.position));
                        //等待處理的節點新增這個新節點
                        processingNodes.Add(upNode);
                    }
                }
                // dowm
                if (TryGetGameObjectAt(node.currentGameObject.transform.position + new Vector3(0, 0, -z_Offset), out GameObject downRoom))
                {
                    if (!node.walkPath.Contains(downRoom))
                    {
                        Node downNode = Node.GenerateNode(node, downRoom, CalculateDis(node.currentGameObject.transform.position, downRoom.transform.position), CalculateDis(downRoom.transform.position, endRoom.transform.position));
                        processingNodes.Add(downNode);
                    }
                }
                // left
                if (TryGetGameObjectAt(node.currentGameObject.transform.position + new Vector3(-x_Offset, 0, 0), out GameObject leftRoom))
                {
                    if (!node.walkPath.Contains(leftRoom))
                    {
                        Node leftNode = Node.GenerateNode(node, leftRoom, CalculateDis(node.currentGameObject.transform.position, leftRoom.transform.position), CalculateDis(leftRoom.transform.position, endRoom.transform.position));
                        processingNodes.Add(leftNode);
                    }
                }
                // right
                if (TryGetGameObjectAt(node.currentGameObject.transform.position + new Vector3(x_Offset, 0, 0), out GameObject rightRoom))
                {
                    if (!node.walkPath.Contains(rightRoom))
                    {
                        Node rightNode = Node.GenerateNode(node, rightRoom, CalculateDis(node.currentGameObject.transform.position, rightRoom.transform.position), CalculateDis(rightRoom.transform.position, endRoom.transform.position));
                        processingNodes.Add(rightNode);
                    }
                }

                // 選擇最佳節點
                Node bestNode = SelectBestNode();
                if(bestNode == null) //防呆
                {
                    Debug.LogError("看到此log 請檢查是不是有沒通路");
                }
                // 移出已經處理過的節點
                processingNodes.Remove(bestNode);
                
                return AStarExec(startRoom, endRoom, bestNode);
            }
        }

        /// <summary>
        /// 選擇出最佳節點
        /// 越晚產出的點越早被選擇 所以要從後面開始找
        /// </summary>
        /// <returns></returns>
        private static Node SelectBestNode()
        {
            Node bestNode = null;
            float minDis = float.MaxValue;
            for(int i = processingNodes.Count -1 ; i >= 0 ; i--) //從後方反向迴圈
            {
                var node = processingNodes[i]; //取出需要處理的節點
                var dis = node.disToTarget + node.walkedLen; // 計算這個節點的 距離權重
                if (dis < minDis) //長度 比 minDis 短 >> 距終點更近
                {// 改變最佳點
                    minDis = dis; 
                    bestNode = node;
                }
            }
            //完全判斷完，回傳篩選出的最佳點
            return bestNode;
        }

/// <summary>
/// 計算兩點的距離
/// </summary>
/// <param name="from">起點</param>
/// <param name="to">終點</param>
/// <returns></returns>
        public static float CalculateDis(Vector3 from, Vector3 to)
        {
            var x = to.x - from.x;
            var z = to.z - from.z;
            x = x < 0 ? -x : x; // if (X < 0) x = -x else x = x
            z = z < 0 ? -z : z;
            return x + z;
        }
    }

/// <summary>
/// 節點
/// </summary>
    public class Node
    {
        /// <summary>
        /// 目前這個節點上的Room
        /// </summary>
        public GameObject currentGameObject;
        /// <summary>
        /// 走過的路徑(路過的房間) 
        /// </summary>
        public List<GameObject> walkPath = new List<GameObject>();
        /// <summary>
        /// 已經走的路
        /// </summary>
        public float walkedLen; 
        /// <summary>
        /// 預測要走的路
        /// </summary>
        public float disToTarget; 
/// <summary>
/// 生成新節點 
/// </summary>
/// <param name="node">(舊)節點</param>
/// <param name="go">上、下、左、右的room物件</param>
/// <param name="walkLen">舊節點的步數 + 新舊room物件的距離(1)</param>
/// <param name="disToTarget">與終點的距離</param>
/// <returns></returns>
/// 起始點不使用此生成
        public static Node GenerateNode(Node node, GameObject go, float walkLen, float disToTarget)
        {
            Node newNode = new Node();
            newNode.currentGameObject = go;
            newNode.walkPath = new List<GameObject>(node.walkPath); //從舊節點複製路徑list 
            newNode.walkPath.Add(go); // 經過的房間增加room物件
            newNode.walkedLen = node.walkedLen + walkLen;
            newNode.disToTarget = disToTarget;
            
            return newNode;
        }

        public string NodeInfo()
        {
            return "currentGameObject : " + currentGameObject + " walkedLen : " + walkedLen + " disToTarget : " + disToTarget + " pathCount :" + walkPath.Count;
        }
    }
}