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
        //GameController.instance.AddBonue(1);
        //GameController.instance.AddBlackHole(1);
        //GameController.instance.AddScoreModifier(1);
        //TODO: GameControl��һ���ӿڣ���һ������������
    }
}
