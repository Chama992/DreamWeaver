using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;//UI�õĿ�
using UnityEngine.SceneManagement;//���س���
using UnityEditor;

public class MainMenuButton : MonoBehaviour
{
    //�ӿڡ�������button��ǰ����棬˵�����棬���ý���
    // private GameObject startButton;
    // private GameObject aboutButton;
    // private GameObject settingsButton;
    // private GameObject exitButton;
    private GameObject buttonGrid;
    public string inGameScene;
    public GameObject prestoryPanel;//ǰ��
    public GameObject aboutPanel;//˵������
    public GameObject settingsPanel;//���ý���
    // Start is called before the first frame update
    void Start()
    {
        //�����Ƿ�����ť����ִ����Ӧ����
        buttonGrid = transform.Find("ButtonGrid").gameObject;
        buttonGrid.transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartGame);
        buttonGrid.transform.Find("AboutButton").GetComponent<Button>().onClick.AddListener(ShowAbout);
        buttonGrid.transform.Find("SettingButton").GetComponent<Button>().onClick.AddListener(OpenSettings);
        buttonGrid.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(ExitGame);
        prestoryPanel.transform.Find("imagesix").Find("Button to GameScene").GetComponent<Button>().onClick.AddListener(() => MyScenemanager.Instance.LoadScene(inGameScene));
        aboutPanel.transform.Find("return Button ").GetComponent<Button>().onClick.AddListener(ToggleAboutPanel);
        prestoryPanel.transform.Find("return Button ").GetComponent<Button>().onClick.AddListener(TogglePrestoryPanel);
    }
    public void StartGame() //ǰ����棬������ǰ�����ʵ�ּ�����Ϸ�����ͷ���
    {
        TogglePrestoryPanel();
    }
    public void ContinueGame() //������Ϸ
    {
        TogglePrestoryPanel();
        // GameController.Instance.LoadPlayerStat();
    }
    public void ShowAbout()//��˵������
    {
        ToggleAboutPanel();
    }

    public void OpenSettings()//�����ý���
    {
        ToggleSettingsPanel();
    }

    public void ExitGame()//�˳�
    {
        // EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void TogglePanel(GameObject Panel) //�л�Panel����״̬
    {
        bool isActive =Panel.activeSelf;
        Panel.SetActive(!isActive);
    }
    public void TogglePrestoryPanel() //prestoryPanel��Toggle��д������Ϊ���ں�����Ҫ������ҳ��ʱֱ����Onclick����������
    {
        TogglePanel(prestoryPanel);
    }

    public void ToggleAboutPanel() //aboutPanel��Toggle��д������Ϊ���ں�����Ҫ������ҳ��ʱֱ����Onclick����������
    {
        TogglePanel(aboutPanel);
    }

    public void ToggleSettingsPanel() //settingsPanel��Toggle��д������Ϊ���ں�����Ҫ������ҳ��ʱֱ����Onclick����������
    {
        TogglePanel(settingsPanel);
    }
}
