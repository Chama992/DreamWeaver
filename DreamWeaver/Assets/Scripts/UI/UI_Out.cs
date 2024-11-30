using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Out : MonoBehaviour
{
    [SerializeField] private Transform inUI;
    [SerializeField] private Transform inGameBG;
    [SerializeField] private Transform startGame;
    [SerializeField] private Transform pauseGame;
    [SerializeField] private Transform endGame;
    [SerializeField] private Transform info;
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
        StartCoroutine(this.FadeInOut());
        // inUI.gameObject.SetActive(true);
        // inGameBG.gameObject.SetActive(true);
        // startGame.gameObject.SetActive(false);
    }
    /// <summary>
    /// 用于游戏开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInOut()
    {
        CanvasGroup canvasGroup  =  startGame.gameObject.GetComponent<CanvasGroup>();
        CanvasGroup inCanvasGroup =  inUI.gameObject.GetComponent<CanvasGroup>();
        Image image = inGameBG.GetComponentInChildren<Image>();
        float alpha = 1;
        float speed = 1.5f;
        inGameBG.gameObject.SetActive(true);
        inUI.gameObject.SetActive(true);
        while (alpha > 0.1)
        {
            alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime * speed);
            canvasGroup.alpha = alpha;
            inCanvasGroup.alpha = 1-alpha;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1-alpha);
            yield return null;
        }
        startGame.gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        inCanvasGroup.alpha = 1;
        canvasGroup.alpha = 1;
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

    public void OpenInfo()
    {
        info.gameObject.SetActive(true);
    }
    public void CloseInfo()
    {
        info.gameObject.SetActive(false);
    }
}
