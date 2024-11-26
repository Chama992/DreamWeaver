using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UI_Out : MonoBehaviour
{
    [SerializeField] private Transform startGame;
    [SerializeField] private Transform pauseGame;
    [SerializeField] private Transform endGame;
    [SerializeField] private Transform setting;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI weavelength;
    [SerializeField] private TextMeshProUGUI score;
    void Start()
    {
        GameController.instance.onGameStart += RefreshUI_Start;
        GameController.instance.onGamePause += RefreshUI_Pause;
        GameController.instance.onGameContinue += RefreshUI_Continue;
        GameController.instance.onGameEnd += RefreshUI_End;
    }

    private void OnDisable()
    {
        GameController.instance.onGameStart -= RefreshUI_Start;
        GameController.instance.onGamePause -= RefreshUI_Pause;
        GameController.instance.onGameContinue -= RefreshUI_Continue;
        GameController.instance.onGameEnd -= RefreshUI_End;
    }

    public void RefreshUI_Start()
    {
        startGame.gameObject.SetActive(false);
    }
    public void RefreshUI_Continue()
    {
        pauseGame.gameObject.SetActive(false);
    }
    public void RefreshUI_Pause()
    {
        pauseGame.gameObject.SetActive(true);
    }
    public void RefreshUI_End()
    {
        level.text = GameController.instance.level.ToString();
        score.text = GameController.instance.score.ToString();
        weavelength.text = GameController.instance.overallWeaveLength.ToString();
        pauseGame.gameObject.SetActive(false);
        endGame.gameObject.SetActive(true);
    }
    public void OpenSetting()
    {
        setting.gameObject.SetActive(true);
    }
    public void CloseSetting()
    {
        setting.gameObject.SetActive(false);
    }
}
