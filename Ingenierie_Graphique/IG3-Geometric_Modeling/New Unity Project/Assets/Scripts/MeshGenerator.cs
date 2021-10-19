using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : MonoBehaviour
{
    private MeshFilter m_Mf;

    // Start is called before the first frame update
    void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
        // m_Mf.sharedMesh = CreateTriangle();
        // m_Mf.sharedMesh = CreateQuadXZ(new Vector3(4, 0, 2));
        // m_Mf.sharedMesh = CreateStripXZ(new Vector3(4, 0, 2), 4);
        m_Mf.sharedMesh = CreatePlaneXZ(new Vector3(4, 0, 2), 8, 2);
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
        // Crée un quad centré sur l'origine selon les axes X,Z
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
        // Crée un quad centré sur l'origine selon les axes X,Z
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

    Mesh CreatePlaneXZ(Vector3 size, int nSegmentsX, int nSegmentsZ) // TODO
    {
        // Crée un quad centré sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;

        Mesh newMesh = new Mesh();
        newMesh.name = "strip";

        //Remplissage vertices
        Vector3[] vertices = new Vector3[(nSegments + 1) * 2];
        for (int i = 0; i < nSegments + 1; i++)
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
            triangles[index++] = i + 1;
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }
}