using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerNodeControl : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Player player;
    private int pointCount;
    private Dictionary<int,int> piecesSequences = new Dictionary<int,int>(); //用于记录哪个点被连接了
    private bool startDrawLineFlag = false;
    public Material Material;
    private void Awake()
    {
        player = gameObject.GetComponentInParent<Player>();
        player.CrossDoor += OnCrossDoor;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.sortingLayerName = "Player";
        lineRenderer.material = Material;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.blue;
    }
    private void Update()
    {
        if (startDrawLineFlag)
        {
            lineRenderer.SetPosition(pointCount - 1, player.transform.position);
        }
    }

    private void OnDisable()
    {
        player.CrossDoor -= OnCrossDoor;
    }

    public void SetLevelStartPoint(int pieceIndex,Piece piece)
    {
        pointCount = 2;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(0, piece.node.position);
        lineRenderer.SetPosition(1, player.transform.position);
        piecesSequences[pieceIndex] = 0;
        startDrawLineFlag = true;
    }

    public void LinkNode(int pieceIndex,Piece piece)
    {
        if (piece.transform.position == GameController.instance.levelStartPoint)
            return;
        if (piecesSequences.ContainsKey(pieceIndex))
        {
            CancleLinkNode(pieceIndex);
            GameController.instance.TryDisconnectNode(piece);
            return;
        }
        GameController.instance.ConnectNode(piece);
        pointCount++;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount - 2, piece.node.position);
        lineRenderer.SetPosition(pointCount-1, player.transform.position);
        piecesSequences[pieceIndex] = pointCount - 2;
    }
    public void CancleLinkNode(int pieceIndex)
    {
        // pointCount--;
        // lineRenderer.positionCount = pointCount;
        // lineRenderer.SetPosition(pointCount-1, player.transform.position);
        Vector3[] points = new Vector3[pointCount];
        lineRenderer.GetPositions(points);
        List<Vector3> pointList = new List<Vector3>(points);
        pointList.RemoveAt(piecesSequences[pieceIndex]);
        points = pointList.ToArray();
        lineRenderer.SetPositions(points);
        pointCount--;
        lineRenderer.positionCount = pointCount;
        piecesSequences.Remove(pieceIndex);
        lineRenderer.SetPosition(pointCount-1, player.transform.position);
    }
    public void LinkDoor(Vector2  doorPos,Vector2 endDoorPos)
    {
        pointCount += 2;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount - 3, doorPos);
        lineRenderer.SetPosition(pointCount - 2, endDoorPos);
        lineRenderer.SetPosition(pointCount-1, player.transform.position);
    }
    public void ResetLine()
    {
        lineRenderer.positionCount = 0;
        pointCount = 0;
        piecesSequences.Clear();
        startDrawLineFlag = false;
    }

    public void OnCrossDoor( Vector2 startPosition, Vector2 endPosition)
    {
        Debug.Log("穿的们的位置" + startPosition );
        
        Debug.Log("到达们的位置" + endPosition );
    }
}
