using System;
using UnityEngine;


public class BlackHolePrefab : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: GameControl给一个接口游戏结束
        }
    }
}
