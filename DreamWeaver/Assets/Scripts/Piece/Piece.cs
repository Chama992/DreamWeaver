using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Theme
{ 
}
[Serializable]
public class Piece : MonoBehaviour
{
    /// <summary>
    /// 碎片唯一id
    /// </summary>
    public int id;
    /// <summary>
    /// 碎片主题
    /// </summary>
    public Theme theme;
    /// <summary>
    /// 碎片节点
    /// </summary>
    public Transform node;
    /// <summary>
    /// 碎片难度参数，越大意味着越后出现
    /// </summary>
    public int difficulty;
    /// <summary>
    /// 是否为检查点，即起点或终点
    /// </summary>
    public bool isCheckPoint;
    /// <summary>
    /// 是否可翻转
    /// </summary>
    public bool canFilp;
    /// <summary>
    /// 随机参数，用于其机制
    /// </summary>
    [HideInInspector] public float ramdomInt;
    /// <summary>
    /// 是否允许被连接
    /// </summary>
    [HideInInspector] public bool allowLink = true;

    protected virtual void Start()
    {
        ramdomInt = Random.Range(0f, 1f);
    }
    protected virtual void Update()
    {

    }
}
