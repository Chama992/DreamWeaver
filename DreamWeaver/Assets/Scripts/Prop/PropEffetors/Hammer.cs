using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : PropEffector
{
    private GameObject pointPrefab;
    private List<GameObject> pointBG = new List<GameObject>();
    public override void Initialize(PropEffectorManager _manager)
    {
        base.Initialize(_manager);
        player.canBuild = false;
        propEffectCounter = _manager.hammerPropDuration;
        PropEffectorType = PropEffectorType.Constant;
        pointPrefab = Resources.Load<GameObject>("Prefab/ChoosePoint") ;
        foreach (Vector3 position in GameController.instance.pieceGenePositions)
        {
            GameObject point = GameObject.Instantiate(pointPrefab, position, Quaternion.identity);
            HammerChoosePoint  hammerChoosePoint =  point.GetComponent<HammerChoosePoint>();
            hammerChoosePoint.SetPoint(position);
            hammerChoosePoint.PieceGenerated += OnGenerated;
            pointBG.Add(point);
        }
    }
    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0)
        {
            OnGenerated();
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
            pointBG[i].AddComponent<HammerChoosePoint>().PieceGenerated -= OnGenerated;
            GameObject.Destroy(pointBG[i]);
        }
        player.canBuild = true;
    }
}
