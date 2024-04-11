using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    [SerializeField] private Rigidbody2D rb;

    public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public int pointsCount = 0;

    float pointsMinDistance = 0.1f;
    float circleColliderRadius;
    
    public void AddPoint( Vector2 newPoint)
    {
        if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;

        // add circle collider to point
        CircleCollider2D circleCollider2D = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.offset = newPoint;
        circleCollider2D.radius = circleColliderRadius;


        // line renderer
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);

        // edge collider
        if (points.Count > 1)
            edgeCollider.points = points.ToArray();
    }
    public Vector2 GetLastPoint()
    {
        return (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount - 1);
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
    //private void Update()
    //{
    //    Debug.Log(rb.velocity.magnitude/Time.deltaTime); 
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Grid"))
    //    {
    //        this.rb.velocity = Vector2.zero;
    //    }
    //}
}
