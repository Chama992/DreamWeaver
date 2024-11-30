using UnityEngine;

public class Feather : PropEffector
{
    private float decreaseScale;
    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Constant;
        propDuration = 5f;
        propEffectCounter = propDuration;
        decreaseScale = 0.5f;
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
