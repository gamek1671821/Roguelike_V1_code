using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource bgmSource;
    private void Awake()
    {
        Instance = this;
    }
    //初始化
    public void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(string name, bool isLoop = true)
    {
        //加載BGM AudioClip
        AudioClip clip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
        bgmSource.clip = clip;
        bgmSource.loop = isLoop;
        bgmSource.volume = 0f;
        bgmSource.Play();
    }
    public void PlayEffect(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
        AudioSource.PlayClipAtPoint(clip, this.transform.position , 0f); //播放
    }
}
