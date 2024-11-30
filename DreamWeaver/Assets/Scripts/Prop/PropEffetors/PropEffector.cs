using UnityEngine;


public  class PropEffector
{
    public PropEffectorType PropEffectorType;
    protected float propId;
    protected float propDuration;
    protected float propEffectCounter;
    public bool propActive;
    protected Player player;

    public virtual void Initialize(PropEffectorManager _manager)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        propActive = true;
    }

    public virtual void Update()
    {
        
    }

    public virtual void Instant()
    {
        propActive = false;
    }

    public virtual void Destroy()
    {
        
    }
}
public enum PropEffectorType
{
    Instant,
    Constant,
    Special,
    None
}
