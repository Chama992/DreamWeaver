using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : PropEffector
{
    private GameObject pointPrefab;
    private List<GameObject> pointBG = new List<GameObject>();
    public override void Initialize(PropEffectorManager _manager, int _id)
    {
        base.Initialize(_manager,_id);
        player.canBuild = false;
        propEffectCounter = _manager.hammerPropDuration;
        PropEffectorType = PropEffectorType.Constant;
        pointPrefab = Resources.Load<GameObject>("Prefab/ChoosePoint") ;
        foreach (Vector3 position in player.GetComponent<PropEffectorManager>().position2Generate)
        {
            GameObject point = GameObject.Instantiate(pointPrefab, position, Quaternion.identity);
            HammerChoosePoint  hammerChoosePoint =  point.GetComponent<HammerChoosePoint>();
            hammerChoosePoint.SetPoint(position);
            hammerChoosePoint.PieceGenerated += OnGenerated;
            pointBG.Add(point);
        }

        if (pointBG.Count == 0)
        {
            OnGenerated();
            player.Props.GetProps(propId,1);
        }
    }
    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0 || Input.GetMouseButtonDown(1))
        {
            OnGenerated();
            player.Props.GetProps(propId,1);
            return;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        OnGenerated();
    }

    public void OnGenerated()
    {
        propActive = false;
        for (int i = 0; i < pointBG.Count; i++)
        {
            pointBG[i].GetComponent<HammerChoosePoint>().PieceGenerated -= OnGenerated;
            GameObject.Destroy(pointBG[i]);
        }
        player.canBuild = true;
    }
}
