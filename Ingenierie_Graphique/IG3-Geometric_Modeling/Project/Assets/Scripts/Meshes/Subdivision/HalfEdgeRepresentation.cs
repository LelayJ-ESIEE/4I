using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdgeRepresentation
{
    private Mesh m_Mesh;

    public List<Face> m_Faces;
    public List<Vertex> m_Vertices;
    public List<HalfEdge> m_Edges;

    public HalfEdgeRepresentation()
    {
        this.m_Faces = new List<Face>();
        this.m_Vertices = new List<Vertex>();
        this.m_Edges = new List<HalfEdge>();
    }

    public HalfEdgeRepresentation(Mesh mesh)
    {
        this.m_Mesh = mesh;
        this.m_Faces = new List<Face>();
        this.m_Vertices = new List<Vertex>();
        this.m_Edges = new List<HalfEdge>();
        InitializeFromMesh();
    }

    private void InitializeFromMesh()
    {
        int[] quads = m_Mesh.GetIndices(0);
        int nQuads = quads.Length / 4;
        int indexV = 0;
        foreach (Vector3 vertex in m_Mesh.vertices) // Generate all Vertices
        {
            m_Vertices.Add(new Vertex(indexV, vertex));
            indexV++;
        }
        int indexEdge = 0;
        indexV = 0;
        int i;
        for (i = 0; i < nQuads; i++) // Generate all Faces and HalfEdges without twinEdge
        {
            Face newFace = new Face(i);
            m_Faces.Add(newFace);
            HalfEdge edge1 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].m_adjacentEdges.Add(edge1);
            HalfEdge edge2 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].m_adjacentEdges.Add(edge2);
            HalfEdge edge3 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].m_adjacentEdges.Add(edge3);
            HalfEdge edge4 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].m_adjacentEdges.Add(edge4);
            m_Edges.Add(edge1);
            m_Edges.Add(edge2);
            m_Edges.Add(edge3);
            m_Edges.Add(edge4);

            newFace.m_side = edge1;

            edge1.m_nextEdge = edge2;
            edge1.m_prevEdge = edge4;

            edge2.m_nextEdge = edge3;
            edge2.m_prevEdge = edge1;

            edge3.m_nextEdge = edge4;
            edge3.m_prevEdge = edge2;

            edge4.m_nextEdge = edge1;
            edge4.m_prevEdge = edge3;
        }

        // Add twinEdges
        List<HalfEdge> edgesToFill = new List<HalfEdge>(m_Edges);
        while (edgesToFill.Count != 0)
        {
            HalfEdge edgeToFill = edgesToFill[0];
            HalfEdge twinEdge = null;
            foreach (HalfEdge edge in edgesToFill)
            {
                if (IsEdgeTwin(edgeToFill, edge))
                {
                    twinEdge = edge;
                    break;
                }
            }
            edgeToFill.m_twinEdge = twinEdge;
            edgesToFill.Remove(edgeToFill);
            if (twinEdge != null) // if found twinEdge, add actual edge as twinEdge and remove from list
            {
                twinEdge.m_twinEdge = edgeToFill;
                edgesToFill.Remove(twinEdge);
            }
        }

        // Add edge to nextEdge adjacentEges for edges lacking twinEdge
        foreach (HalfEdge he in m_Edges)
        {
            if (he.m_twinEdge == null)
            {
                he.m_nextEdge.m_sourceVertex.m_adjacentEdges.Add(he);
            }
        }
    }

    private bool IsEdgeTwin(HalfEdge edge1, HalfEdge edge2)
    {
        return edge1.m_sourceVertex == edge2.m_nextEdge.m_sourceVertex && edge1.m_nextEdge.m_sourceVertex == edge2.m_sourceVertex;
    }

    public Mesh getMeshVertexFaces()
    {
        Mesh newMesh = new Mesh();

        Vector3[] vertices = new Vector3[m_Vertices.Count];
        int[] quads = new int[m_Faces.Count * 4];

        foreach (Vertex v in m_Vertices)
        {
            vertices[v.m_index] = v.m_position;
        }

        int idQuad = 0;
        foreach (Face f in m_Faces)
        {
            int offset = 0;
            HalfEdge he = f.m_side.m_prevEdge;
            do
            {
                quads[idQuad * 4 + offset] = he.m_sourceVertex.m_index;
                offset++;
                he = he.m_nextEdge;
            }
            while (f.m_side.m_prevEdge != he);
            idQuad++;
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}
