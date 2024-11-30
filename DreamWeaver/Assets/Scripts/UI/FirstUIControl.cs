using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstUIControl : MonoBehaviour
{
    public GameObject MainMenu;
    public List<Button> Buttons;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            
            StartCoroutine(ButtonDown());
        }
    }
    private IEnumerator ButtonDown()
    {
        List<Image> images = new List<Image>();
        for (int i = 0; i < Buttons.Count; i++)
        {
            images.Add(Buttons[i].gameObject.GetComponent<Image>());
            Buttons[i].interactable = false;
        }
        float alpha = 0;
        float speed = 1.5f;
        while (alpha < 0.99)
        {
            alpha = Mathf.MoveTowards(alpha, 1, Time.deltaTime * speed);
            foreach (Image image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
            yield return null;
        }
        for (int i = 0; i < Buttons.Count; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1);
            Buttons[i].interactable = true;
        }
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
