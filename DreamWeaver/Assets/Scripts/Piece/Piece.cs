using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Piece : MonoBehaviour
{
    [Tooltip("��Ƭ����")]
    public Transform backGround;

    [Tooltip("��Ƭ�ڵ�")]
    public Transform node;

    [Tooltip("��Ƭ�ڵ�������ͼ��")]
    public Sprite nodedSprited;

    [Tooltip("��Ƭ�ѶȲ�����Խ����ζ��Խ�����")]
    public int difficulty;

    [Tooltip("�Ƿ�Ϊ���㣬�������յ�")]
    public bool isCheckPoint;

    [Tooltip("�Ƿ�ɷ�ת")]
    public bool canFilp;

    /// <summary>
    /// ������������������
    /// </summary>
    [HideInInspector] public float ramdomInt;
    /// <summary>
    /// �Ƿ���������
    /// </summary>
    [HideInInspector] public bool allowLink = true;
    /// <summary>
    /// �Ƿ�����
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
