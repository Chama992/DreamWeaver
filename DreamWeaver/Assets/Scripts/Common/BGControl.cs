using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
    
    private void OnEnable()
    {
        Canvas BGCanvas = GetComponent<Canvas>();
        BGCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        BGCanvas.worldCamera = Camera.main;
    }
}
