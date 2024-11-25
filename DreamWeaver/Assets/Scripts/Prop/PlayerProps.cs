using System.Collections.Generic;
using UnityEngine;

public class PlayerProps
{
    private Dictionary<int, int> props = new Dictionary<int, int>();//ǰһ����id ��һ��������
    /// <summary>
    /// ��õ���
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
    /// ʹ�õ���
    /// </summary>
    /// <param name="propId"></param>
    public void UseProps(int propId)
    {
        if (props.ContainsKey(propId) && props[propId] > 0)
        {
            props[propId]--;
            //TODO�� ������дʹ��Ч��
            InGameUIManager.Instance.FreshPropPanel(propId,props[propId]);
            if (props[propId] == 0)
                props.Remove(propId);
        }
    }
    
}
