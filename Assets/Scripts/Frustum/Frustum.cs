using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class FrustumPlane
{
    public Vec3 vertexA;
    public Vec3 vertexB;
    public Vec3 vertexC;

    public Vec3 normal;
}

public class Frustum : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public FrustumPlane nearPlane = new FrustumPlane();
    public FrustumPlane farPlane = new FrustumPlane();

    public FrustumPlane leftPlane = new FrustumPlane();
    public FrustumPlane rightPlane = new FrustumPlane();
    public FrustumPlane upPlane = new FrustumPlane();
    public FrustumPlane downPlane = new FrustumPlane();

    private List<Vec3> vertexList = new List<Vec3>();

    private List<FrustumPlane> planes = new List<FrustumPlane>();

    public int screenWidth;
    public int screenHeight;
    private float aspectRatio;

    public float fov;
    public float vFov;

    public float farDist;
    public float nearDist;

    public Vec3 nearCenter;
    public Vec3 farCenter;

    public Vec3 farUpRightV;
    public Vec3 farUpLeftV;
    public Vec3 farDownRightV;
    public Vec3 farDownLeftV;

    public Vec3 nearUpRightV;
    public Vec3 nearUpLeftV;
    public Vec3 nearDownRightV;
    public Vec3 nearDownLeftV;


    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        GetCameraFrustum();
        AddVerticesToList();
        AddPlanesToList();
    }

    private void Update()
    {
        GetCameraFrustum();
        UpdatePoints();
        UpdateVertex();
        UpdatePlanes();
    }

    private void OnDrawGizmos()
    {
        //UpdatePoints();
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

    private void GetCameraFrustum()
    {
        nearDist = mainCamera.nearClipPlane;
        farDist = mainCamera.farClipPlane;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        aspectRatio = (float)screenWidth / (float)screenHeight;
        fov = mainCamera.fieldOfView * 2.4f;
        vFov = fov / aspectRatio;
    }

    private void AddVerticesToList()
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

    private void UpdatePoints()
    {
        aspectRatio = (float)screenWidth / (float)screenHeight;

        vFov = fov / aspectRatio;

        Vec3 up = transform.up;
        Vec3 right = transform.right;

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

    private void AddPlanesToList()
    {
        planes.Add(upPlane);
        planes.Add(rightPlane);
        planes.Add(downPlane);
        planes.Add(leftPlane);

        planes.Add(farPlane);
        planes.Add(nearPlane);
    }

    private void UpdateVertex()
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

    private void UpdatePlanes()
    {
        Vec3 point = transform.position + transform.forward * ((farDist - nearDist) / 2 + nearDist); //Punto en el centro de la figura

        for (int i = 0; i < planes.Count; i++)
        {
            planes[i].vertexA = vertexList[i * 3 + 0];
            planes[i].vertexB = vertexList[i * 3 + 1];
            planes[i].vertexC = vertexList[i * 3 + 2];

            Vec3 vectorAB = planes[i].vertexB - planes[i].vertexA;
            Vec3 vectorAC = planes[i].vertexC - planes[i].vertexA;

            Vec3 normalPlane = Vec3.Cross(vectorAB, vectorAC).normalized;

            Vec3 vectorToPlane = point - planes[i].vertexA;

            float distanceToPlane = Vec3.Dot(vectorToPlane, normalPlane);

            if (distanceToPlane > 0.0f)
            {
                planes[i].normal = normalPlane;
            }
            else
            {
                planes[i].normal = normalPlane * -1;
            }
        }
    }

    public List<FrustumPlane> GetFrustum()
    {
        return planes;
    }
}
