using System.Collections.Generic;
using UnityEngine;

public class PlayerProps
{
    private Dictionary<int, int> props = new Dictionary<int, int>();//前一个是id 后一个是数量
    /// <summary>
    /// 获得道具
    /// </summary>
    /// <param name="propId"></param>
    public void GetProps(int propId)
    {
        if (props.ContainsKey(propId))
            props[propId]++;
        else
            props.Add(propId, 1);
        InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
    }
    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="propId"></param>
    public void UseProps(int propId)
    {
        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            props[propId]--;
            //TODO： 在这里写使用效果
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    
}
