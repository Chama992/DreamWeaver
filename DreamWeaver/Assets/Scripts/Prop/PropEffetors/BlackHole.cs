using UnityEngine;


public class BlackHole : PropEffector
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Instant()
    {
        base.Instant();
        //TODO: GameControl给一个接口，下一关在那里生成
    }
}
