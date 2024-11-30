using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Piece : MonoBehaviour
{
    [Tooltip("碎片背景")]
    public Transform backGround;

    [Tooltip("碎片节点")]
    public Transform node;

    [Tooltip("碎片节点已连接图案")]
    public Sprite nodedSprited;

    [Tooltip("碎片难度参数，越大意味着越后出现")]
    public int difficulty;

    [Tooltip("是否为检查点，即起点或终点")]
    public bool isCheckPoint;

    [Tooltip("是否可翻转")]
    public bool canFilp;

    /// <summary>
    /// 随机参数，用于其机制
    /// </summary>
    [HideInInspector] public float ramdomInt;
    /// <summary>
    /// 是否允许被连接
    /// </summary>
    [HideInInspector] public bool allowLink = true;
    /// <summary>
    /// 是否被连接
    /// </summary>
    [HideInInspector] public bool isLinked = false;

    protected virtual void Start()
    {
        node.GetComponent<SpriteRenderer>().sprite = null;
        ramdomInt = Random.Range(0f, 1f);
        GameController.instance.onLevelReset += ResetPiece;
        if(canFilp&&Random.Range(0,2)==0)
        {
            transform.Rotate(0, 180, 0);
        }
        backGround.Rotate(0, 0, Random.Range(-30, 30));
    }

    protected virtual void OnDisable()
    {
        GameController.instance.onLevelReset -= ResetPiece;
    }
    protected virtual void Update()
    {
        if(isLinked)
        {
            node.GetComponent<SpriteRenderer>().sprite = nodedSprited;
        }
    }
    protected virtual void ResetPiece()
    {
        isLinked = false;
        node.GetComponent<SpriteRenderer>().sprite = null;
    }
}
