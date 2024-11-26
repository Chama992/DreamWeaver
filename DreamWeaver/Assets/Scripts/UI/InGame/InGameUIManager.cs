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
    private List<GameObject> propRogueFrameObjectsPool = new();
    [Header("PropPanel")]
    public GameObject propPanel;
    public GameObject propFrame;
    private Dictionary<int, PropFrameUI> propFrameUIs = new();//记录第几个位置是哪个道具
    private List<GameObject> propFrameObjectsPool = new();
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenRoguePropPanel();
        }
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            CloseRoguePropPanel();
        }
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

    public void OpenRoguePropPanel()
    {
        roguePropPanel.SetActive(true);
        GenerateRoguePropPanel();
    }

    /// <summary>
    /// 生成肉鸽面板
    /// </summary>
    public void GenerateRoguePropPanel()
    {
        List<PropData> propDatas = PropDataManager.Instance.GetRandomProps();
        for (int i = 0; i < propDatas.Count; i++)
        {
            GameObject roguePropFrame = GetFromRoguePropFramePool();
            roguePropFrame.GetComponent<RogueFrameUI>().Initialize(propDatas[i]);
        }
    }
    /// <summary>
    /// 关闭肉鸽面板
    /// </summary>
    public void CloseRoguePropPanel()
    {
        roguePropPanel.SetActive(false);
        Transform rogueGroup = roguePropPanel.transform.Find("RogueGroup");
        for (int i = 0; i < rogueGroup.childCount; i++)
        {
            GameObject roguePropFrame = rogueGroup.GetChild(i).gameObject;
            InRoguePropFramePool(roguePropFrame);
        }
    }
    /// <summary>
    /// 肉鸽框框对象池入池
    /// </summary>
    /// <param name="propFrameObject"></param>
    private void InRoguePropFramePool(GameObject _propRogueFrameObject)
    {
        propRogueFrameObjectsPool.Add(_propRogueFrameObject);
        _propRogueFrameObject.SetActive(false);
    }
    /// <summary>
    /// 肉鸽框框对象池出池
    /// </summary>
    /// <returns></returns>
    private GameObject GetFromRoguePropFramePool()
    {
        GameObject newProp;
        if (propRogueFrameObjectsPool.Count != 0)
        {
            newProp = propRogueFrameObjectsPool[0];
            // newProp.transform.SetAsLastSibling();
            propRogueFrameObjectsPool.RemoveAt(0);
            newProp.SetActive(true);
            return newProp;
            
        }
        newProp = Instantiate(roguePropFrame, roguePropPanel.transform.Find("RogueGroup"));
        return newProp;
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
            newProp.Initialize(PropDataManager.Instance.GetPropData(propId), propCount);
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
            propFrameObjectsPool.RemoveAt(0);
            return newProp;
        }
        newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content);
        return newProp;
    }
}
