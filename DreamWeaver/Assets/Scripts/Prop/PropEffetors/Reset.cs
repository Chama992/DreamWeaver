using UnityEngine;


public class Reset : PropEffector
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Instant()
    {
        base.Instant();
        GameController.instance.RegenerateLevel();
    }
}
