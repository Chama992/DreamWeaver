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

    private Dictionary<int, int> props = new Dictionary<int, int>();//ǰһ����id ��һ��������
    /// <summary>
    /// ��õ���
    /// </summary>
    /// <param name="propId"></param>
    public void GetProps(int propId,int count)
    {
        if (PropDataManager.Instance.GetPropTypeEffectorType(propId) == PropEffectorType.Special)
        {
            //TODO: д��������߼�
            return;
        }
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
    /// ʹ�ö�ӦID�ĵ���
    /// </summary>
    /// <param name="propId"></param>
    public void UseProp(int propId)
    {
        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            props[propId]--;
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            UseSpecificProps(propId);
            //TODO�� ������дʹ�õ����߼�
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    public void UseSpecificProps(int propId)
    {
        PropEffectorManager propEffectorManager = player.gameObject.GetComponent<PropEffectorManager>();
        PropType propType = PropDataManager.Instance.GetPropType(propId);
        switch (propType)
        {
            case PropType.Fireworks:
                propEffectorManager.AddPropEffector<Firework>();
                break;
            case PropType.Feather:
                propEffectorManager.AddPropEffector<Feather>();
                break;
            case PropType.HookLock:
                propEffectorManager.AddPropEffector<HookLock>();
                break;
            case PropType.Bomb:
                propEffectorManager.AddPropEffector<Bomb>();
                break;
            case PropType.Reset:
                propEffectorManager.AddPropEffector<Feather>();
                break;
            case PropType.Hammer:
                propEffectorManager.AddPropEffector<Feather>();
                break;
            case PropType.BlackHole:
                propEffectorManager.AddPropEffector<Feather>();
                break;
        }
    }
}
