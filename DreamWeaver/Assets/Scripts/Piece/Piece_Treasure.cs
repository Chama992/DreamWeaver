using UnityEngine;

public class Piece_Treasure : Piece
{
    [SerializeField] private SpriteRenderer treasureBox;
    [SerializeField] private Sprite treasureClose;
    [SerializeField] private Sprite treasureOpen;
    private bool isOpened;


    protected override void Start()
    {
        treasureBox.sprite = treasureClose;
    }
    protected override void Update()
    {
        base.Update();
        if (!isOpened&&(GameController.instance.player.transform.position - treasureBox.transform.position).magnitude <= GameController.instance.interactRatio)
        {
            isOpened = true;
            OpenTreasure();
        }
    }

    private void OpenTreasure()
    {
        treasureBox.sprite = treasureOpen;
        //调用相关方法
        GameController.instance.AddBonue(1);
    }

    protected override void ResetPiece()
    {
        base.ResetPiece();
        if(isOpened)
        {
            isOpened = false;
            GameController.instance.AddBonue(-1);
            treasureBox.sprite = treasureClose;
        }
    }

}
