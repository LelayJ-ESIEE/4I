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
        ComputeVector3FromKxKz lambda;
        if (CompareTag("Plane"))
            lambda = (kX, kZ) => new Vector3(kX, 0, kZ);
        else if (CompareTag("Cylinder"))
            lambda = (kX, kZ) => {
                float theta = kX * 2 * Mathf.PI;
                float y = 4 * kZ;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta), y, rho * Mathf.Sin(theta));
            };
        else if (CompareTag("InnerSphere"))
            lambda = (kX, kZ) => {
                float rho = 2;
                float theta = kX * 2 * Mathf.PI;
                float phi = kZ * Mathf.PI;
                return rho * new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi));
            };
        else if (CompareTag("Sphere"))
            lambda = (kX, kZ) => {
                float rho = 2;
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                return rho * new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi));
            };
        else if (CompareTag("Ring"))
            lambda = (kX, kZ) => {
                float theta = kX * 2 * Mathf.PI;
                float rho = (2 - kZ) * Mathf.PI / 2;
                return new Vector3(rho * Mathf.Cos(theta), 0, rho * Mathf.Sin(theta));
            };
        else if (CompareTag("Cube"))
            m_Mf.sharedMesh = createCube(1);
        else if (CompareTag("Chips"))
            m_Mf.sharedMesh = createChips(1);
        else
                    lambda = (kX, kZ) => {
                        return new Vector3(kX, 0, kZ);
                    };

        // m_Mf.sharedMesh = WrapNormalizedPlane(50, 50, lambda);

        // CreateStripXZQuads

        //m_Mf.sharedMesh = CreateStripXZQuads(new Vector3(4, 0, 2), 2);

        // WrapNormalizePlaneQuads

        // m_Mf.sharedMesh = WrapNormalizePlaneQuads(50, 50, lambda);

        // CreateRegularPolygonXZQuads

        // m_Mf.sharedMesh = CreateRegularPolygonXZQuads(5, 18);
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

    Mesh CreateStripXZQuads(Vector3 size, int nSegments)
    {
        Vector3 halfSize = size * .5f;

        Mesh newMesh = new Mesh();
        newMesh.name = "stripQuads";

        Vector3[] vertices = new Vector3[(nSegments + 1) * 2];
        int[] quads = new int[nSegments * 4];

        //Vertices
        for (int i = 0; i < nSegments + 1; i++)
        {
            float k = (float)i / nSegments;
            float y = .25f * Mathf.Sin(k * Mathf.PI * 2 * 3);
            vertices[i] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), y, -halfSize.z);
            vertices[nSegments + 1 + i] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), y, halfSize.z);
        }

        //Triangles
        int index = 0;
        for (int i = 0; i < nSegments; i++)
        {
            quads[index++] = i;
            quads[index++] = i + nSegments + 1;
            quads[index++] = i + nSegments + 2;
            quads[index++] = i + 1;
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh WrapNormalizePlaneQuads(int nSegmentsX, int nSegmentsZ, ComputeVector3FromKxKz computePosition)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "plane";
        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] quads = new int[nSegmentsX * nSegmentsZ * 4];
        //Vertices
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

        //Quads
        index = 0;
        //double boucle également
        for (int i = 0; i < nSegmentsX; i++)
        {
            for (int j = 0; j < nSegmentsZ; j++)
            {
                quads[index++] = i * (nSegmentsZ + 1) + j;
                quads[index++] = i * (nSegmentsZ + 1) + j + 1;
                quads[index++] = (i + 1) * (nSegmentsZ + 1) + j + 1;
                quads[index++] = (i + 1) * (nSegmentsZ + 1) + j;
            }
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh CreateRegularPolygonXZQuads(float radius, int nQuads)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "RegularPolygonQuads";

        Vector3[] vertices = new Vector3[nQuads * 2 + 1];
        int[] quads = new int[nQuads * 4];

        vertices[0] = Vector3.zero;
        int index = 1;

        float angStep = 360 / nQuads;
        //Vertices
        for (int i = 0; i < nQuads; i++)
        {
            Quaternion ang = Quaternion.Euler(0, angStep * i, 0);
            Vector3 newVertice = ang * Vector3.right * radius;

            if(i > 0)
                vertices[index++] = Vector3.Lerp(newVertice, vertices[index - 2], 0.5f);
            vertices[index++] = newVertice;
        }
        vertices[index++] = Vector3.Lerp(vertices[index - 2], vertices[1], .5f);

        //Quads
        for (int i = 0; i < nQuads; i++)
        {
            int offset = i * 4;
            quads[offset] = 0;
            if (i == 0)
            {
                quads[offset + 1] = index - 1;
                quads[offset + 2] = 1;
                quads[offset + 3] = 2;
            }
            else
            {
                quads[offset + 1] = i * 2;
                quads[offset + 2] = i * 2 + 1;
                quads[offset + 3] = i * 2 + 2;
            }
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh createCube(int size)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "Cube";
        Vector3[] vertices = new Vector3[8];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(size, 0, 0);
        vertices[2] = new Vector3(size, 0, size);
        vertices[3] = new Vector3(0, 0, size);
        vertices[4] = new Vector3(0, size, 0);
        vertices[5] = new Vector3(size, size, 0);
        vertices[6] = new Vector3(size, size, size);
        vertices[7] = new Vector3(0, size, size);

        int nbQuads = 6;
        int[] quads = new int[nbQuads * 4];
        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        quads[4] = 4;
        quads[5] = 5;
        quads[6] = 1;
        quads[7] = 0;

        quads[8] = 5;
        quads[9] = 6;
        quads[10] = 2;
        quads[11] = 1;

        quads[12] = 6;
        quads[13] = 7;
        quads[14] = 3;
        quads[15] = 2;

        quads[16] = 7;
        quads[17] = 4;
        quads[18] = 0;
        quads[19] = 3;

        quads[20] = 7;
        quads[21] = 6;
        quads[22] = 5;
        quads[23] = 4;

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh createChips(int size)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "Chips";
        Vector3[] vertices = new Vector3[8];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(size, 0, 0);
        vertices[2] = new Vector3(size, 0, size);
        vertices[3] = new Vector3(0, 0, size);
        vertices[4] = new Vector3(0, size, 0);
        vertices[5] = new Vector3(size, size, 0);
        vertices[6] = new Vector3(size, size, size);
        vertices[7] = new Vector3(0, size, size);

        int nbQuads = 3;
        int[] quads = new int[nbQuads * 4];
        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        quads[4] = 4;
        quads[5] = 5;
        quads[6] = 1;
        quads[7] = 0;

        quads[8] = 7;
        quads[9] = 6;
        quads[10] = 5;
        quads[11] = 4;

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}