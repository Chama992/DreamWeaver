using UnityEditor;
using UnityEngine;

public static class GizmosTool
{
    /// <summary>
    /// 绘制默认颜色线条
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        Gizmos.DrawLine(start,end);
    }
    /// <summary>
    /// 绘制带颜色线条
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color"></param>
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawLine(start,end);
        Gizmos.color  = defaultColor;
    }
    /// <summary>
    /// 绘制正方形
    /// </summary>
    /// <param name="centerPoint">正中心</param>
    /// <param name="width">宽度</param>
    /// <param name="height">长度</param>
    /// <param name="color">颜色</param>
    public static void DrawSquare(Vector3 centerPoint, float width, float height, Color color)
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = color;
        Vector3 leftStart = centerPoint + Vector3.left * width / 2 - Vector3.up * height/2;
        Vector3 rightStart = centerPoint + Vector3.right * width / 2 - Vector3.up * height/2;
        Vector3 downLeftStart = leftStart + Vector3.down * height;
        Vector3 downRightStart = rightStart + Vector3.down * height;
        Gizmos.DrawLine(leftStart,rightStart);
        Gizmos.DrawLine(leftStart,downLeftStart);
        Gizmos.DrawLine(rightStart,downRightStart);
        Gizmos.DrawLine(downLeftStart,downRightStart);
        Gizmos.color  = defaultColor;
    }
    public static void DrawCircle(Vector3 centerPoint, float radius,int vertexCount = 50)
    {
        Color defaultColor = Gizmos.color;
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        Vector3 oldPos = centerPoint;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, centerPoint + pos);
            oldPos = centerPoint + pos;
            theta += deltaTheta;
        }
        Gizmos.color  = defaultColor;
    }
    public static void DrawCircle(Vector3 centerPoint, float radius,Color color,int vertexCount = 50)
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = color;
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        Vector3 oldPos = centerPoint;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, (Vector3)centerPoint + pos);
            oldPos = centerPoint + pos;
            theta += deltaTheta;
        }
        Gizmos.color  = defaultColor;
    }
    public static void DrawGroundCheck(Vector3 groundCheckPoint, float groundCheckWidth,float groundCheckHeight, Color color)
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = color;
        Vector3 leftStart = groundCheckPoint+Vector3.up * groundCheckHeight / 2 + Vector3.left * groundCheckWidth / 2;
        Vector3 rightStart = groundCheckPoint + Vector3.up * groundCheckHeight / 2 +  Vector3.right * groundCheckWidth / 2;
        Vector3 downLeftStart = leftStart + Vector3.down * groundCheckHeight;
        Vector3 downRightStart = rightStart + Vector3.down * groundCheckHeight;
        DrawLine(leftStart,downLeftStart);
        DrawLine(rightStart,downRightStart); 
        DrawLine(downLeftStart,downRightStart);
        DrawLine(leftStart,rightStart);
        Gizmos.color  = defaultColor;
    }
}