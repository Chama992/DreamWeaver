using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUIManager : SingleTon<InGameUIManager>
{
    [SerializeField]private Player player;
    [Header("Info")]
    public TMP_Text levelDepthTmp;
    public TMP_Text threadLengthTmp;
    public TMP_Text ScoreTmp;
    public List<Image> Stars;
    private float scores;
    private int  levelDepth;
    private float threadLength;
    [Header("RoguePanel")]
    public GameObject roguePropPanel;
    public GameObject roguePropFrame;
    private List<GameObject> propRogueFrameObjectsPool = new();
    private int chooseCount;
    [Header("PropPanel")]
    public GameObject propPanel;
    public GameObject propFrame;
    public Dictionary<int, PropFrameUI> propFrameUIs = new();
    // private List<GameObject> propFrameObjectsPool = new();
    public Dictionary<int, int> propFrameUISave = new();

    private void Start()
    {
        GameController.instance.onLevelStart += OnLevelBegin;
        GameController.instance.onLevelReset += onLevelReset;
    }
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Tab))
        // {
        //     OpenRoguePropPanel(1);
        // }
        // if (Input.GetKeyDown(KeyCode.CapsLock))
        // {
        //     CloseRoguePropPanel();
        // }
        SetThreadLength(GameController.instance.levelWeaveLength);
        SetLevelDepth(GameController.instance.level);
        SetScore(GameController.instance.score);
        SetStars(GameController.instance.stars);
    }

    /// <summary>
    /// 设置关卡深度
    /// </summary>
    /// <param name="_levelDepth"></param>
    private void SetLevelDepth( int _levelDepth )
    {
        levelDepth = _levelDepth;
        levelDepthTmp.text ="Depth:" +  levelDepth.ToString();
    }
    /// <summary>
    /// 设置线的长度
    /// </summary>
    /// <param name="_threadLength"></param>
    private void SetThreadLength(float _threadLength)
    {
        threadLength = _threadLength;
        threadLengthTmp.text ="Lengths" +  threadLength.ToString();
    }
    /// <summary>
    /// 设置关卡分数
    /// </summary>
    /// <param name="_scores"></param>
    private void SetScore(float _scores)
    {
        scores = _scores;
        ScoreTmp.text = "Scores" +  scores.ToString();
    }
    /// <summary>
    /// 设置星星个数
    /// </summary>
    /// <param name="_starCount"></param>
    private void SetStars(int _starCount)
    {
        for (int i = 0; i < 3; i++)
        {
            if( i < _starCount)
                Stars[i].color = Color.red;
            else
                Stars[i].color = Color.white;

        }
    }
    /// <summary>
    /// 打开肉鸽面板
    /// </summary>
    public void OpenRoguePropPanel(int count)
    {
        FX.instance.SmoothSizeAppear(roguePropPanel);
        chooseCount = count;
        GenerateRoguePropPanel();
    }
    /// <summary>
    /// 生成肉鸽面板
    /// </summary>
    public void GenerateRoguePropPanel()
    {
        List<PropData> propDatas = PropDataManager.Instance.GetRandomProps();
        for (int i = 0; i < 3; i++)
        {
            GameObject roguePropFrame = GetFromRoguePropFramePool();
            roguePropFrame.transform.SetAsFirstSibling();
            roguePropFrame.GetComponent<RogueFrameUI>().Initialize(propDatas[i]);
        }
    }

    public void ChoseRogue()
    {
        chooseCount--;
        CloseRoguePropPanel();
        if (chooseCount > 0)
        {
            GenerateRoguePropPanel();
        }
        else
        {
            GameController.instance.ReadyLevel();
            FX.instance.SmoothSizeDisappear(roguePropPanel);
        }

    }
    /// <summary>
    /// 关闭肉鸽面板
    /// </summary>
    public void CloseRoguePropPanel()
    {
        // roguePropPanel.SetActive(false);
        Transform rogueGroup = roguePropPanel.transform.Find("RogueGroup");
        for (int i = 0; i < 3; i++)
        {
            GameObject roguePropFrame = rogueGroup.GetChild(i).gameObject;
            roguePropFrame.transform.SetAsLastSibling();
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
                // GameObject propFrameObject = propPanel.GetComponent<ScrollRect>().content.Find(propId.ToString()).gameObject;
                // InPropFramePool(propFrameObject);
                Destroy(propPanel.GetComponent<ScrollRect>().content.Find(propId.ToString()).gameObject); 
            }
            else
                propFrameUIs[propId].GetComponent<PropFrameUI>().ChangePropCount(propCount); 
        }
        else
        {
            // GameObject propFrameObject = GetFromPropFramePool();
            // PropFrameUI newProp = propFrameObject.GetComponent<PropFrameUI>();
            // newProp.Initialize(PropDataManager.Instance.GetPropData(propId), propCount);
            // propFrameUIs.Add(propId, newProp);
            PropFrameUI newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content).GetComponent<PropFrameUI>();
            newProp.Initialize(PropDataManager.Instance.GetPropData(propId), propCount);
            propFrameUIs.Add(propId,newProp);
        }
    }
    public void FreshPropPanelComplete()
    {
        
        Transform props = propPanel.GetComponent<ScrollRect>().content;
        for (int i = 0; i < props.childCount; i++)
            Destroy(props.GetChild(i).gameObject);
        propFrameUIs.Clear();
        foreach (var pair in propFrameUISave)
        {
            if (pair.Value != 0)
            {
                PropFrameUI newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content).GetComponent<PropFrameUI>();
                newProp.Initialize(PropDataManager.Instance.GetPropData(pair.Key), pair.Value);
                propFrameUIs.Add(pair.Key,newProp); 
            }
        }
    }

    private void OnLevelBegin()
    {
        propFrameUISave = new Dictionary<int, int>(player.Props.props);
     }
    private void onLevelReset()
    {
        FreshPropPanelComplete();
        player.Props.props = new Dictionary<int, int>(propFrameUISave);
        foreach (int key in player.Props.props.Keys)
        {
            if (player.Props.props[key] == 0)
            {
                player.Props.props.Remove(key);
            }
        }
    }
    
#if 框框对象池有bug
    // /// <summary>
    // /// 框框对象池入池
    // /// </summary>
    // /// <param name="propFrameObject"></param>
    // private void InPropFramePool(GameObject propFrameObject)
    // {
    //     propFrameObjectsPool.Add(propFrameObject);
    //     propFrameObject.SetActive(false);
    // }
    // /// <summary>
    // /// 框框对象池出池
    // /// </summary>
    // /// <returns></returns>
    // private GameObject GetFromPropFramePool()
    // {
    //     GameObject newProp;
    //     if (propFrameObjectsPool.Count != 0)
    //     {
    //         newProp = propFrameObjectsPool[0];
    //         newProp.transform.SetAsLastSibling();
    //         newProp.SetActive(true);
    //         propFrameObjectsPool.RemoveAt(0);
    //         return newProp;
    //     }
    //     newProp = Instantiate(propFrame, propPanel.GetComponent<ScrollRect>().content);
    //     return newProp;
    // }
    #endif
}
