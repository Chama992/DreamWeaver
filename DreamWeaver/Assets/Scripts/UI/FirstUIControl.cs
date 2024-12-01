using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstUIControl : MonoBehaviour
{
    public GameObject MainMenu;
    [SerializeField] private Color refreshColor;
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
            FX.instance.SmoothRefresh(refreshColor,1.5f,CallBack);
        }
    }

    private void CallBack()
    {
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
