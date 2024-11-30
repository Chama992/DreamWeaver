using Unity.Mathematics;
using UnityEngine;

public class Bomb : PropEffector
{
    private float force;
    private float forceUp;
    private float radius;
    private GameObject bomb;
    public override void Initialize(PropEffectorManager _manager)
    {
        base.Initialize(_manager);
        PropEffectorType = PropEffectorType.Constant;
        propDuration = 1.5f;
        propEffectCounter = propDuration;
        force = _manager.bombforce;
        forceUp = _manager.bombforceUp;
        radius = _manager.bombradius;
        GameObject bombPrefab = Resources.Load<GameObject>("Prefab/Bomb");
        bomb =  GameObject.Instantiate(bombPrefab,player.transform.position,Quaternion.identity);
        bomb.GetComponent<Animator>().speed =  0.625f/ propDuration;
    }
    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0)
        {
            propActive = false;
            Collider2D collider2D =  Physics2D.OverlapCircle(bomb.transform.position, radius, LayerMask.GetMask("Player"));
            if (collider2D != null)
            {
                player.Rb.velocity = new Vector2(player.Rb.velocity.x, 0);
                player.Rb.AddForce((collider2D.transform.position - bomb.transform.position).normalized * force + Vector3.up * forceUp, ForceMode2D.Impulse);
            }
            GameObject.Destroy(bomb);
        }
    }
}
