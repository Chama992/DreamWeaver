using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_CheckPoint : Piece
{
    public List<Transform> startTutor;
    public List<Transform> endTutor;

    protected override void Start()
    {
        base.Start();
        GameController.instance.onLevelStart += DisableTutorial;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.instance.onLevelStart -= DisableTutorial;
    }
    public override void ShowTutorial()
    {
        if (showed || tutorial == null)
            return;
        base.ShowTutorial();
        if (GameController.instance.level >= startTutor.Count)
            return;

        if(transform.position == GameController.instance.levelStartPoint && startTutor[GameController.instance.level]!=null)
        {
            FX.instance.SmoothSizeAppear(startTutor[GameController.instance.level].gameObject);
        }
        else if(transform.position == GameController.instance.levelEndPoint && endTutor[GameController.instance.level] != null)
        {
            FX.instance.SmoothSizeAppear(endTutor[GameController.instance.level].gameObject);
        }
    }

    public void DisableTutorial()
    {
        if(GameController.instance.level != 0)
        {
            if (endTutor[GameController.instance.level - 1] != null) 
                FX.instance.SmoothSizeDisappear(endTutor[GameController.instance.level - 1].gameObject);
        }
        ShowTutorial();
    }
}
