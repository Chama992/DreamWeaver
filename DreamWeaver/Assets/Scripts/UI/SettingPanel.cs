using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    // public GameObject somethingDontDestroyOnload;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 保持该GameObject不被销毁

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

    public void ToggleSettingsPanel() //settingsPanel的Toggle。写出来是为了在后面需要返回主页面时直接在Onclick里调这个函数
    {
        TogglePanel(GameObject.Find("SettingPanel"));
    }
    void TogglePanel(GameObject Panel) //切换Panel激活状态
    {
        bool isActive =Panel.activeSelf;
        Panel.SetActive(!isActive);
    }
    
}