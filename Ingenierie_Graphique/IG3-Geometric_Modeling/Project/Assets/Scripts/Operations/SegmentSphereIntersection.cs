using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSphereIntersection : MonoBehaviour
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

    bool interSegmentSphere(Segment seg, Sphere sphere, out Vector3 interPt, out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.p2 - seg.p1;
        Vector3 OmA = seg.p1 - sphere.center;

        if(Mathf.Approximately(Vector3.Dot(OmA, AB), 0))
            return false;

        float a, b, c, d;
        a = Vector3.Dot(AB, AB);
        b = 2 * Vector3.Dot(OmA, AB);
        c = Vector3.Dot(OmA, OmA) - Mathf.Pow(sphere.radius, 2);
        d = b * b - 4 * a * c;
        if (d < 0)
        {
            return false;
        }

        float t = (-b - Mathf.Sqrt(d)) / (2 * a);
        interPt = seg.p1 + t * AB;
        interNormal = (interPt - sphere.center).normalized;
        return true;
    }

    void OnDrawGizmos()
    {
        Segment seg = new Segment(A, B);
        Sphere sphere = new Sphere(m_Transform.position, m_Transform.localScale.z / 2);

        bool intersecting = interSegmentSphere(seg, sphere, out Vector3 interP, out Vector3 interN);

        if (intersecting)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(interP, 0.1f);
            Debug.DrawLine(interP, interP + interN, Color.blue);
        }
    }
}
