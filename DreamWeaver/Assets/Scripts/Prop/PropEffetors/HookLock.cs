using UnityEngine;


public class HookLock : PropEffector
{
    private float radius;
    private LineRenderer playerLineRender;
    public override void Initialize()
    {
        base.Initialize();
        PropEffectorType = PropEffectorType.Constant;
        radius = 4f;
        propEffectCounter = 8f;
        playerLineRender= player.gameObject.AddComponent<LineRenderer>();
        playerLineRender.startWidth = 0.1f;
        playerLineRender.endWidth = 0.1f;
        playerLineRender.positionCount = 2;
        playerLineRender.material = Resources.Load<Material>("Materials/Lock");
    }
    public override void Instant()
    {
        base.Instant();
    }

    public override void Update()
    {
        base.Update();
        propEffectCounter -= Time.deltaTime;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));;
        playerLineRender.SetPosition(0, player.transform.position);
        playerLineRender.SetPosition(1, (mousePos - player.transform.position).normalized * radius);
        Debug.Log(mousePos);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position,(mousePos - player.transform.position).normalized,radius,LayerMask.GetMask("Ground"));
            if (hit)
            {
                player.HookState.SetTarget(hit.point);
                player.StateMachine.ChangeState(player.HookState);
            }
            GameObject.Destroy(player.GetComponent<LineRenderer>());
            propActive = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject.Destroy(player.GetComponent<LineRenderer>());
            propActive = false;
        }
    }
}
