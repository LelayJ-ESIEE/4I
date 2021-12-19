using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    public int m_index;
    public HalfEdge m_side;

    public Face(int index, HalfEdge side)
    {
        m_index = index;
        m_side = side;
    }

    public Face(int index) : this(index, null) { }

    public int getNbVertices()
    {
        int nbVertices = 0;
        HalfEdge he = m_side;
        do
        {
            nbVertices++;
            he = he.m_nextEdge;
        }
        while (m_side != he);
        return nbVertices;
    }
}
