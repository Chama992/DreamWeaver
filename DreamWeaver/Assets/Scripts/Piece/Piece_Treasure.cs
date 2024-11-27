using UnityEngine;

public class Piece_Treasure : Piece
{
    [SerializeField] private SpriteRenderer treasureBox;
    [SerializeField] private Sprite treasureOpen;

    protected override void Update()
    {
        base.Update();
        if ((GameController.instance.player.transform.position - treasureBox.transform.position).magnitude <= GameController.instance.interactRatio)
        {
            OpenTreasure();
        }
    }

    private void OpenTreasure()
    {
        treasureBox.sprite = treasureOpen;
        //������ط���
        GameController.instance.AddBonue(1);
    }

}
