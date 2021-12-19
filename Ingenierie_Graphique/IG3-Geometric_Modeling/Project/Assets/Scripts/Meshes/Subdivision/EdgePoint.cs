using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePoint
{
    public int m_index;
    public Vector3 m_position;
    public HalfEdge m_edge;
    public HalfEdge m_twinEdge;

    public EdgePoint(int index, Vector3 position, HalfEdge edge, HalfEdge twinEdge)
    {
        m_index = index;
        m_position = position;
        m_edge = edge;
        m_twinEdge = twinEdge;
    }

}
