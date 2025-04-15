using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Piece currentPiece;
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public CapsuleCollider2D Cc2 { get; private set; }
    public DistanceJoint2D DistanceJoint2D { get; private set; }
    public LineRenderer LineRenderer { get; private set; }
    [Header("Node Info")]
    public PlayerNodeControl PlayerNodeControl;
    #endregion
    public PlayerEntityController PlayerEntityController { get; private set; }
    [Header("CollisionCheck Info")]
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    #region Props
    public PlayerProps Props { get; private set; } = new PlayerProps();
    private List<KeyCode> propKeys = new List<KeyCode>() { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    #endregion

    public bool canGrap = true;
    public bool canBuild = true;
    public Action<Vector2,Vector2> CrossDoor;
    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Cc2 = GetComponent<CapsuleCollider2D>();
        DistanceJoint2D = GetComponent<DistanceJoint2D>();
        DistanceJoint2D.enabled = false;
        LineRenderer = GetComponent<LineRenderer>();
        PlayerEntityController = GetComponent<PlayerEntityController>();
        canGrap = true;
        Props.Initialize(this);
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (GameController.instance.isResetAnimating||GameController.instance.isReadyAnimating||!GameController.instance.isGaming || GameController.instance.isPausing)
        {
            Rb.velocity = Vector2.zero;
            return;
        }
        UsePropDetect();
        LinkNodeChekc();
    }
    private void LinkNodeChekc()
    {
        RaycastHit2D hit = IsPieceChecked();
        if (hit)
        {
            currentPiece = hit.transform.gameObject.GetComponentInParent<Piece>();
            currentPiece.ShowTutorial();
        }
        if (currentPiece != null)
        {
            if(!GameController.instance.isPausing&&currentPiece.node != null&&Input.GetKeyDown(KeyCode.S)&&(transform.position-currentPiece.node.position).magnitude<GameController.instance.interactRatio)
            {
                PlayerNodeControl.LinkNode(currentPiece.gameObject.GetInstanceID(), currentPiece);
            }
        }
    }
    public RaycastHit2D IsPieceChecked() => Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, whatIsGround);
    #region Prop
    private void UsePropDetect()
    {
        if (!GameController.instance.isGaming && GameController.instance.isReadyAnimating &&GameController.instance.isResetAnimating && GameController.instance.isPausing)
        {
            return;
        }
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < propKeys.Count; i++)
            {
                if (Input.GetKeyDown(propKeys[i]))
                {
                    Props.UsePropByIndex(CheckPropToUse(propKeys[i]));
                    break;
                }
            }
        }
    }

    private int CheckPropToUse(KeyCode _key)
    {
        string key = _key.ToString().Substring(_key.ToString().Length - 1, 1);
        int keyInt = int.Parse(key);
        return keyInt;
        // int propIndex = int.TryParse(_key.ToString());
    }

    #endregion

}
