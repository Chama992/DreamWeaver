using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : PropEffector
{

    public override void Initialize(PropEffectorManager _manager)
    {
        base.Initialize(_manager);
    }

    public override void Instant()
    {
        base.Instant();
        // GameController.instance.GenerateRandomPiece(GameController.instance.enabledPieces);

    }

    public override void Update()
    {
        base.Update();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
