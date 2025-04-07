using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniInfo : MonoBehaviour
{
     [System.Serializable]
    public struct Info
    {
        public string aniName;
        public float triggerTime;
    }
    public List<Info> infos;
}
