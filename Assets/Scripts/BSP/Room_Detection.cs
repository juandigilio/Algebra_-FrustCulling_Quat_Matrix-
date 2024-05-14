using UnityEngine;
using System.Collections.Generic;
using CustomMath;

[System.Serializable]
public class Room_Detection : MonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Frustum frustum = new Frustum();
    [SerializeField] public ObjectCulling objectCulling;
    [SerializeField] public float initalAngle;
    [SerializeField] public float cameraFov;
    [SerializeField] public float otherFov;

    public List<Transform> planes;
    public List<GameObject> doors;

    private List<Room> rooms = new List<Room>();

    List<Vec3> nearPoint = new List<Vec3>();
    List<Vec3> farPoint = new List<Vec3>();
    private Vec3 nearLeftPoint;
    private Vec3 farLeftPoint;
    private Vec3 nearRightPoint;
    private Vec3 farRightPoint;
    private int totalLines;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        frustum = GetComponent<Frustum>();
        objectCulling = GetComponent<ObjectCulling>();

        totalLines = 16; 
    }

    private void Start()
    {
        rooms = objectCulling.GetRooms();
    }
    private void Update()
    {
        CheckCameraPosition();
        CheckRaysCollision();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < totalLines; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nearPoint[i], farPoint[i]);
        }
    }

    private void GetRaysPoints()
    {
        List<Vec3> newNear = new List<Vec3>();
        List<Vec3> newFar = new List<Vec3>();

        nearLeftPoint = Vec3.Lerp(frustum.nearUpLeftV, frustum.nearDownLeftV, 0.5f);
        farLeftPoint = Vec3.Lerp(frustum.farUpLeftV, frustum.farDownLeftV, 0.5f);

        nearRightPoint = Vec3.Lerp(frustum.nearUpRightV, frustum.nearDownRightV, 0.5f);
        farRightPoint = Vec3.Lerp(frustum.farUpRightV, frustum.farDownRightV, 0.5f);

        newNear.Add(nearLeftPoint);
        newFar.Add(farLeftPoint);

        for (int i = 0; i < totalLines; i++)
        {
            float t = (1.0f / totalLines) * ((float)i + 1.0f);

            newNear.Add(Vec3.Lerp(nearLeftPoint, nearRightPoint, t));
            newFar.Add(Vec3.Lerp(farLeftPoint, farRightPoint, t));
        }

        nearPoint = newNear;
        farPoint = newFar;
    }

    private void CheckRaysCollision()
    {
        GetRaysPoints();

        for (int i = 0; i < totalLines; i++)
        {
            Vec3 lineStart = nearPoint[i];
            Vec3 lineEnd = farPoint[i];

            MyRaycastHit hit = new MyRaycastHit();

            int room_ID = 0;

            if (MyRaycast.CastRay(lineStart, (lineEnd - lineStart), out hit, planes, doors, room_ID))
            {
                float actualMagnitude = Vec3.Distance(nearPoint[i], farPoint[i]);
                float newMagnitude = Vec3.Distance(nearPoint[i], hit.point);

                if (actualMagnitude >= newMagnitude)
                {
                    lineEnd = hit.point;
                    //Debug.Log("La linea colisiona.");
                }
            }

            nearPoint[i] = lineStart;
            farPoint[i] = lineEnd;
        }
    }

    private void CheckCameraPosition()
    {
        foreach (Room room in rooms)
        {
            if (room != null && mainCamera != null)
            {
                room.isVisible = true;

                foreach (Transform normal in room.normals)
                {
                    MyPlane plane = new MyPlane(normal.forward, normal.position);

                    if (!plane.GetSide(mainCamera.transform.position))
                    {
                        room.isVisible = false;
                        break;
                    }
                }

                if (room.isVisible)
                {
                    //Debug.Log("La cámara está dentro de la habitación." + room);
                }
            }
            else
            {
                Debug.LogWarning("Falta asignar la habitación o la cámara." + room);
            }
        }
    }
}
