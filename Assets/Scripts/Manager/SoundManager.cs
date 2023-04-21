using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //全部的声音资源都声明为一个静态变量
    private const string Sounds_Path_Prefix = "Sounds/";
    private const string bg = "Floating_light";

    //音乐组件
    private AudioSource bgAudioSource;//用于控制背景音乐

    /// <summary>
    /// 构造方法，将参数扔给父类使用
    /// </summary>
    /// <param name="facade"></param>
    public SoundManager() { }



    /// <summary>
    /// 根据资源路径加载声音资源
    /// </summary>
    private AudioClip LoadSound(string soundPath)
    {
        return Resources.Load<AudioClip>(Sounds_Path_Prefix + soundPath);
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="audioSource"></param>音频源组件，其作用就是用于播放音频剪辑（AudioClip）资源。
    /// <param name="audioClip"></param>音乐
    /// <param name="volume"></param>声音大小
    /// <param name="loop"></param>是否循环
    private void PlaySound(AudioSource audioSource, AudioClip audioClip, float volume, bool loop = false)
    {

        audioSource.clip = audioClip;//挂载声音
        audioSource.volume = volume;//设置音量
        audioSource.loop = loop;//循环播放
        audioSource.Play();//播放

    }


    private void Start()
    {
        //创建一个游戏物体用于播放声音
        GameObject audioSourceGameObject = new GameObject("AudioSource(GameObject)");
        //在这个物体下面挂载两个audiosource音频源组件，其作用就是用于播放音频剪辑（AudioClip）资源。
        bgAudioSource = audioSourceGameObject.AddComponent<AudioSource>();
        //加载默认的声音(背景)
        PlaySound(bgAudioSource, LoadSound(bg), 0.1f, true);
    }
}
