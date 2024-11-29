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
    /// ��ƬΨһid
    /// </summary>
    public int id;
    /// <summary>
    /// ��Ƭ����
    /// </summary>
    public Theme theme;
    /// <summary>
    /// ��Ƭ�ڵ�
    /// </summary>
    public Transform node;
    /// <summary>
    /// ��Ƭ�ѶȲ�����Խ����ζ��Խ�����
    /// </summary>
    public int difficulty;
    /// <summary>
    /// �Ƿ�Ϊ���㣬�������յ�
    /// </summary>
    public bool isCheckPoint;
    /// <summary>
    /// �Ƿ�ɷ�ת
    /// </summary>
    public bool canFilp;
    /// <summary>
    /// ������������������
    /// </summary>
    [HideInInspector] public float ramdomInt;
    /// <summary>
    /// �Ƿ���������
    /// </summary>
    [HideInInspector] public bool allowLink = true;

    protected virtual void Start()
    {
        ramdomInt = Random.Range(0f, 1f);
        GameController.instance.onLevelReset += ResetPiece;
        if(canFilp&&Random.Range(0,2)==0)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    protected virtual void OnDisable()
    {
        GameController.instance.onLevelReset -= ResetPiece;
    }
    protected virtual void Update()
    {

    }
    protected virtual void ResetPiece()
    {

    }
}
