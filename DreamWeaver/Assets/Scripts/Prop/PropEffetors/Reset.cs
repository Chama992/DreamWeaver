using UnityEngine;


public class Reset : PropEffector
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Instant()
    {
        base.Instant();
        //TODO: GameControl给一个接口，指定位置生成一个随机碎片
    }
}
