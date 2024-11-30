using UnityEngine;


public class Reset : PropEffector
{
    public override void Initialize(PropEffectorManager _manager)
    {
        base.Initialize(_manager);
    }

    public override void Instant()
    {
        base.Instant();
        GameController.instance.RegenerateLevel();
    }
}
