using Unity.Mathematics;
using UnityEngine;

public class Bomb : PropEffector
{
    private float force;
    private float forceUp;
    private float radius;
    private float waitTime;
    private GameObject bomb;
    public override void Initialize(PropEffectorManager _manager, int _id)
    {
        base.Initialize(_manager,_id);
        PropEffectorType = PropEffectorType.Constant;
        force = _manager.bombForce;
        forceUp = _manager.bombForceUp;
        radius = _manager.bombRadius;
        waitTime = _manager.bombWaitTime;
        propDuration = waitTime;
        propEffectCounter = propDuration;
        GameObject bombPrefab = Resources.Load<GameObject>("Prefab/Bomb");
        bomb =  GameObject.Instantiate(bombPrefab,player.transform.position,Quaternion.identity);
    }
    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0)
        {
            bomb.GetComponent<Animator>().speed = 1;
            propActive = false;
            Collider2D collider2D =  Physics2D.OverlapCircle(bomb.transform.position, radius, LayerMask.GetMask("Player"));
            if (collider2D != null)
            {
                player.Rb.velocity = new Vector2(player.Rb.velocity.x, 0);
                player.Rb.AddForce((collider2D.transform.position - bomb.transform.position).normalized * force + Vector3.up * forceUp, ForceMode2D.Impulse);
            }
            MySoundManager.PlayAudio("Õ¨µ¯");
        }
    }
}
