using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : PropEffector
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Instant()
    {
        base.Instant();
        //GameController.instance.GenerateRandomPiece(GameController.instance.enabledPieces);
    }

    public override void Update()
    {
        base.Update();
    }
}
