using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : SingleTon<GameController>
{
    public GameObject playerDeadCanvas;
    public GameObject playerWinCanvas;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public void OnPlayerDeath()
    {
        FindObjectOfType<Player>().enabled = false; 
        GameObject deadUI = Instantiate(playerDeadCanvas);
        deadUI.SetActive(true);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.StartCoroutine(PlayerDeadUI(deadUI));//��UI
    }
    public IEnumerator PlayerDeadUI(GameObject deadUI)
    {
        float alphaValue = 0;
        Button backButton = deadUI.transform.GetChild(1).GetComponent<Button>();
        backButton.onClick.AddListener(BackToMainMenu);
        backButton.interactable = false;
        Button continueButton =deadUI.transform.GetChild(2).GetComponent<Button>();
        continueButton.onClick.AddListener(OnTryAgain);
        continueButton.interactable = false;
        CanvasGroup canvasGroup =  deadUI.GetComponent<CanvasGroup>();
        while (alphaValue < 0.99f)
        {
            alphaValue= Mathf.Lerp(alphaValue,1,Time.deltaTime*2f);
            canvasGroup.alpha = alphaValue;
            Debug.Log(alphaValue);
            yield return null;
        }
        backButton.interactable = true;
        continueButton.interactable = true;
    }
    public void OnPlayerWin()
    {
        GameObject winUI = Instantiate(playerWinCanvas);
        winUI.SetActive(true);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.StartCoroutine(PlayerwinUI(winUI));//��UI
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/save.json"))
            {
                streamWriter.Write("");
            }
        }
    }
    public IEnumerator PlayerwinUI(GameObject winUI)
    {
        float alphaValue = 0;
        Button backButton = winUI.transform.GetChild(1).GetComponent<Button>();
        backButton.onClick.AddListener(BackToMainMenu);
        backButton.interactable = false;
        CanvasGroup canvasGroup =  winUI.GetComponent<CanvasGroup>();
        while (alphaValue < 0.99f)
        {
            alphaValue= Mathf.Lerp(alphaValue,1,Time.deltaTime * 2f);
            canvasGroup.alpha = alphaValue;
            yield return null;
        }
        backButton.interactable = true;
        FindObjectOfType<Player>().enabled = false;//���� 
    }
    public void OnTryAgain()
    {
        MyScenemanager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}


