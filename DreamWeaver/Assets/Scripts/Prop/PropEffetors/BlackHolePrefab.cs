using System;
using UnityEngine;


public class BlackHolePrefab : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.instance.PauseGame();
            GameController.instance.EndGame();
        }
    }
}
