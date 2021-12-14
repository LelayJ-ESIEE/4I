using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder
{
    public Vector3 p1, p2;
    public float radius;

    public Cylinder(Vector3 p1, Vector3 p2, float radius)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.radius = radius;
    }
}
