using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Spring : Piece
{
    [SerializeField] private Transform spring;
    [SerializeField] private Sprite springIn;
    [SerializeField] private Sprite springOut;
    [SerializeField] private float force;
    [SerializeField] private float detectRange;
    private float CDTimer = 1f;

    protected override void Update()
    {
        base.Update();
        if(CDTimer>0)
        {
            CDTimer -= Time.deltaTime;
        }
        else if((GameController.instance.player.transform.position - spring.position).magnitude<detectRange)
        {
            spring.GetComponent<SpriteRenderer>().sprite = springOut;
            CDTimer = 1f;
            GameController.instance.player.Rb.velocity = new Vector2(GameController.instance.player.Rb.velocity.x,0);
            GameController.instance.player.Rb.AddForce(new Vector2(0,force),ForceMode2D.Impulse);
            MySoundManager.PlayAudio("µ¯»É");
        }
        else
        {
            spring.GetComponent<SpriteRenderer>().sprite = springIn;
        }
    }

    protected override void ResetPiece()
    {
        base.ResetPiece();
        spring.GetComponent<SpriteRenderer>().sprite = springIn;
    }
}
