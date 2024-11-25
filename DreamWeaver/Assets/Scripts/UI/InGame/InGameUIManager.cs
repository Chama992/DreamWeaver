using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class InGameUIManager : SingleTon<InGameUIManager>
{
    private Player player;
    public TMP_Text levelDepthTmp;
    public TMP_Text threadLengthTmp;
    private int  levelDepth;
    private float threadLength;
    private GameObject roguePropPanel;
    private GameObject roguePropFrame;
    private GameObject propPanel;
    private GameObject propFrame;
    private Dictionary<int, GameObject> propFrameUIs = new();//��¼�ڼ���λ�����ĸ�����
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// ���ùؿ����
    /// </summary>
    /// <param name="_levelDepth"></param>
    private void SetLevelDepth( int _levelDepth )
    {
        levelDepth = _levelDepth;
        levelDepthTmp.text = levelDepth.ToString();
    }
    /// <summary>
    /// �����ߵĳ���
    /// </summary>
    /// <param name="_threadLength"></param>
    private void SetThreadLength(float _threadLength)
    {
        threadLength = _threadLength;
        threadLengthTmp.text = threadLength.ToString("P2");
    }
    /// <summary>
    /// ����������
    /// </summary>
    public void GenerateRoguePropPanel()
    {
        
    }
    /// <summary>
    /// �ر�������
    /// </summary>
    public void DestroyRoguePropPanel()
    {
        
    }
    /// <summary>
    /// ������뵥��ѡ�
    /// </summary>
    private void GenerateRoguePropFrame()
    {
    }
    /// <summary>
    /// ˢ�¾��ڵ������
    /// </summary>
    public void FreshPropPanel(int propId,int propCount)
    {
        if (propFrameUIs.ContainsKey(propId))
        {
            if (propCount == 0)
            {
                propFrameUIs.Remove(propId);
            }
            else
                propFrameUIs[propId].GetComponent<PropFrameUI>().ChangePropCount(propCount); 
        }
        else
        {
            Instantiate(propFrame, propPanel.transform);
            propFrame.GetComponent<PropFrameUI>().Initialize(PropPools.Instance.GetPropData(propId), propCount);
            propFrameUIs.Add(propId,propFrame);
        }
    }
}
