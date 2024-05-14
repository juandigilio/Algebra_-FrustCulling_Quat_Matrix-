using UnityEngine;
using System.Collections.Generic;
//using CustomMath;

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

    private List<Room> rooms = new List<Room>();

    List<Vector3> nearPoint = new List<Vector3>();
    List<Vector3> farPoint = new List<Vector3>();
    private Vector3 nearLeftPoint;
    private Vector3 farLeftPoint;
    private Vector3 nearRightPoint;
    private Vector3 farRightPoint;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        frustum = GetComponent<Frustum>();
        objectCulling = GetComponent<ObjectCulling>();
    }

    private void Start()
    {
        rooms = objectCulling.GetRooms();
    }
    private void Update()
    {
        CheckCameraPosition();
    }

    private void OnDrawGizmos()
    {
        DrawRays();
    }

    private void GetRaysPoints(int totalLines)
    {
        List<Vector3> newNear = new List<Vector3>();
        List<Vector3> newFar = new List<Vector3>();

        nearLeftPoint = Vector3.Lerp(frustum.nearUpLeftV, frustum.nearDownLeftV, 0.5f);
        farLeftPoint = Vector3.Lerp(frustum.farUpLeftV, frustum.farDownLeftV, 0.5f);

        nearRightPoint = Vector3.Lerp(frustum.nearUpRightV, frustum.nearDownRightV, 0.5f);
        farRightPoint = Vector3.Lerp(frustum.farUpRightV, frustum.farDownRightV, 0.5f);

        newNear.Add(nearLeftPoint);
        newFar.Add(farLeftPoint);

        for (int i = 0; i < totalLines; i++)
        {
            float t = (1.0f / totalLines) * ((float)i + 1.0f);

            newNear.Add(Vector3.Lerp(nearLeftPoint, nearRightPoint, t));
            newFar.Add(Vector3.Lerp(farLeftPoint, farRightPoint, t));
        }

        nearPoint = newNear;
        farPoint = newFar;
    }

    private void DrawRays()
    {
        int totalLines = 16;

        GetRaysPoints(totalLines);

        for (int i = 0; i < totalLines; i++)
        {
            Vector3 lineStart = nearPoint[i];
            Vector3 lineEnd = farPoint[i];

            MyRaycastHit hit = new MyRaycastHit();

            if (MyRaycast.CastRay(lineStart, (lineEnd - lineStart).normalized, out hit, planes))
            {
                Vector3 newFar = hit.point;

                float actualMagnitude = Vector3.Distance(nearPoint[i], farPoint[i]);
                float newMagnitude = Vector3.Distance(nearPoint[i], newFar);

                if (actualMagnitude >= newMagnitude)
                {
                    lineEnd = hit.point;
                    Debug.Log("La linea colisiona.");
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(lineStart, lineEnd);
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
                    Plane plane = new Plane(normal.forward, normal.position);

                    if (!plane.GetSide(mainCamera.transform.position))
                    {
                        room.isVisible = false;
                        break;
                    }
                }

                if (room.isVisible)
                {
                    Debug.Log("La cámara está dentro de la habitación." + room);
                }
            }
            else
            {
                Debug.LogWarning("Falta asignar la habitación o la cámara." + room);
            }
        }
    }
}
