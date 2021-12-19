using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class CatmullClark : MonoBehaviour
{
    [Header("CatmullClark")]
    [SerializeField] int m_NumberOfIterations;

    MeshFilter m_Mf;
    Mesh m_BaseMesh;
    Mesh m_Mesh;

    HalfEdgeRepresentation m_HalfEdgeRepresentation;

    public List<Vector3> m_FacePoints = new List<Vector3>();
    public List<Vector3> m_MidPoints = new List<Vector3>();
    public List<EdgePoint> m_EdgePoints = new List<EdgePoint>();

    // Start is called before the first frame update
    void Start()
    {
        m_Mf = GetComponent<MeshFilter>();
        m_BaseMesh = m_Mf.sharedMesh;
        m_Mesh = Mesh.Instantiate(m_BaseMesh);

        m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);

        for (int i = 0; i < m_NumberOfIterations; i++)
        {
            m_Mesh = m_HalfEdgeRepresentation.getMeshVertexFaces();
            m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);
            Iterate(m_HalfEdgeRepresentation);
        }

        m_Mesh = m_HalfEdgeRepresentation.getMeshVertexFaces();
        m_Mf.sharedMesh = m_Mesh;
    }

    void Iterate(HalfEdgeRepresentation state) {
        // Create new points : FacePoints and EdgePoints
        InitializePoints(state);
        // Update Vertices position
        foreach (Vertex v in state.m_Vertices)
            UpdateVerticePosition(v);
        // For each EdgePoint, split Edges
        foreach (EdgePoint ep in m_EdgePoints)
            SplitOnEdgePoint(state, ep);
        // Split Faces
        List<Face> faces= new List<Face>(state.m_Faces);
        foreach (Face face in faces)
        {
            Vector3 facePoint = m_FacePoints[face.m_index];
            SplitFace(state, face, facePoint);
        }
    }

    void InitializePoints(HalfEdgeRepresentation state)
    {
        InitializeFaceMidPoints(state);
        InitializeEdgePoints(state);
    }

    void InitializeFaceMidPoints(HalfEdgeRepresentation state) {
        m_FacePoints.Clear();
        m_MidPoints.Clear();

        foreach (Face f in state.m_Faces)
        {
            Vector3 facePoint = Vector3.zero;
            HalfEdge he = f.m_side;
            do
            {
                m_MidPoints.Add(Vector3.Lerp(he.m_sourceVertex.m_position, he.m_nextEdge.m_sourceVertex.m_position, 0.5f));
                facePoint += he.m_sourceVertex.m_position;
                he = he.m_nextEdge;
            }
            while (f.m_side != he);
            facePoint = facePoint / 4;
            m_FacePoints.Add(facePoint);
        }

    }

    void InitializeEdgePoints(HalfEdgeRepresentation state) {
        m_EdgePoints.Clear();
        HashSet<HalfEdge> heDone = new HashSet<HalfEdge>();
        int idEdgePoint = 0;
        foreach (Face f in state.m_Faces)
        {
            HalfEdge he = f.m_side;
            do
            {
                if (!heDone.Contains(he))
                {
                    // Compute EdgePoint = edge verticies and facePoints mean
                    Vector3 edgePt;
                    if (he.m_twinEdge != null)
                    {
                        edgePt = (he.m_sourceVertex.m_position + he.m_nextEdge.m_sourceVertex.m_position + m_FacePoints[f.m_index] + m_FacePoints[he.m_twinEdge.m_face.m_index]) / 4;
                        heDone.Add(he.m_twinEdge);
                    }
                    else
                    {
                        edgePt = m_MidPoints[he.m_index];
                    }
                    heDone.Add(he);
                    m_EdgePoints.Add(new EdgePoint(idEdgePoint++, edgePt, he, he.m_twinEdge));
                }
                he = he.m_nextEdge;
            }
            while (f.m_side != he);
        }
    }

    void UpdateVerticePosition(Vertex vertex)
    {
        float n = vertex.getValence();
        if(n == vertex.getNbAdjacentFaces())
        {
            Vector3 Q = FacePointsMean(vertex);
            Vector3 R = MidPointsMean(vertex);
            Vector3 newPos = (Q / n) + ((2 * R) / n) + ((n - 3) / n) * vertex.m_position;
            vertex.m_position = newPos;
        }
    }

    Vector3 FacePointsMean(Vertex vertex)
    {
        Vector3 res = Vector3.zero;
        foreach (HalfEdge he in vertex.m_adjacentEdges)
        {
            if (he.m_sourceVertex == vertex) 
            {
                res += m_FacePoints[he.m_face.m_index];
            }
        }
        return res / vertex.getNbAdjacentFaces();
    }

    Vector3 MidPointsMean(Vertex vertex)
    {
        Vector3 res = Vector3.zero;
        foreach (HalfEdge he in vertex.m_adjacentEdges)
        {
            res += m_MidPoints[he.m_index];
        }
        return res / vertex.m_adjacentEdges.Count;
    }

    void SplitOnEdgePoint(HalfEdgeRepresentation state, EdgePoint ep)
    {
        Vertex epVertex = new Vertex(state.m_Vertices.Count, ep.m_position);
        state.m_Vertices.Add(epVertex);

        int index = state.m_Edges.Count;
        HalfEdge he1 = SplitEdge(state, index++, ep.m_edge, epVertex);
        if (ep.m_twinEdge != null)
        {
            HalfEdge he2 = SplitEdge(state, index++, ep.m_twinEdge, epVertex);

            he1.m_twinEdge = ep.m_twinEdge;
            he2.m_twinEdge = ep.m_edge;

            ep.m_twinEdge.m_twinEdge = he1;
            ep.m_twinEdge = he2;
        }
        else
        {
            epVertex.m_adjacentEdges.Add(ep.m_edge);
        }
    }

    HalfEdge SplitEdge(HalfEdgeRepresentation state, int index, HalfEdge edge, Vertex vertex)
    {
        HalfEdge he = new HalfEdge(index, edge.m_face, vertex);
        state.m_Edges.Add(he);
        vertex.m_adjacentEdges.Add(he);

        he.m_nextEdge = edge.m_nextEdge;
        he.m_prevEdge = edge;

        edge.m_nextEdge = he;

        return he;
    }

    void SplitFace(HalfEdgeRepresentation state, Face face, Vector3 facePoint)
    {
        int nbVertices = face.getNbVertices();
        if(nbVertices%2 == 0)
        {
            int indexFaces = state.m_Faces.Count;
            int indexHalfEdges = state.m_Edges.Count;

            Vertex facePointVertex = new Vertex(state.m_Vertices.Count, facePoint);
            state.m_Vertices.Add(facePointVertex);

            // Saving current edges state : 1st HEs of the current face and the next face
            HalfEdge firstHe = face.m_side.m_nextEdge;
            HalfEdge heNext = firstHe.m_nextEdge.m_nextEdge;

            // Reorganize Face to one of the new faces
            face.m_side = firstHe;
            HalfEdge prevEdge = new HalfEdge(indexHalfEdges++, face, facePointVertex);
            facePointVertex.m_adjacentEdges.Add(prevEdge);
            HalfEdge nextEdge = new HalfEdge(indexHalfEdges++, face, heNext.m_sourceVertex);
            state.m_Edges.Add(prevEdge);
            state.m_Edges.Add(nextEdge);

            prevEdge.m_prevEdge = nextEdge;
            prevEdge.m_nextEdge = firstHe;

            nextEdge.m_prevEdge = firstHe.m_nextEdge;
            nextEdge.m_nextEdge = prevEdge;

            firstHe.m_prevEdge = prevEdge;
            firstHe.m_nextEdge.m_nextEdge = nextEdge;

            // Create new Faces while needed
            int counter = 0;
            do
            {
                // Saving current edges state : 1st HEs of the current face and the next face
                firstHe = heNext;
                heNext = firstHe.m_nextEdge.m_nextEdge;

                // Create the new face and assign existing edges to it
                Face newFace = new Face(indexFaces, firstHe);
                state.m_Faces.Add(newFace);
                firstHe.m_face = newFace;
                firstHe.m_nextEdge.m_face = newFace;

                // Create the 2 new edges going to the facePoint
                prevEdge = new HalfEdge(indexHalfEdges++, newFace, facePointVertex);
                facePointVertex.m_adjacentEdges.Add(prevEdge);
                nextEdge = new HalfEdge(indexHalfEdges++, newFace, heNext.m_sourceVertex);
                state.m_Edges.Add(prevEdge);
                state.m_Edges.Add(nextEdge);

                // Fait les liaisons entre les edges existants et les nouveaux edges
                prevEdge.m_prevEdge = nextEdge;
                prevEdge.m_nextEdge = firstHe;

                nextEdge.m_prevEdge = firstHe.m_nextEdge;
                nextEdge.m_nextEdge = prevEdge;

                firstHe.m_prevEdge = prevEdge;
                firstHe.m_nextEdge.m_nextEdge = nextEdge;

                if (counter == 0) // if first adjacent face generated, twinEdge of prevEdge = nextEdge of reorganized (base) face
                {
                    prevEdge.m_twinEdge = face.m_side.m_nextEdge.m_nextEdge;
                    face.m_side.m_nextEdge.m_nextEdge.m_twinEdge = prevEdge;
                }
                else // else twinEdge of prevEdge is nextEdge of last generated face
                {
                    prevEdge.m_twinEdge = state.m_Faces[indexFaces - counter].m_side.m_nextEdge.m_nextEdge;
                    state.m_Faces[indexFaces - counter].m_side.m_nextEdge.m_nextEdge.m_twinEdge = prevEdge;
                }

                if (nbVertices / 2 - 2 == counter) // if last face, twinEdge of nextEsge is prevEdge of reorganized (base) face
                {
                    nextEdge.m_twinEdge = face.m_side.m_prevEdge;
                    face.m_side.m_prevEdge.m_twinEdge = nextEdge;
                }

                counter++;
                indexFaces++;
            }
            while (face.m_side != heNext); // While haven't make the whole turn around
        }
    }

    // Update is called once per frame
    void Update() { }
}
