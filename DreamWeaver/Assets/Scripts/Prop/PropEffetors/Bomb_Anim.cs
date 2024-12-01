using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Anim : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private bool isFlashing;
    private float bombTimer;
    private float flashTimer;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bombTimer = PropEffectorManager.instance.bombWaitTime;
        flashTimer = .1f;
    }

    private void Update()
    {
        bombTimer -= Time.deltaTime;
        if(isFlashing&&bombTimer>0)
        {
            flashTimer -= Time.deltaTime;
            if(flashTimer<0)
            {
                flashTimer = .1f;
                if (sr.color != Color.yellow)
                {
                    sr.color = Color.yellow;
                }
                else
                {
                    sr.color = Color.white;
                }
            }
        }
        if(bombTimer<0)
        {
            sr.color = Color.white;
            anim.SetBool("bombed", true);
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Flash()
    {
        isFlashing = true;
        anim.speed = 0;
    }
}
