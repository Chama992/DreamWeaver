using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstUIControl : MonoBehaviour
{
    public GameObject MainMenu;
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
            gameObject.SetActive(false);
            MainMenu.SetActive(true);
        }
    }
}
