using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    public Vector3 normal;
    public float d;

    public Plane(Vector3 normal, float d)
    {
        this.normal = normal;
        this.d = d;
    }

    public Plane(Vector3 normal, Vector3 p) : this(normal, Vector3.Dot(normal, p)){}

    public Plane(Vector3 p1, Vector3 p2, Vector3 p3) : this(Vector3.Cross(p2-p1, p3-p1).normalized, p1){}
}
