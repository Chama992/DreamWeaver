using System.Collections.Generic;
using UnityEngine;

public class PlayerProps
{
    private Player player;

    public void Initialize(Player _player)
    {
        player = _player;
    }

    private Dictionary<int, int> props = new Dictionary<int, int>();//前一个是id 后一个是数量
    /// <summary>
    /// 获得道具
    /// </summary>
    /// <param name="propId"></param>
    public void GetProps(int propId,int count)
    {
        if (props.ContainsKey(propId))
            props[propId] += count;
        else
            props.Add(propId, count);
        InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
    }
    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="propId"></param>
    public void UseProp(int propId)
    {
        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            props[propId]--;
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            //TODO： 在这里写使用道具逻辑
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    
}
