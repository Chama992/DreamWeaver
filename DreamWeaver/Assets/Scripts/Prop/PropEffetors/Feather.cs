using UnityEngine;

public class Feather : PropEffector
{
    private float decreaseScale;
    private float propDuration;
    public override void Initialize(PropEffectorManager _manager, int _id)
    {
        base.Initialize(_manager,_id);
        PropEffectorType = PropEffectorType.Constant;
        propDuration = _manager.featherPropDuration;
        propEffectCounter = propDuration;
        decreaseScale = _manager.decreaseScale;
    }
    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0)
        {
            propActive = false;
            return;
        }
        if (player.AirState.stateActive)
        {
            player.Rb.velocity = new Vector2(player.Rb.velocity.x, player.Rb.velocity.y * decreaseScale);
        }
    }
}
