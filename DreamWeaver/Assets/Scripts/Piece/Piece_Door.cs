using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Door : Piece
{
    public static Action onOpenDoor;
    public static Action onCloseDoor;

    [SerializeField] private Transform door;
    [HideInInspector] public Vector3 doorPosition;
    [HideInInspector] public Piece_Door relatedDoor;
    [HideInInspector] public bool isOpen;

    private void Start()
    {
        doorPosition = door.position;
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!isOpen)
            {
                isOpen = true;
                relatedDoor.isOpen = true;
                GameController.instance.player.transform.position = relatedDoor.doorPosition;
                GameController.instance.ConnectDoor(this);
                onOpenDoor?.Invoke();
            }
            else
            {
                isOpen = false;
                relatedDoor.isOpen = false;
                GameController.instance.player.transform.position = relatedDoor.doorPosition;
                GameController.instance.TryDisconnectDoor(this);
                onCloseDoor?.Invoke();
            }    
        }
    }

}
