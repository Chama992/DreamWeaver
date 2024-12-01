using UnityEngine;


public class Reset : PropEffector
{
    public override void Initialize(PropEffectorManager _manager, int _id)
    {
        base.Initialize(_manager,_id);
    }

    public override void Instant()
    {
        base.Instant();
        GameController.instance.RegenerateLevel();
    }
}
