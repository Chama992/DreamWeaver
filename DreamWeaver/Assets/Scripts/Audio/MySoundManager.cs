using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//做了个静态类，原理是Resources.Load加载音频
//并且动态创建GO添加AudioSouce组件然后播放
//【重要】记得AUDIO_PATH设置音频路径

/*笔记fromY
 * 静态音效管理器
 * 作用：在游戏中动态加载，播放sound
 * 用法：
 * 1.配置路径：将待使用的所有音频文件放在Resources文件夹中，并将这个路径赋给AUDIO_PATH；
 * 2.在其他事件中调用三种方法之一。如SoundManager.PlayOneAudio("某音效")
 * 
 * 添加了AudioMixer来控制音效的输出
 */
public  class MySoundManager : SingleTon<MySoundManager>
{
    public const string AUDIO_PATH = "Audio/Sound/";//音效路径
    private static GameObject oneShotObj;
    private static AudioSource oneShotAudioSource;
    private static GameObject defalutSoundObj_1;//没想好干啥,也许用来做BGM？可BGM直接在unity里面设置不就好了(没错我就是懒
    private static Dictionary<string,float> SfxName_PlayTime;//记录某sfx的上一次播放时间
    private static Dictionary<string,AudioClip> audios;//存储sfx资源
    //添加的AudioMixer
    public AudioMixerGroup audioMixerGroup;
    //播放音效直接调用这个函数就行
    //它不关心能否同一时间大量播放,即你每一帧调用一次的话它每一帧播放一次，它不关上一个同样的音效放没放完
    public static void PlayAudio(string _sfxName)
    {
        if(oneShotObj == null)
        {
            oneShotObj = new GameObject(AUDIO_PATH + "OneShotSound");
            oneShotAudioSource = oneShotObj.AddComponent<AudioSource>();
        }
        oneShotAudioSource.pitch = Random.Range(0.8f, 1.2f);
        oneShotAudioSource.PlayOneShot(GetAudio(_sfxName));
    }

    //播放音效直接调用这个函数就行
    //它关心能否同一时间大量播放
    public static void PlayOneAudio(string _sfxName)
    {
        if(SfxName_PlayTime == null)
        {
            SfxName_PlayTime = new Dictionary<string, float>();
        }

        if(!SfxName_PlayTime.ContainsKey(_sfxName))
        {
            SfxName_PlayTime.Add(_sfxName,0f);
        }
        float curTime = Time.time;
        AudioClip clip = GetAudio(_sfxName);
        //如果播放完
        if(curTime > SfxName_PlayTime[_sfxName] + clip.length)
        {
            if(oneShotObj == null)
            {
                oneShotObj = new GameObject(AUDIO_PATH + "OneShotSound");
                oneShotAudioSource = oneShotObj.AddComponent<AudioSource>();
            }
            oneShotAudioSource.pitch = Random.Range(0.8f, 1.2f);
            oneShotAudioSource.PlayOneShot(GetAudio(_sfxName));
            SfxName_PlayTime[_sfxName] = curTime;
        }
    }
    
    //获取audio clip
    private static AudioClip GetAudio(string _sfxName)
    {
        if(audios == null)
        {
            audios = new Dictionary<string, AudioClip>();
        }
    
        if(!audios.ContainsKey(_sfxName))
        {
            AudioClip clip = Resources.Load<AudioClip>(AUDIO_PATH + _sfxName);
            audios.Add(_sfxName,clip);
        }
        return audios[_sfxName];
    }
}
