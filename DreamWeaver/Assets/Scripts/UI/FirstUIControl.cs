using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstUIControl : MonoBehaviour
{
    public Image Image;
    public GameObject MainMenu;
    public GameObject Hint;

    private bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(false);
        Hint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !pressed)
        {
            StartCoroutine(StartButtonDown());
        }
    }
    private IEnumerator StartButtonDown()
    {
        MySoundManager.PlayAudio("¿ªÊ¼ÓÎÏ·");
        Button[] buttons = MainMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        MainMenu.SetActive(true);
        Hint.SetActive(true);
        CanvasGroup CanvasGroup = MainMenu.GetComponent<CanvasGroup>();
        float alpha = 0;
        float speed = 1.5f;
        while (alpha < 0.99)
        {
            alpha = Mathf.MoveTowards(alpha, 1, Time.deltaTime * speed);
            CanvasGroup.alpha = alpha;
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1- alpha);
            // Debug.Log(alpha);
            yield return null;
        }
        Image.gameObject.SetActive(false);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
        pressed = true;
    }
}
