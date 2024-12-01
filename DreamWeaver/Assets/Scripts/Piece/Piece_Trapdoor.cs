using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Trapdoor : Piece
{
    [SerializeField] private float waitTime;
    [SerializeField] private float openSpeed;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    private float waitTimer = 100;
    private bool isOpened = false;

    protected override void Update()
    {
        base.Update();
        if(isOpened)
        {
            if (Left.rotation.eulerAngles.z == 0 ||Left.rotation.eulerAngles.z > 270)
            {
                Left.Rotate(0, 0, -Time.deltaTime * openSpeed);
            }
            if (Right.rotation.eulerAngles.z < 90)
            {
                Right.Rotate(0, 0, Time.deltaTime * openSpeed);
            }
            MySoundManager.PlayAudio("活板门打开");
        }
        if((GameController.instance.player.transform.position-checkPoint.position).magnitude<GameController.instance.interactRatio)
        {
            waitTimer -= Time.deltaTime;
            if(waitTimer < 0)
            {
                isOpened = true;
            }
        }
        else
        {
            waitTimer = waitTime;
            //if (Left.rotation.z < 0)
            //{
            //    Left.Rotate(0, 0, Time.deltaTime * openSpeed);
            //}
            //if (Right.rotation.z > 0)
            //{
            //    Right.Rotate(0, 0, -Time.deltaTime * openSpeed);
            //}
        }
    }

    protected override void ResetPiece()
    {
        base.ResetPiece();
        isOpened = false;
        Left.rotation = Quaternion.identity;
        Right.rotation = Quaternion.identity;
    }
}
