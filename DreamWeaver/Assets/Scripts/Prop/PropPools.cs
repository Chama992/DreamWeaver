using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public class PropPools : SingleTon<PropPools>
{
    private Dictionary<int, PropData> propPools = new();
    private void Start()
    {
        this.StartCoroutine(LoadPropDataCoroutine);
    }

    public PropData GetPropData(int propId)
    {
        if (propPools.ContainsKey(propId))
            return propPools[propId];
        return Resources.Load<PropData>("Props/" + propId.ToString());
    }
    private IEnumerator LoadPropDataCoroutine( )
    {
        int propId = 1;
        while (true)
        {
            PropData propData = Resources.Load<PropData>("Props/" + propId.ToString());
            if (propData == null)
                break;
            propPools.Add(propId++, propData);
            yield return null;
        }
        
    }
}