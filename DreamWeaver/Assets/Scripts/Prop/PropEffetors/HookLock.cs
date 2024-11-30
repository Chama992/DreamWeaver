using System.Collections;
using UnityEngine;


public class HookLock : PropEffector
{
    private float radius;
    public float hookSpeed;
    private LineRenderer playerLineRender;
    public override void Initialize(PropEffectorManager _manager)
    {
        base.Initialize(_manager);
        PropEffectorType = PropEffectorType.Constant;
        radius = _manager.radius;
        hookSpeed = _manager.hookSpeed;
        propEffectCounter = _manager.hookLockPropDuration;
        playerLineRender= player.gameObject.GetComponent<LineRenderer>();
        playerLineRender.startWidth = 0.25f;
        playerLineRender.endWidth = 0.25f;
        playerLineRender.positionCount = 2;
        playerLineRender.material = Resources.Load<Material>("Materials/Lock");
        playerLineRender.startColor = new Color(105,105,105);
        playerLineRender.endColor = new Color(47,79,79);
        player.LineRenderer.enabled = true;
    }
    public override void Instant()
    {
        base.Instant();
    }

    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        if (propEffectCounter < 0)
        {
            propActive = false;
            playerLineRender.positionCount = 0;
            playerLineRender.enabled = false;
            player.canGrap = true;
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));;
        mousePos.z = 0;
        playerLineRender.SetPosition(0, player.transform.position);
        playerLineRender.SetPosition(1, (player.transform.position + (mousePos - player.transform.position).normalized * radius));
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position,(mousePos - player.transform.position).normalized,radius,LayerMask.GetMask("Ground"));
            if (hit)
            {
                player.HookState.SetTarget(hit.point,hit.collider,hookSpeed);
                player.StateMachine.ChangeState(player.HookState);
                propActive = false;
            }
        }
        // else if (Input.GetMouseButtonDown(1))
        // {
        //     propActive = false;
        // }
    }

    public override void Destroy()
    {
        GameObject.Destroy(player.GetComponent<LineRenderer>());
        propActive = false;
    }
}
