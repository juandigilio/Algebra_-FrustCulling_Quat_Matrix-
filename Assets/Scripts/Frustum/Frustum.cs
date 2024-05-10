using System.Collections.Generic;
using UnityEngine;

public class Frustum : MonoBehaviour 
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField] 
    private List<GameObject> objectsToCull;

    public class FrustumPlane
    {
        public Vector3 vertexA;
        public Vector3 vertexB;
        public Vector3 vertexC;

        public Vector3 normal;
    }

    public FrustumPlane nearPlane = new FrustumPlane();
    public FrustumPlane farPlane = new FrustumPlane();

    public FrustumPlane leftPlane = new FrustumPlane();
    public FrustumPlane rightPlane = new FrustumPlane();
    public FrustumPlane upPlane = new FrustumPlane();
    public FrustumPlane downPlane = new FrustumPlane();

    private List<Vector3> vertexList = new List<Vector3>();

    private List<FrustumPlane> planes = new List<FrustumPlane>();

    public int screenWidth;
    public int screenHeight;
    private float aspectRatio;

    public float fov;
    private float vFov;

    public float farDist;
    public float nearDist;

    private Vector3 nearCenter;
    private Vector3 farCenter;

    private Vector3 farUpRightV;
    private Vector3 farUpLeftV;
    private Vector3 farDownRightV;
    private Vector3 farDownLeftV;

    private Vector3 nearUpRightV;
    private Vector3 nearUpLeftV;
    private Vector3 nearDownRightV;
    private Vector3 nearDownLeftV;



    private bool IsAnyVertexInFrustum(Vector3[] vertices)
    {
        foreach (Vector3 vertex in vertices)
        {
            bool isInsideFrustum = true;

            foreach (FrustumPlane plane in planes)
            {
                if (PlanePointDistance(plane, vertex) < 0.0f)
                {
                    isInsideFrustum = false;
                    break;
                }
            }

            if (isInsideFrustum)
            {
                return true;
            }
        }

        return false;
    }

    Bounds GetMeshBounds(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;

        if (vertices.Length == 0)
        {
            Debug.LogWarning("The mesh has no vertices!");
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Vector3 min = vertices[0];
        Vector3 max = vertices[0];

        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vector3.Min(min, vertices[i]);
            max = Vector3.Max(max, vertices[i]);
        }

        Bounds bounds = new Bounds((max + min) / 2f, max - min);
        return bounds;
    }

    /// <summary>
    /// Devuelve los vertices de la bounding box de una mesh transformados.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    Vector3[] GetMeshBoundsVertex(Mesh mesh, Transform objTransform)
    {
        Bounds bounds = GetMeshBounds(mesh);
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        // Nuestro frustum trabaja con vertices Vector3, por lo tanto debemos pasar nuestra variable Bounds a 8 vertices.
        Vector3[] corners = new Vector3[8];

        corners[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
        corners[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
        corners[2] = center + new Vector3(-extents.x, -extents.y, extents.z);
        corners[3] = center + new Vector3(extents.x, -extents.y, extents.z);
        corners[4] = center + new Vector3(-extents.x, extents.y, -extents.z);
        corners[5] = center + new Vector3(extents.x, extents.y, -extents.z);
        corners[6] = center + new Vector3(-extents.x, extents.y, extents.z);
        corners[7] = center + new Vector3(extents.x, extents.y, extents.z);

        // Ya tenemos todas los vertices de la caja, pero tambien debemos transformar la posicion de estos a su posicion en el mundo.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = objTransform.TransformPoint(corners[i]);
        }

        return corners;
    }

    /// <summary>
    /// Transforma todos los vertices de una mesh y los devuelve en un array.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    Vector3[] GetMeshVertex(Mesh mesh, Transform objTransform)
    {
        Vector3[] meshVertex = new Vector3[mesh.vertexCount];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            meshVertex[i] = objTransform.TransformPoint(mesh.vertices[i]);
        }

        return meshVertex;
    }


    private void CullObjects()
    {
        foreach (GameObject obj in objectsToCull)
        {
            MeshFilter mf = obj.GetComponent<MeshFilter>();
            if (!mf)
            {
                Debug.LogWarning("Object " + obj.name + " does not have a mesh.");
                continue;
            }

            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            if (!mr)
            {
                Debug.LogWarning("Object " + obj.name + " does not have a mesh renderer.");
                continue;
            }

            Vector3[] boundingBoxVertex = GetMeshBoundsVertex(mf.mesh, obj.transform);

            if (IsAnyVertexInFrustum(boundingBoxVertex))
            {
                Vector3[] meshVertex = GetMeshVertex(mf.mesh, obj.transform);
                if (IsAnyVertexInFrustum(meshVertex))
                {
                    mr.enabled = true;
                }
                else
                {
                    mr.enabled = false;
                }
            }
            else
            {
                mr.enabled = false;
            }
        }
    }

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        CreateNewFrutum();
        AddVerticesToList();
        AddPlanesToList();

    }

    //FrustumGetter

    void CreateNewFrutum()
    {
        nearDist = mainCamera.nearClipPlane;
        farDist = mainCamera.farClipPlane;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        aspectRatio = (float)screenWidth / (float)screenHeight;
        fov = mainCamera.fieldOfView * 2.4f;
        vFov = fov / aspectRatio;
    }

    void UpdatePoints()
    {
        aspectRatio = (float)screenWidth / (float)screenHeight;

        vFov = fov / aspectRatio;

        Vector3 up = transform.up;
        Vector3 right = transform.right;

        nearCenter = transform.position + transform.forward * nearDist;
        farCenter = transform.position + transform.forward * farDist;


        float nearPlaneHeight = Mathf.Tan((vFov * 0.5f) * Mathf.Deg2Rad) * nearDist * 2f;
        float nearPlaneWidth = nearPlaneHeight * aspectRatio;

        float farPlaneHeight = Mathf.Tan((vFov * 0.5f) * Mathf.Deg2Rad) * farDist * 2f;
        float farPlaneWidth = farPlaneHeight * aspectRatio;


        //Up, Right y Forward son vectores3 que van desde el 0 del objeto hasta un punto. Cuando yo roto mi figura el punto al que se dirige cada vector también cambia, y no mantiene el 1
        //en una sola coordenada determinada (x=right, y=up, z=forward), sino que cada uno puede tener valores en las 3 coordenadas a la vez (con la magnitud siendo 1).
        //Usando el transform.up y el transform.right ademas del forward de antes para conseguir los vertices logro tener las posiciones relativas al objeto, no del ambito global
        nearUpLeftV.x = nearCenter.x + (up.x * nearPlaneHeight / 2) - (right.x * nearPlaneWidth / 2);
        nearUpLeftV.y = nearCenter.y + (up.y * nearPlaneHeight / 2) - (right.y * nearPlaneWidth / 2);
        nearUpLeftV.z = nearCenter.z + (up.z * nearPlaneHeight / 2) - (right.z * nearPlaneWidth / 2);

        nearUpRightV.x = nearCenter.x + (up.x * nearPlaneHeight / 2) + (right.x * nearPlaneWidth / 2);
        nearUpRightV.y = nearCenter.y + (up.y * nearPlaneHeight / 2) + (right.y * nearPlaneWidth / 2);
        nearUpRightV.z = nearCenter.z + (up.z * nearPlaneHeight / 2) + (right.z * nearPlaneWidth / 2);

        nearDownLeftV.x = nearCenter.x - (up.x * nearPlaneHeight / 2) - (right.x * nearPlaneWidth / 2);
        nearDownLeftV.y = nearCenter.y - (up.y * nearPlaneHeight / 2) - (right.y * nearPlaneWidth / 2);
        nearDownLeftV.z = nearCenter.z - (up.z * nearPlaneHeight / 2) - (right.z * nearPlaneWidth / 2);

        nearDownRightV.x = nearCenter.x - (up.x * nearPlaneHeight / 2) + (right.x * nearPlaneWidth / 2);
        nearDownRightV.y = nearCenter.y - (up.y * nearPlaneHeight / 2) + (right.y * nearPlaneWidth / 2);
        nearDownRightV.z = nearCenter.z - (up.z * nearPlaneHeight / 2) + (right.z * nearPlaneWidth / 2);

        farUpLeftV.x = farCenter.x + (up.x * farPlaneHeight / 2) - (right.x * farPlaneWidth / 2);
        farUpLeftV.y = farCenter.y + (up.y * farPlaneHeight / 2) - (right.y * farPlaneWidth / 2);
        farUpLeftV.z = farCenter.z + (up.z * farPlaneHeight / 2) - (right.z * farPlaneWidth / 2);

        farUpRightV.x = farCenter.x + (up.x * farPlaneHeight / 2) + (right.x * farPlaneWidth / 2);
        farUpRightV.y = farCenter.y + (up.y * farPlaneHeight / 2) + (right.y * farPlaneWidth / 2);
        farUpRightV.z = farCenter.z + (up.z * farPlaneHeight / 2) + (right.z * farPlaneWidth / 2);

        farDownLeftV.x = farCenter.x - (up.x * farPlaneHeight / 2) - (right.x * farPlaneWidth / 2);
        farDownLeftV.y = farCenter.y - (up.y * farPlaneHeight / 2) - (right.y * farPlaneWidth / 2);
        farDownLeftV.z = farCenter.z - (up.z * farPlaneHeight / 2) - (right.z * farPlaneWidth / 2);

        farDownRightV.x = farCenter.x - (up.x * farPlaneHeight / 2) + (right.x * farPlaneWidth / 2);
        farDownRightV.y = farCenter.y - (up.y * farPlaneHeight / 2) + (right.y * farPlaneWidth / 2);
        farDownRightV.z = farCenter.z - (up.z * farPlaneHeight / 2) + (right.z * farPlaneWidth / 2);
    }

    void AddVerticesToList()
    {
        //Triangulo superior
        vertexList.Add(transform.position);
        vertexList.Add(farUpRightV);
        vertexList.Add(farUpLeftV);

        //Triangulo derecho
        vertexList.Add(transform.position);
        vertexList.Add(farUpRightV);
        vertexList.Add(farDownRightV);

        //Triangulo inferior
        vertexList.Add(transform.position);
        vertexList.Add(farDownRightV);
        vertexList.Add(farDownLeftV);

        //Triangulo izquierdo
        vertexList.Add(transform.position);
        vertexList.Add(farUpLeftV);
        vertexList.Add(farDownLeftV);

        //Triangulo del far plane
        vertexList.Add(farUpRightV);
        vertexList.Add(farDownRightV);
        vertexList.Add(farDownLeftV);

        //Triangulo del near plane
        vertexList.Add(nearUpRightV);
        vertexList.Add(nearDownRightV);
        vertexList.Add(nearDownLeftV);
    }

    void UpdateVertex()
    {
        //Update triangulo superior
        vertexList[0] = nearUpRightV;
        vertexList[1] = farUpRightV;
        vertexList[2] = farUpLeftV;

        //Update triangulo derecho
        vertexList[3] = nearUpRightV;
        vertexList[4] = farUpRightV;
        vertexList[5] = farDownRightV;

        //Update triangulo inferior
        vertexList[6] = nearDownLeftV;
        vertexList[7] = farDownRightV;
        vertexList[8] = farDownLeftV;

        //Update triangulo izquierdo
        vertexList[9] = nearDownLeftV;
        vertexList[10] = farUpLeftV;
        vertexList[11] = farDownLeftV;

        //Update triangulo del far plane
        vertexList[12] = farUpRightV;
        vertexList[13] = farDownRightV;
        vertexList[14] = farDownLeftV;

        //Update triangulo del near plane
        vertexList[15] = nearUpRightV;
        vertexList[16] = nearDownRightV;
        vertexList[17] = nearDownLeftV;
    }

    void AddPlanesToList()
    {
        planes.Add(upPlane);
        planes.Add(rightPlane);
        planes.Add(downPlane);
        planes.Add(leftPlane);

        planes.Add(farPlane);
        planes.Add(nearPlane);
    }

    void UpdatePlanes()
    {
        Vector3 point = transform.position + transform.forward * ((farDist - nearDist) / 2 + nearDist); //Punto en el centro de la figura

        for (int i = 0; i < planes.Count; i++)
        {
            planes[i].vertexA = vertexList[i * 3 + 0];
            planes[i].vertexB = vertexList[i * 3 + 1];
            planes[i].vertexC = vertexList[i * 3 + 2];

            Vector3 vectorAB = planes[i].vertexB - planes[i].vertexA;
            Vector3 vectorAC = planes[i].vertexC - planes[i].vertexA;

            Vector3 normalPlane = Vector3.Cross(vectorAB, vectorAC).normalized; //Calcula la normal con producto cruz y la normaliza

            //Verifica la orientación y la cambia en caso de que no sea hacia el centro
            Vector3 vectorToPlane = point - planes[i].vertexA;
            float distanceToPlane = Vector3.Dot(vectorToPlane, normalPlane); //Si > 0 apuntan hacia el mismo lado (el centro), sino no

            if (distanceToPlane > 0.0f) //Si es mayor que cero mantengo la dirección porque esta hacia el centro
            {
                planes[i].normal = normalPlane;
            }
            else //Si apunta hacia diferente lado la multiplico por -1 para invertir su dirección porque significa que estaba hacia afuera
            {
                planes[i].normal = normalPlane * -1;
            }
        }
    }

    float PlanePointDistance(FrustumPlane plane, Vector3 pointToCheck)
    {
        float dist = Vector3.Dot(plane.normal, (pointToCheck - plane.vertexA));
        return dist;
    }

    void OnDrawGizmos()
    {
        UpdatePoints();  
        Gizmos.color = Color.green;

        Gizmos.DrawLine(nearUpRightV, farUpRightV);
        Gizmos.DrawLine(nearUpLeftV, farUpLeftV);
        Gizmos.DrawLine(farUpRightV, farUpLeftV);
        Gizmos.DrawLine(nearUpRightV, nearUpLeftV);

        Gizmos.DrawLine(nearDownRightV, farDownRightV);
        Gizmos.DrawLine(nearDownLeftV, farDownLeftV);
        Gizmos.DrawLine(farDownRightV, farDownLeftV);
        Gizmos.DrawLine(nearDownRightV, nearDownLeftV);

        Gizmos.DrawLine(nearDownRightV, nearUpRightV);
        Gizmos.DrawLine(nearDownLeftV, nearUpLeftV);
        Gizmos.DrawLine(farDownRightV, farUpRightV);
        Gizmos.DrawLine(farDownLeftV, farUpLeftV);
    }

    private void Update()
    {
        UpdatePoints();
        UpdateVertex();
        UpdatePlanes();

        CullObjects();
    }
}
