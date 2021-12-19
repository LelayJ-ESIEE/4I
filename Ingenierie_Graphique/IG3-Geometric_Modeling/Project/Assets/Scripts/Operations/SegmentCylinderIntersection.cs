using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentCylinderIntersection : MonoBehaviour
{
    [Header("Segment points (Absolute)")]
    [SerializeField] Vector3 A;
    [SerializeField] Vector3 B;

    Transform m_Transform;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(A, B, Color.red);
    }

    bool interSegmentCylinder(Segment seg, Cylinder cylinder, out Vector3 interPt, out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.p2 - seg.p1;

        Vector3 PQ = cylinder.p2 - cylinder.p1;
        Vector3 uPQ = PQ.normalized;
        Vector3 PA = seg.p1 - cylinder.p1;

        // Intesection equation :
        float a, b, c, d;
        a = Vector3.Dot(AB, AB) - Mathf.Pow(Vector3.Dot(AB, uPQ), 2);
        b = 2 * (Vector3.Dot(PA, AB) - Vector3.Dot(AB, uPQ) * Vector3.Dot(PA, uPQ));
        c = Vector3.Dot(PA, PA) - Mathf.Pow(Vector3.Dot(PA, uPQ), 2) - Mathf.Pow(cylinder.radius, 2);
        d = b * b - 4 * a * c;
        
        if (d < 0)
        {
            return false;
        }
        
        float t = (-b - Mathf.Sqrt(d)) / (2 * a);
        interPt = seg.p1 + t * AB;

        interNormal = Vector3.ProjectOnPlane(interPt - cylinder.p1, uPQ).normalized;
        return true;
    }

    void OnDrawGizmos()
    {
        Segment seg = new Segment(A, B);
        Cylinder cylinder = new Cylinder(m_Transform.position + m_Transform.up * 100, m_Transform.position - m_Transform.up * 100, m_Transform.localScale.x/2);

        bool intersecting = interSegmentCylinder(seg, cylinder, out Vector3 interP, out Vector3 interN);

        if (intersecting)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(interP, 0.1f);
            Debug.DrawLine(interP, interP + interN, Color.blue);
        }
    }
}
