using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class PropManager : SingleTon<PropManager>
{
    private Dictionary<int, PropData> propPools = new();
    private void Start()
    {
        StartCoroutine(LoadPropDataCoroutine());
    }
    /// <summary>
    /// 获取道具数据
    /// </summary>
    /// <param name="propId"></param>
    /// <returns></returns>
    public PropData GetPropData(int propId)
    {
        if (propPools.ContainsKey(propId))
            return propPools[propId];
        return Resources.Load<PropData>("Data/Props/" + propId.ToString());
    }
    /// <summary>
    /// 加载道具数据
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadPropDataCoroutine( )
    {
        int propId = 1;
        while (true)
        {
            PropData propData = Resources.Load<PropData>("Props/" + propId.ToString());
            if (propData is null)
                break;
            propPools.Add(propId++, propData);
            yield return null;
        }
    }
    /// <summary>
    /// 获取随机三个道具数据
    /// </summary>
    /// <returns></returns>
    public List<PropData> GetRandomProps(int count = 3)
    {
        List<PropData> props = new();
        List<int> propsId = propPools.Keys.ToList();
        while (props.Count < count)
        {
            int propId = propsId[Random.Range(0, propsId.Count)];
            propsId.Remove(propId);
            props.Add(propPools[propId]);
        }

        return props;
    }
}