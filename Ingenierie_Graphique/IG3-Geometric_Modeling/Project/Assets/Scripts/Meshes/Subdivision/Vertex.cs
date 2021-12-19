using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public int m_index;
    public Vector3 m_position;
    public List<HalfEdge> m_adjacentEdges;

    public Vertex(int index, Vector3 position)
    {
        m_index = index;
        m_position = position;
        m_adjacentEdges = new List<HalfEdge>();
    }

    public Vertex(int index, float x, float y, float z) : this(index, new Vector3(x, y, z)) { }

    public int getValence() { return m_adjacentEdges.Count; }

    public int getNbAdjacentFaces()
    {
        int res = 0;
        foreach (HalfEdge he in m_adjacentEdges)
        {
            if (he.m_sourceVertex == this)
            {
                res++;
            }
        }
        return res;
    }
}
