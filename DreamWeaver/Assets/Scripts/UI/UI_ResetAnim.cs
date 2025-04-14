using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResetAnim : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        anim.SetBool("isStart", true);
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
