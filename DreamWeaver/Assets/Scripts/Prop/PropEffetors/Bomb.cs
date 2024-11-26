using Unity.Mathematics;
using UnityEngine;

public class Bomb : PropEffector
{
    private float force;
    private float radius;
    private GameObject bomb;
    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Constant;
        propDuration = 3f;
        propEffectCounter = propDuration;
        force = 20f;
        radius = 10;
        GameObject bombPrefab = Resources.Load<GameObject>("Prefab/Bomb");
        bomb =  GameObject.Instantiate(bombPrefab,player.transform.position,Quaternion.identity);
    }
    public override void Update()
    {
        base.Update();
        if (propEffectCounter < 0)
        {
            propActive = false;
            Collider2D collider2D =  Physics2D.OverlapCircle(bomb.transform.position, radius, LayerMask.GetMask("Player"));
            if (collider2D != null)
            {
                player.Rb.AddForce((collider2D.transform.position - bomb.transform.position).normalized * force, ForceMode2D.Impulse);
            }
            GameObject.Destroy(bomb);
        }
    }
}
