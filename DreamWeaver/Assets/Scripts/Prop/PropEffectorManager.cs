using System;
using System.Collections.Generic;
using UnityEngine;


public class PropEffectorManager : MonoBehaviour
{
    private List<PropEffector> propEffectors = new();
    private void Update()
    {
        if (propEffectors.Count == 0)
            return;
        List<PropEffector> propRemoveEffectors = new List<PropEffector>();
        for (int i = 0; i < propEffectors.Count; i++)
        {
            if (propEffectors[i].propActive)
            {
                if (propEffectors[i].PropEffectorType == PropEffectorType.Instant)
                {
                    propEffectors[i].Instant();
                }
                else if (propEffectors[i].PropEffectorType == PropEffectorType.Constant)
                {
                    propEffectors[i].Update();
                }
            }
            else
            {
                propRemoveEffectors.Add(propEffectors[i]);
            }
        }
        //ÒÆ³ý
        for (int i = 0; i < propRemoveEffectors.Count; i++)
        {
            propEffectors.Remove(propRemoveEffectors[i]);
        }
    }

    public void AddPropEffector<T>() where T : PropEffector,new()
    {
        T newPropEffector = new T();
        newPropEffector.Initialize();
        propEffectors.Add(newPropEffector);
    }
}