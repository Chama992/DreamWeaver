using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UI_Out : MonoBehaviour
{
    [SerializeField] private Transform inUI;
    [SerializeField] private Transform inGameBG;
    [SerializeField] private Transform startGame;
    [SerializeField] private Transform pauseGame;
    [SerializeField] private Transform endGame;
    [SerializeField] private Transform setting;
    [SerializeField] private TextMeshProUGUI hint;
    [SerializeField] private TextMeshProUGUI hintLevel;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI weavelength;
    [SerializeField] private TextMeshProUGUI score;
    void Start()
    {
        GameController.instance.onGameStart += RefreshUI_Start;
        GameController.instance.onGamePause += RefreshUI_Pause;
        GameController.instance.onGameContinue += RefreshUI_Continue;
        GameController.instance.onGameEnd += RefreshUI_End;
        GameController.instance.onLevelReady += RefreshHint;
        GameController.instance.onLevelStart += CloseHint;
    }
    private void OnDisable()
    {
        GameController.instance.onGameStart -= RefreshUI_Start;
        GameController.instance.onGamePause -= RefreshUI_Pause;
        GameController.instance.onGameContinue -= RefreshUI_Continue;
        GameController.instance.onGameEnd -= RefreshUI_End;
        GameController.instance.onLevelReady -= RefreshHint;
        GameController.instance.onLevelStart -= CloseHint;
    }

    public void RefreshUI_Start()
    {
        inUI.gameObject.SetActive(true);
        inGameBG.gameObject.SetActive(true);
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
        inUI.gameObject.SetActive(false);
        level.text = GameController.instance.level.ToString();
        score.text = GameController.instance.score.ToString();
        weavelength.text = GameController.instance.overallWeaveLength.ToString();
        pauseGame.gameObject.SetActive(false);
        endGame.gameObject.SetActive(true);
    }
    public void RefreshHint()
    {
        FX.instance.SmoothColorAppear<TextMeshProUGUI>(hint);
        FX.instance.SmoothColorAppear<TextMeshProUGUI>(hintLevel);
        hintLevel.text = GameController.instance.level.ToString();
    }

    public void CloseHint()
    {
        FX.instance.SmoothColorDisappear<TextMeshProUGUI>(hint);
        FX.instance.SmoothColorDisappear<TextMeshProUGUI>(hintLevel);

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
