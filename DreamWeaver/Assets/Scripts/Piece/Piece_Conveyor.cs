using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Conveyor : Piece
{
    [SerializeField] private float speed;
    [SerializeField] private float force;


    private SurfaceEffector2D se;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        se = GetComponentInChildren<SurfaceEffector2D>();
        anim = GetComponentInChildren<Animator>();
        se.speed = speed;
        se.forceScale = force;
        if(ramdomInt>0.5)
        {
            anim.speed *= -1;
            se.speed *= -1;
        }
        if(Random.Range(0,2)==1)
        {
            GetComponentInChildren<SurfaceEffector2D>().gameObject.transform.Rotate(180, 0, 0);
        }
    }
}
