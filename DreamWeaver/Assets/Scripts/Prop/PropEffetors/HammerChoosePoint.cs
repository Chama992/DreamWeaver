using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class HammerChoosePoint : MonoBehaviour
{
    private Vector3 point;
    public Action PieceGenerated;
    public void SetPoint(Vector3 _point)
    {
        point = _point;
    }

    public void GeneratePoint()
    {
        Piece piece;
        int tryCount = 10;
        while (true)
        {
            piece = GameController.instance.enabledPieces[Random.Range(0, GameController.instance.enabledPieces.Count)];
            tryCount--;
            if (!(piece.GetType() == typeof(Piece_Door)) || tryCount <= 0)
            {
                break;
            }
        }
        MySoundManager.PlayOneAudio("¹¤³Ì´¸");
        GameController.instance.otherPieces.Add(GameController.instance.GeneratePiece(piece, point, false));
        FindObjectOfType<PropEffectorManager>().position2Generate.Remove(point);
        PieceGenerated?.Invoke();
    }
}
