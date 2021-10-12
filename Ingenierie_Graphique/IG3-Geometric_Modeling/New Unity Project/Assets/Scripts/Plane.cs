using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    Vector3 normal;
    float d;

    public Plane(Vector3 normal, float d)
    {
        this.normal = normal;
        this.d = d;
    }

    public Plane(Vector3 normal, Vector3 p)
    {
        Plane(normal, Vector3.Dot(normal, p));
    }

    public Plane(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Plane(Vector3.Cross(p2-p1, p3-p1).normalized, p1);
    }
}
