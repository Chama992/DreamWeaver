using System;
using System.Collections.Generic;
using UnityEngine;


public class PropEffectorManager : MonoBehaviour
{
    private List<PropEffector> propEffectors = new();
    [Header("Firework")]
    [SerializeField]public float force;
    [Header("Bomb")]
    [SerializeField]public float bombforce;
    [SerializeField]public float bombforceUp;
    [SerializeField]public float bombradius;
    [Header("Feather")]
    [SerializeField]public float decreaseScale;
    [SerializeField]public float featherPropDuration;
    [Header("HookLock")]
    [SerializeField]public float radius;
    [SerializeField]public float hookSpeed;
    [SerializeField]public float hookLockPropDuration;
    [Header("Hammer")]
    [SerializeField]public float hammerPropDuration;
    private void Start()
    {
        GameController.instance.onLevelReset += OnGameReset;
    }
    private void Update()
    {
        if (propEffectors.Count == 0 || GameController.instance.isPausing ||GameController.instance.isReadyAnimating|| GameController.instance.isResetAnimating || !GameController.instance.isGaming)
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

    private void OnDisable()
    {
        GameController.instance.onLevelReset -= OnGameReset;
    }

    public void AddPropEffector<T>() where T : PropEffector,new()
    {
        T newPropEffector = new T();
        newPropEffector.Initialize(this);
        propEffectors.Add(newPropEffector);
    }

    private void OnGameReset()
    {
        foreach (var effect in propEffectors)
        {
            effect.Destroy();
        }
       
    }
}