using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUIManager : SingleTon<InGameUIManager>
{
    private Player player;
    public TMP_Text levelDepthTmp;
    public TMP_Text threadLengthTmp;
    private int  levelDepth;
    private float threadLength;
    [Header("RoguePanel")]
    public GameObject roguePropPanel;
    public GameObject roguePropFrame;
    [Header("PropPanel")]
    public GameObject propPanel;
    public GameObject propFrame;
    private Dictionary<int, PropFrameUI> propFrameUIs = new();//记录第几个位置是哪个道具
    private List<GameObject> propFrameObjectsPool = new();
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
        List<PropData> propDatas = PropManager.Instance.GetRandomProps();
        
        for (int i = 0; i < propDatas.Count; i++)
        {
            
        }
    }
    /// <summary>
    /// 关闭肉鸽面板
    /// </summary>
    public void CloseRoguePropPanel()
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
                GameObject propFrameObject = propPanel.GetComponent<ScrollRect>().content.Find(propId.ToString()).gameObject;
                InPropFramePool(propFrameObject);
                // Destroy(propPanel.GetComponent<ScrollRect>().content.Find(propId.ToString()).gameObject); 
            }
            else
                propFrameUIs[propId].GetComponent<PropFrameUI>().ChangePropCount(propCount); 
        }
        else
        {
            GameObject propFrameObject = GetFromPropFramePool();
            PropFrameUI newProp = propFrameObject.GetComponent<PropFrameUI>();
            newProp.Initialize(PropManager.Instance.GetPropData(propId), propCount);
            propFrameUIs.Add(propId, newProp);
            // PropFrameUI newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content).GetComponent<PropFrameUI>();
            // newProp.Initialize(PropPools.Instance.GetPropData(propId), propCount);
            // propFrameUIs.Add(propId,newProp);
        }
    }
    /// <summary>
    /// 框框对象池入池
    /// </summary>
    /// <param name="propFrameObject"></param>
    private void InPropFramePool(GameObject propFrameObject)
    {
        propFrameObjectsPool.Add(propFrameObject);
        propFrameObject.SetActive(false);
    }
    /// <summary>
    /// 框框对象池出池
    /// </summary>
    /// <returns></returns>
    private GameObject GetFromPropFramePool()
    {
        GameObject newProp;
        if (propFrameObjectsPool.Count != 0)
        {
            newProp = propFrameObjectsPool[0];
            newProp.transform.SetAsLastSibling();
            newProp.SetActive(true);
            return newProp;
        }
        newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content);
        return newProp;
    }
}
