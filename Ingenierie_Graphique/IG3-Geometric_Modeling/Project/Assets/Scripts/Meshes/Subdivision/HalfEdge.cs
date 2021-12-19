using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdge
{
    public int m_index;

    public Face m_face;
    public Vertex m_sourceVertex;

    public HalfEdge m_prevEdge;
    public HalfEdge m_nextEdge;
    public HalfEdge m_twinEdge;

    public HalfEdge(int index, Face face, Vertex sourceVertex, HalfEdge prevEdge, HalfEdge nextEdge, HalfEdge twinEdge)
    {
        m_index = index;
        m_face = face;
        m_sourceVertex = sourceVertex;
        m_prevEdge = prevEdge;
        m_nextEdge = nextEdge;
        m_twinEdge = twinEdge;
    }

    public HalfEdge(int index, Face face, Vertex sourceVertex) : this(index, face, sourceVertex, null, null, null) {; }
}
