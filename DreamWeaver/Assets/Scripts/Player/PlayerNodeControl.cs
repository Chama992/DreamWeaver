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
    public GameObject testStartPosition;
    protected virtual void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        SetLevelStartPoint(testStartPosition.transform.position,0);
    }
    private void Update()
    {
        if (startDrawLineFlag)
        {
            lineRenderer.SetPosition(pointCount - 1, player.transform.position);
        }
    }
    public void SetLevelStartPoint(Vector3 point,int pieceIndex)
    {
        pointCount = 2;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(0, point);
        lineRenderer.SetPosition(1, player.transform.position);
        piecesSequences[pieceIndex] = 0;
        startDrawLineFlag = true;
    }

    public void LinkNode(Vector3 point,int pieceIndex)
    {
        if (piecesSequences.ContainsKey(pieceIndex))
        {
            CancleLinkNode(pieceIndex);
            Debug.Log("取消链接");
            return;
        }
        Debug.Log("连接");
        pointCount++;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount - 2, point);
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

    public void ResetLine()
    {
        lineRenderer.positionCount = 0;
        pointCount = 0;
        startDrawLineFlag = false;
    }

    /// <summary>
    /// Dijkstra算法算长度最大值
    /// </summary>
    /// <param name="startPoint"> 起始点</param>
    /// <param name="endPoint"> 终点</param>
    /// <param name="points"> 需要经过计算的节点</param>
    /// <returns></returns>
    public float SumLongestPath(Vector2 startPoint, Vector2 endPoint, List<Vector2> points)
    {
        //计算距离作为权重
        Dictionary<int, float[]> pathWeights = new Dictionary<int, float[]>();
        List<Vector2> nodes = new List<Vector2>();
        nodes.Add(startPoint);
        nodes.AddRange(points);
        nodes.Add(endPoint);
        for (int i = 0; i < nodes.Count; i++)
        {
            pathWeights[i] = new float[nodes.Count];
        }
        for (int i = 0; i < nodes.Count; i++)
        {   
            for (int j = i + 1; j < nodes.Count; j++)
            {
                float distance = Vector2.Distance(nodes[i], nodes[j]);
                pathWeights[i][j] = distance;
                pathWeights[j][i] = distance;
            }
        }
        //防止直接从起始点跳到终点 
        pathWeights[0][nodes.Count - 1] = 0;
        //初始化计算数组
        int[] path = new int[nodes.Count];//用于记录路径
        float[] pathWeight = new float[nodes.Count];//用于更新距离
        bool[] flag = new bool[nodes.Count];
        for (int i = 0; i < nodes.Count; i++)
        {
            flag[i] = false;
            pathWeight[i] = pathWeights[0][i];
            path[i] = 0;
        }
        flag[0] = true;
        //开始计算
        int calculateCount = 0;
        while (true)
        {
            //避免死循环
            calculateCount++;
            if (calculateCount > nodes.Count)
            {
                Debug.Log("计算有误");
                break;
            }
            float maxDistance = float.MinValue;
            int maxIndex = -1;
            //找到当前权重最大的节点 记录距离以及其节点号
            for (int j = 0; j < pathWeight.Length; j++)
            {
                if (flag[j] == false && pathWeight[j] > maxDistance)
                {
                    maxDistance = pathWeight[j];
                    maxIndex = j;
                }
            }
            if (maxIndex == -1)
            {
                break;
            }
            //更新标记
            flag[maxIndex] = true;
            //更新权重数组
            for (int k = 0; k < nodes.Count(); k++)
            {
                if (pathWeights[k][maxIndex] + pathWeight[maxIndex] > pathWeights[k][0] && flag[k] == false)
                {
                    pathWeight[k] = pathWeights[k][maxIndex] + pathWeight[maxIndex];
                    path[k] = maxIndex;
                }
            }
        }
        Debug.Log("最大距离:"+ pathWeight[^1]);
        return pathWeight[^1];
    }
}
