using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentPlaneIntersection : MonoBehaviour
{
    [Header("Segment points")]
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
        Segment seg = new Segment(A, B);
        Plane plane = new Plane(m_Transform.up, m_Transform.position);
        bool intersecting = interSegmentPlane(seg, plane, interPt, interNormal);

        Debug.DrawLine(A, B, Color.red);

        if (intersecting)
        {
            Debug.Log("Intersection between line and plane at " + interPt);
        }
    }

    bool interSegmentPlane(Segment seg, Plane plane, out Vector3 interPt, out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.p2 - seg.p1;
        float dotABn = Vector3.Dot(AB, plane.normal);

        if (Mathf.Approximately(dotABn, 0))
            return false;

        float t = (plane.d - Vector3.Dot(seg.p1, plane.normal)) / dotABn;
        if (t < 0 || t > 1)
            return false;

        interPt = seg.p1 + t * AB;
        interNormal = (dotABn < 0) ? plane.normal : -(plane.normal);
        return true;
    }
}
