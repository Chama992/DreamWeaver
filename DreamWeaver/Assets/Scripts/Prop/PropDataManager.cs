using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class PropDataManager : SingleTon<PropDataManager>
{
    private Dictionary<int, PropData> propPools = new();
    protected override void Awake()
    {
        base.Awake();
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
    /// 获取道具类型
    /// </summary>
    /// <param name="propId"></param>
    /// <returns></returns>
    public PropType GetPropType(int propId)
    {
        if (propPools.ContainsKey(propId))
            return propPools[propId].propType;
        return Resources.Load<PropData>("Data/Props/" + propId.ToString()).propType;
    }
    public PropEffectorType GetPropTypeEffectorType(int propId)
    {
        if (propPools.ContainsKey(propId))
            return propPools[propId].propEffectorType;
        return Resources.Load<PropData>("Data/Props/" + propId.ToString()).propEffectorType;
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
            PropData propData = Resources.Load<PropData>("Data/Props/" + propId.ToString());
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

public enum PropType
{
    Fireworks,
    Feather,
    HookLock,
    Bomb,
    Reset,
    Hammer,
    BlackHole
}