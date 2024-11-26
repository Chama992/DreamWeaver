using UnityEngine;


public class Firework : PropEffector
{
    private float force;
    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Instant;
        force = 30f;
    }
    
    public override void Instant()
    {
        base.Instant();
        player.Rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        // player.Rb.velocity = new Vector2(player.Rb.velocity.x, player.Rb.velocity.y + player.jumpForce);//给一个瞬间的力);
    }
}
