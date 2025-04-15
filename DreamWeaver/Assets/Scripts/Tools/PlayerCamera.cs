using System;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject target;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, playerCamera.transform.position.z);
    }
}
