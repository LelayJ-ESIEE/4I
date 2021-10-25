using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : MonoBehaviour
{
    delegate Vector3 ComputeVector3FromKxKz(float kX, float kZ);
    private MeshFilter m_Mf;

    // Start is called before the first frame update
    void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
        
        // Explicits
        // m_Mf.sharedMesh = CreateTriangle();
        // m_Mf.sharedMesh = CreateQuadXZ(new Vector3(4, 0, 2));
        // m_Mf.sharedMesh = CreateStripXZ(new Vector3(4, 0, 2), 4);
        // m_Mf.sharedMesh = CreatePlane(new Vector3(4, 0, 2), 8, 2);
        
        // Normalized Wraped Planes
        m_Mf.sharedMesh = WrapNormalizedPlane(200, 100,
            // Plane
            //(kX, kZ) => new Vector3(kX, 0, kZ)

            // Cylinder
            //(kX, kZ) => {
            //    float theta = kX * 2 * Mathf.PI;
            //    float y = 4 * kZ;
            //    float rho = 2;
            //    return new Vector3(rho * Mathf.Cos(theta), y, rho * Mathf.Sin(theta));
            //}

            // Inner Sphere
            //(kX, kZ) =>
            //{
            //    float rho = 2;
            //    float theta = kX * 2 * Mathf.PI;
            //    float phi = kZ * Mathf.PI;
            //    return rho * new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi));
            //}

            // Sphere
            (kX, kZ) =>
            {
                float rho = 2;
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                return rho * new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi));
            }

            // Ring

            // Helix

            // Funnel Helix

            // Closed Cylinder

            // Torus

            // Unregular Rorus

            // Radial Ripple

            // Inverted Fir

            // Crenellated Inverted Fir

            // Paraboloid

            // Closed Star-based Prism

            // Tube

            // Egg
        );
    }

    Mesh CreateTriangle()
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "triangle";

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[1 * 3];

        vertices[0] = Vector3.right;
        vertices[1] = Vector3.up;
        vertices[2] = Vector3.forward;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }

    Mesh CreateQuadXZ(Vector3 size)
    {
        // Cr�e un quad centr� sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;

        Mesh newMesh = new Mesh();
        newMesh.name = "quad";

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[2 * 3];

        vertices[0] = new Vector3(-halfSize.x, 0, -halfSize.z);
        vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z);
        vertices[2] = new Vector3(halfSize.x, 0, halfSize.z);
        vertices[3] = new Vector3(halfSize.x, 0, -halfSize.z);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }

    Mesh CreateStripXZ(Vector3 size, int nSegments)
    {
        // Cr�e un quad centr� sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;

        Mesh newMesh = new Mesh();
        newMesh.name = "strip";

        //Remplissage vertices
        Vector3[] vertices = new Vector3[(nSegments + 1) * 2];
        for (int i = 0; i < nSegments+1; i++)
        {
            float k = (float)i / nSegments;
            vertices[i] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), 0, -halfSize.z);
            vertices[i + nSegments + 1] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), 0, halfSize.z);
        }

        //Triangles
        int[] triangles = new int[nSegments * 2 * 3];
        int index = 0;
        for (int i = 0; i < nSegments; i++)
        {
            triangles[index++] = i;
            triangles[index++] = i + nSegments + 1;
            triangles[index++] = i + nSegments + 2;

            triangles[index++] = i;
            triangles[index++] = i + nSegments + 2;
            triangles[index++] = i+1;
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }

    Mesh CreatePlane(Vector3 size, int nSegmentsX, int nSegmentsZ)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "plane";

        Vector3 halfSize = size * .5f;

        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 2 * 3];

        // VERTICES
        int index = 0;
        for (int i = 0; i < nSegmentsX + 1; i++)
        {
            float kX = (float)i / nSegmentsX;
            for (int j = 0; j < nSegmentsZ + 1; j++)
            {
                float kZ = (float)j / nSegmentsZ;
                vertices[index++] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, kX), 0, Mathf.Lerp(-halfSize.z, halfSize.z, kZ));
            }
        }

        //TRIANGLES
        index = 0;
        int offset = 0;
        for (int i = 0; i < nSegmentsX; i++)
        {
            for (int j = 0; j < nSegmentsZ; j++)
            {
                triangles[index++] = offset + j;
                triangles[index++] = offset + j + 1;
                triangles[index++] = offset + j + 1 + nSegmentsZ + 1;

                triangles[index++] = offset + j;
                triangles[index++] = offset + j + 1 + nSegmentsZ + 1;
                triangles[index++] = offset + j + nSegmentsZ + 1;
            }
            offset += (nSegmentsZ + 1);
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh WrapNormalizedPlane(int nSegmentsX, int nSegmentsZ, ComputeVector3FromKxKz computePosition)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "wrappedNormalizedPlane";

        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 2 * 3];

        // VERTICES
        int index = 0;
        for (int i = 0; i < nSegmentsX + 1; i++)
        {
            float kX = (float)i / nSegmentsX;
            for (int j = 0; j < nSegmentsZ + 1; j++)
            {
                float kZ = (float)j / nSegmentsZ;
                vertices[index++] = computePosition(kX, kZ);
            }
        }

        //TRIANGLES
        index = 0;
        int offset = 0;
        for (int i = 0; i < nSegmentsX; i++)
        {
            for (int j = 0; j < nSegmentsZ; j++)
            {
                triangles[index++] = offset + j;
                triangles[index++] = offset + j + 1;
                triangles[index++] = offset + j + 1 + nSegmentsZ + 1;

                triangles[index++] = offset + j;
                triangles[index++] = offset + j + 1 + nSegmentsZ + 1;
                triangles[index++] = offset + j + nSegmentsZ + 1;
            }
            offset += (nSegmentsZ + 1);
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}