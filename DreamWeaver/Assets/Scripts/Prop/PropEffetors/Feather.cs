using UnityEngine;

public class Feather : PropEffector
{
    private float decreaseScale;
    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Constant;
        propDuration = 1.5f;
        propEffectCounter = propDuration;
        decreaseScale = 0.75f;
    }
    public override void Update()
    {
        base.Update();
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
