using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    // public GameObject somethingDontDestroyOnload;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // ���ָ�GameObject��������

    }

    private void Start()
    {
        Transform setPanel = transform.Find("SettingPanel");
        setPanel.transform.Find("return Button ").GetComponent<Button>().onClick.AddListener(ToggleSettingsPanel);
        Slider masterSlider = setPanel.transform.Find("Slider for Master").GetComponent<Slider>();
        Slider musciSlider = setPanel.transform.Find("Slider for music").GetComponent<Slider>();
        Slider soundSlider = setPanel.transform.Find("Slider for sound").GetComponent<Slider>();
        Slider brightnessSlider = setPanel.transform.Find("Slider for brightness").GetComponent<Slider>();
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.MusicSldOnClick);
        musciSlider.onValueChanged.AddListener(AudioManager.Instance.MusicSldOnClick);
        soundSlider.onValueChanged.AddListener(AudioManager.Instance.SoundSldOnClick);
        brightnessSlider.onValueChanged.AddListener(BrightnessManager.Instance.TransparencySldOnClick);
    }

    public void ToggleSettingsPanel() //settingsPanel��Toggle��д������Ϊ���ں�����Ҫ������ҳ��ʱֱ����Onclick����������
    {
        TogglePanel(GameObject.Find("SettingPanel"));
    }
    void TogglePanel(GameObject Panel) //�л�Panel����״̬
    {
        bool isActive =Panel.activeSelf;
        Panel.SetActive(!isActive);
    }
    
}