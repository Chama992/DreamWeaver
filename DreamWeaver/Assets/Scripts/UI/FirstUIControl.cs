using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstUIControl : MonoBehaviour
{
    public GameObject MainMenu;
    [SerializeField] private Color refreshColor;
    private bool started;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown&&!started)
        {
            started = true;
            FX.instance.SmoothRefresh(refreshColor,CallBack);
        }
    }

    private void CallBack()
    {
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
