using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyScenemanager : SingleTon<MyScenemanager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); 
    }

    public void LoadScene(string sceneName) 
    {
        // SceneManager.LoadScene(sceneName);
        GameObject loadingScreen = Resources.Load<GameObject>("prefab/UI/SceneLoader");
        LoadSceneAsync(sceneName, loadingScreen);
    }

    private void LoadSceneAsync(string sceneName,GameObject loadingScreen)
    {
        GameObject sceneLoadUi = Instantiate(loadingScreen);
        sceneLoadUi.transform.Find("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Background/{Random.Range(1,6)}");
        StartCoroutine(Load(sceneLoadUi,sceneName));
    }

    private IEnumerator Load(GameObject loadingScreen,string sceneName)
    {
        Slider progress= loadingScreen.GetComponentInChildren<Slider>();
        TMP_Text progressText = progress.GetComponentInChildren<TMP_Text>();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation  = false;
        float fakeProgress = 0f;
        float loadTime = Random.Range(1f,1.5f);
        while (!asyncOperation.isDone)
        {
            float prog = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            fakeProgress += Time.deltaTime;
            fakeProgress = Mathf.Clamp(fakeProgress, 0f, loadTime);
            progress.value = fakeProgress/loadTime;
            progressText.text = (fakeProgress/loadTime).ToString("p2");
            if (prog >= 1f && fakeProgress>= loadTime)
            {
                break;
            }
            yield return null;
        }
        asyncOperation.allowSceneActivation  = true;
    }
}
