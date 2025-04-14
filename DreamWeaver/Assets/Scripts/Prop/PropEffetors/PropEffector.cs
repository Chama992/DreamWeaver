using UnityEngine;


public  class PropEffector
{
    public PropEffectorType PropEffectorType;
    protected int propId;
    protected float propDuration;
    protected float propEffectCounter;
    public bool propActive;
    protected Player player;

    public virtual void Initialize(PropEffectorManager _manager, int _id)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        propId = _id;
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
