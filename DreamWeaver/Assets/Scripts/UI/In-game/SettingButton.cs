using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    private GameObject settingsPanel;
    void Start()//找激活了的cavas，在它的列表里遍历子物体从而找到Setting_Panel
    {
        Transform canvasTransform = GameObject.Find("Canvas(SettingPanel)").transform;
        settingsPanel = canvasTransform.Find("SettingPanel")?.gameObject;
    }  
    public void TogglePanel() //切换Panel激活状态
    {    
        
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
        else 
        {
            Debug.Log("未找到上一场景留下的Setting_Panel，请先从Startmenu进入游戏。Setting_Panel没用单例模式写......."); 
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
