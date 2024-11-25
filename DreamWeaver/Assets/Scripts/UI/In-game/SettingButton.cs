using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    private GameObject settingsPanel;
    void Start()//�Ҽ����˵�cavas���������б������������Ӷ��ҵ�Setting_Panel
    {
        Transform canvasTransform = GameObject.Find("Canvas(SettingPanel)").transform;
        settingsPanel = canvasTransform.Find("SettingPanel")?.gameObject;
    }  
    public void TogglePanel() //�л�Panel����״̬
    {    
        
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
        else 
        {
            Debug.Log("δ�ҵ���һ�������µ�Setting_Panel�����ȴ�Startmenu������Ϸ��Setting_Panelû�õ���ģʽд......."); 
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
