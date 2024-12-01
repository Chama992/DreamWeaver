using System;
using System.Collections;
using UnityEngine;

public class Piece_Door : Piece
{
    public static Action onOpenDoor;
    public static Action onCloseDoor;
    public static bool isInteracting;

    [SerializeField] private Transform closeTutor;
    public Transform openTutor;
    public Transform door;
    [SerializeField] private Sprite OpenSprite;
    [SerializeField] private Sprite CloseSprite;
    [HideInInspector] private SpriteRenderer doorSr;
    [HideInInspector] public Piece_Door relatedDoor;
    [HideInInspector] public bool isOpen = false;
    private bool alsoShowed;

    protected override void Start()
    {
        base.Start();
        doorSr = door.GetComponent<SpriteRenderer>();
        isInteracting = false;
        doorSr.sprite = CloseSprite;
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F) && !isInteracting && (GameController.instance.player.transform.position - door.position).magnitude < GameController.instance.interactRatio )
        {
            if (!isOpen)
            {
                if(!alsoShowed)
                {
                    alsoShowed = true;
                    FX.instance.SmoothSizeAppear(relatedDoor.openTutor.gameObject);
                }
                isInteracting = true;
                isOpen = true;
                doorSr.sprite = OpenSprite;
                relatedDoor.isOpen = true;
                relatedDoor.doorSr.sprite = OpenSprite;
                GameController.instance.player.transform.position = relatedDoor.door.position;
                GameController.instance.ConnectDoor(this);
                GameController.instance.player.CrossDoor?.Invoke(relatedDoor.transform.position,relatedDoor.door.position);
                onOpenDoor?.Invoke();
                StartCoroutine(DoorCD());
                MySoundManager.PlayOneAudio("门开");
            }
            else if (isOpen && GameController.instance.TryDisconnectDoor(this))
            {
                isInteracting = true;
                isOpen = false;
                doorSr.sprite = CloseSprite;
                relatedDoor.isOpen = false;
                relatedDoor.doorSr.sprite = CloseSprite;
                GameController.instance.player.transform.position = relatedDoor.door.position;
                GameController.instance.player.CrossDoor?.Invoke(relatedDoor.transform.position,relatedDoor.door.position);
                onCloseDoor?.Invoke();
                StartCoroutine(DoorCD());
                MySoundManager.PlayOneAudio("门关");
            }
        }
    }

    public IEnumerator DoorCD()
    {
        yield return new WaitForSeconds(.5f);
        isInteracting = false;
    }

    public override void ShowTutorial()
    {
        if (showed || tutorial == null)
            return;
        base.ShowTutorial();
        relatedDoor.showed = true;
    }
    protected override void ResetPiece()
    {
        base.ResetPiece();
        isOpen = false;
        isInteracting = false;
        doorSr.sprite = CloseSprite;
    }

}
