using System.Collections.Generic;
using UnityEngine;

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
        if (props.ContainsKey(propId))
            props[propId] += count;
        else
            props.Add(propId, count);
        InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
    }
    /// <summary>
    /// ʹ�õ���
    /// </summary>
    /// <param name="propId"></param>
    public void UseProp(int propId)
    {
        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            props[propId]--;
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            //TODO�� ������дʹ�õ����߼�
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    
}
