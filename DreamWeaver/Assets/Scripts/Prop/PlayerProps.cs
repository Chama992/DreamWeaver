using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProps
{
    private Player player;
    public void Initialize(Player _player)
    {
        player = _player;
    }

    public Dictionary<int, int> props = new Dictionary<int, int>();//前一个是id 后一个是数量
    /// <summary>
    /// 获得道具
    /// </summary>
    /// <param name="propId"></param>
    public void GetProps(int propId,int count)
    {
        //if (PropDataManager.Instance.GetPropTypeEffectorType(propId) == PropEffectorType.Special)
        //{
            
        //    return;
        //}
        if (props.ContainsKey(propId))
            props[propId] += count;
        else
            props.Add(propId, count);
        InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
    }

    public void UsePropByIndex(int propIndex)
    {
        RectTransform content = InGameUIManager.Instance.propPanel.GetComponent<ScrollRect>().content;
        if (propIndex > content.childCount)
            return;
        Transform child = InGameUIManager.Instance.propPanel.GetComponent<ScrollRect>().content.GetChild(propIndex - 1);
        UseProp(child.GetComponent<PropFrameUI>().propId);
    }

    /// <summary>
    /// 使用对应ID的道具
    /// </summary>
    /// <param name="propId"></param>
    public void UseProp(int propId)
    {
        if (GameController.instance.Tutorial)
        {
            FX.instance.ShowHint("You can try your props after the tutorial is over~");
            return;
        }

        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            if (!UseSpecificProps(propId))
                return;
            props[propId]--;
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    public bool UseSpecificProps(int propId)
    {
        PropEffectorManager propEffectorManager = player.gameObject.GetComponent<PropEffectorManager>();
        PropType propType = PropDataManager.Instance.GetPropType(propId);
        bool useProp = true;
        switch (propType)
        {
            case PropType.Fireworks:
                propEffectorManager.AddPropEffector<Firework>(propId);
                return true;
                break;
            case PropType.Feather:
                propEffectorManager.AddPropEffector<Feather>(propId);
                return true;
                break;
            case PropType.HookLock:
                if (!player.canGrap)
                    useProp = false;
                else
                {
                    propEffectorManager.AddPropEffector<HookLock>(propId);
                    player.canGrap = false;
                }
                break;
            case PropType.Bomb:
                if (player.IsGroundChecked())
                    propEffectorManager.AddPropEffector<Bomb>(propId);
                else
                    useProp = false;
                break;
            case PropType.Reset:
                InGameUIManager.Instance.propFrameUISave[propId]--;
                propEffectorManager.AddPropEffector<Reset>(propId);
                break;
            case PropType.Hammer:
                if (!player.canBuild)
                    useProp = false;
                else
                    propEffectorManager.AddPropEffector<Hammer>(propId);
                break;
            default:
                break;
        }
        return useProp;
    }
}
