using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;
public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    public void MasterSldOnClick(float value)//���������Ʒ���
    {
        audioMixer.SetFloat("volumeMaster",value);
        float currentVolume;
        audioMixer.GetFloat("volumeMaster", out currentVolume);
    }
    public void MusicSldOnClick(float value)//�����������Ʒ���
    {
        audioMixer.SetFloat("volumeMusic", value);
    }
    public void SoundSldOnClick(float value)//��Ч�������Ʒ���
    {
        audioMixer.SetFloat("volumeSound", value);
    }
}

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
