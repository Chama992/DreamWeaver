using System;
using System.Collections.Generic;
using UnityEngine;


public class PropEffectorManager : MonoBehaviour
{
    public static PropEffectorManager instance;
    private List<PropEffector> propEffectors = new();
    [Header("Firework")]
    public float force;
    [Header("Bomb")]
    public float bombWaitTime;
    public float bombForce;
    public float bombForceUp;
    public float bombRadius;
    [Header("Feather")]
    public float decreaseScale;
    public float featherPropDuration;
    [Header("HookLock")]
    public float radius;
    public float hookSpeed;
    public float hookLockPropDuration;
    [Header("Hammer")]
    public float hammerPropDuration;
    public List<Vector3> position2Generate = new List<Vector3>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
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
            propRemoveEffectors[i].Destroy();
            propEffectors.Remove(propRemoveEffectors[i]);
        }
    }

    private void OnDisable()
    {
        GameController.instance.onLevelReset -= OnGameReset;
    }

    public void AddPropEffector<T>(int _id) where T : PropEffector,new()
    {
        T newPropEffector = new T();
        newPropEffector.Initialize(this,_id);
        propEffectors.Add(newPropEffector);
    }

    private void OnGameReset()
    {
        foreach (var effect in propEffectors)
        {
            effect.Destroy();
        }
        propEffectors.Clear();
        position2Generate = GameController.instance.pieceGenePositions.FindAll(t => true);
    }
}