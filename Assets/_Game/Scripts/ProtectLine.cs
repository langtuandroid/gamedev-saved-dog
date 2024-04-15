using System.Collections.Generic;
using UnityEngine;

public class ProtectLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    [SerializeField] private Rigidbody2D rb;

    public List<Vector2> points = new List<Vector2>();

    private int pointsCount = 0;
    private float pointsMinDistance = 0.1f;
    private float circleColliderRadius;

    public int PointsCount => pointsCount;

    public void AddPoint( Vector2 newPoint)
    {
        if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;
        
        CircleCollider2D circleCollider2D = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.offset = newPoint;
        circleCollider2D.radius = circleColliderRadius;


        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);

        if (points.Count > 1)
            edgeCollider.points = points.ToArray();
    }
    public Vector2 GetLastPoint()
    {
        return lineRenderer.GetPosition(lineRenderer.positionCount - 1);
    }

    public void UsePhysics(bool usePhysics)
    {
        rb.isKinematic = !usePhysics;
    }
    
    public void SetLineColor(Gradient colorGradient)
    {
        lineRenderer.colorGradient = colorGradient;
    }
    
    public void SetPointMinDistance(float distance)
    {
        pointsMinDistance = distance;
    }
    
    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        circleColliderRadius = width / 1.5f;
        edgeCollider.edgeRadius = circleColliderRadius;
    }
}
