using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AI;
using Unity.VisualScripting;
using Unity.AI.Navigation;


[RequireComponent(typeof(ARPlaneMeshVisualizer), typeof(MeshRenderer), typeof(ARPlane))]
public class ARFeatheredPlaneMeshVisualizer : MonoBehaviour
{

    static List<Vector3> s_FeatheringUVs = new List<Vector3>();
    static List<Vector3> s_Vertices = new List<Vector3>();
    ARPlaneMeshVisualizer m_PlaneMeshVisualiser;
    ARPlane m_Plane;
    Material m_FeatheredPlaneMaterial;
    [SerializeField] float m_FeatheringWidth = 0.2f;
    public float featheringWidth {  
        get { return m_FeatheringWidth;}
        set { m_FeatheringWidth = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_PlaneMeshVisualiser = GetComponent<ARPlaneMeshVisualizer>();
        m_FeatheredPlaneMaterial = GetComponent<MeshRenderer>().material;
        m_Plane = GetComponent<ARPlane>();
    }

    private void OnEnable()
    {
        m_Plane.boundaryChanged += ARPlane_boundaryUpdated;
    }

    private void OnDisable()
    {
        m_Plane.boundaryChanged -= ARPlane_boundaryUpdated;
    }

    void ARPlane_boundaryUpdated(ARPlaneBoundaryChangedEventArgs eventArgs)
    {
        GameManager.planesFound = true;
        GenerateBoundaryUVs(m_PlaneMeshVisualiser.mesh);

        ARPlane tempPlane = eventArgs.plane;
        NavMeshSurface tempNavMesh = tempPlane.gameObject.GetComponent<NavMeshSurface>();

        if (tempNavMesh)
        {
            tempNavMesh.BuildNavMesh();
        }
        else
        {
            NavMeshSurface temp = tempPlane.AddComponent<NavMeshSurface>();
            temp.BuildNavMesh();
        }
    }

    void GenerateBoundaryUVs(Mesh mesh)
    {
        int vertexCount = mesh.vertexCount;
        s_FeatheringUVs.Clear();
        if (s_FeatheringUVs.Capacity < vertexCount) s_FeatheringUVs.Capacity = vertexCount;
        mesh.GetVertices(s_Vertices);
        Vector3 centerInPlaneSpace = s_Vertices[s_Vertices.Count - 1];
        Vector3 uv = new Vector3(0, 0, 0);
        float shortestUVMapping = float.MaxValue;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            float vertexDist = Vector3.Distance(s_Vertices[i], centerInPlaneSpace);
            float uvMapping = vertexDist / Mathf.Max(vertexDist - featheringWidth, 0.001f);
            uv.x = uvMapping;
            if (shortestUVMapping > uvMapping) shortestUVMapping = uvMapping;
            s_FeatheringUVs.Add(uv);
        }

        m_FeatheredPlaneMaterial.SetFloat("_ShortestUVMapping", shortestUVMapping);
        uv.Set(0, 0, 0);
        s_FeatheringUVs.Add(uv);
        mesh.SetUVs(1, s_FeatheringUVs);
        mesh.UploadMeshData(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
