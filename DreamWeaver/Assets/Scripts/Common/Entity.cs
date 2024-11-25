using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ʵ�����װ��ͨ���ڵ��˺���ҵĹ�ͬ����
/// </summary>
public class Entity : MonoBehaviour
{
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    [Header("CollisionCheck Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    #region FacingDir
    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;
    #endregion

    public bool IsBusy { get; private set; }
    protected virtual void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }
    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    #region Collision
    /// <summary>
    /// ����ɫ�Ƿ��ڵ�����
    /// </summary>
    /// <returns></returns>
    public virtual bool IsGroundChecked() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    /// <summary>
    /// ����ɫ��ǰ�����Ƿ���ǽ
    /// </summary>
    /// <returns></returns>
    public virtual bool IsWallChecked() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    /// <summary>
    /// �ڳ��������л���һЩͼ��ͼ�귽��debug
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        //��groundcheck����������µķ��򴴽�һ����Ϊ��������������
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        //��wallCheck����������ɫ�泯�ķ��򴴽�һ����Ϊǽ������������
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
    #endregion
    #region Flip
    /// <summary>
    /// ��ɫ���棬��Ҫ���ý�ɫ��y�ᷭת180��
    /// </summary>
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    /// <summary>
    /// ���ƽ�ɫ�Ƿ���Ҫ����
    /// </summary>
    /// <param name="_x"></param>
    protected virtual void FlipControl(float _x)
    {
        //�����ƶ����泯���
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)//�����ƶ����泯�ұ�
            Flip();
    }
    #endregion
    #region Velocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        Rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipControl(_xVelocity);
    }
    #endregion
}
