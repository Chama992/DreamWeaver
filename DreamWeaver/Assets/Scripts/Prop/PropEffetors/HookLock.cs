using UnityEngine;


public class HookLock : PropEffector
{
    private float radius;

    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Instant;
        radius = 8f;
    }
    public override void Instant()
    {
        base.Instant();
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position,(mousePosWorld - player.transform.position).normalized,radius,LayerMask.GetMask("Ground"));
        if (hit)
        {
            player.HookState.SetTarget(hit.point);
        }
    }
}
