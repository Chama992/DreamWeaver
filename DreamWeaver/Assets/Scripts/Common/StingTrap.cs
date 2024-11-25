using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingTrap : MonoBehaviour
{
    public float damageTimer;
    private float stingTimer;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            stingTimer -= Time.deltaTime;
            if (stingTimer < 0)
            {
                //这里写碰到陷阱逻辑
                stingTimer = damageTimer;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        stingTimer = 0;
    }
}
