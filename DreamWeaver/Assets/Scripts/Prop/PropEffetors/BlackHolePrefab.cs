using System;
using UnityEngine;


public class BlackHolePrefab : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: GameControl��һ���ӿ���Ϸ����
        }
    }
}
