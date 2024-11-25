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
    private Dictionary<int, GameObject> propFrameUIs = new();//记录第几个位置是哪个道具
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// 设置关卡深度
    /// </summary>
    /// <param name="_levelDepth"></param>
    private void SetLevelDepth( int _levelDepth )
    {
        levelDepth = _levelDepth;
        levelDepthTmp.text = levelDepth.ToString();
    }
    /// <summary>
    /// 设置线的长度
    /// </summary>
    /// <param name="_threadLength"></param>
    private void SetThreadLength(float _threadLength)
    {
        threadLength = _threadLength;
        threadLengthTmp.text = threadLength.ToString("P2");
    }
    /// <summary>
    /// 生成肉鸽面板
    /// </summary>
    public void GenerateRoguePropPanel()
    {
        
    }
    /// <summary>
    /// 关闭肉鸽面板
    /// </summary>
    public void DestroyRoguePropPanel()
    {
        
    }
    /// <summary>
    /// 生成肉鸽单个选项卡
    /// </summary>
    private void GenerateRoguePropFrame()
    {
    }
    /// <summary>
    /// 刷新局内道具面板
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
