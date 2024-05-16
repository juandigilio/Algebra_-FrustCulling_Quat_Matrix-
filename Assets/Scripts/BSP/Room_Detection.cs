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
    [SerializeField] public bool doorsBoundsVertices = false;
    [SerializeField] public int actualRoom = 0;
    [SerializeField] public int actualCollisions;

    public List<GameObject> walls;
    public List<GameObject> doors;

    public List<Vec3> nearPoint = new List<Vec3>();
    public List<Vec3> farPoint = new List<Vec3>();

    private List<Room> rooms = new List<Room>();
    public List<Bounds> doorsBounds = new List<Bounds>();
    public List<Wall> doorsBoundsVertex = new List<Wall>();

    private List<Vec3> intersections = new List<Vec3>();

    private Vec3 nearLeftPoint;
    private Vec3 farLeftPoint;
    private Vec3 nearRightPoint;
    private Vec3 farRightPoint;
    [SerializeField] public int TOTAL_RAYS = 16;

    private int t = 1;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        frustum = GetComponent<Frustum>();
        objectCulling = GetComponent<ObjectCulling>();
    }

    private void Start()
    {
        rooms = objectCulling.GetRooms();

        foreach (GameObject door in doors)
        {
            BSP.GetBounds(door, doorsBoundsVertex, doorsBounds);
        }
    }
    
    private void Update()
    {
        CheckCameraPosition();
        UpdateRaysPosition();
    }

    private void OnDrawGizmos()
    {
        int i = 0;

        foreach (Vec3 near in nearPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nearPoint[i], farPoint[i]);

            i++;
        }

        if (doorsBoundsVertices)
        {
            foreach (Wall vertex in doorsBoundsVertex)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(vertex.vertex1, 0.5f);
                Gizmos.DrawSphere(vertex.vertex2, 0.5f);
                Gizmos.DrawSphere(vertex.vertex3, 0.5f);
                Gizmos.DrawSphere(vertex.vertex4, 0.5f);
            }
        }

        DrawIntersections();

        //actualCollisions = intersections.Count;

    }

    private void DrawIntersections()
    {
        foreach (Vec3 point in intersections)
        {
            Gizmos.color = Color.blue;          
            Gizmos.DrawSphere(point, 0.5f);

        }

        foreach (Vec3 point in BSP.outerPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point, 0.5f);
        }
    }

    private void UpdateRaysPosition()
    {
        List<Vec3> newNear = new List<Vec3>();
        List<Vec3> newFar = new List<Vec3>();

        nearLeftPoint = Vec3.Lerp(frustum.nearUpLeftV, frustum.nearDownLeftV, 0.5f);
        farLeftPoint = Vec3.Lerp(frustum.farUpLeftV, frustum.farDownLeftV, 0.5f);

        nearRightPoint = Vec3.Lerp(frustum.nearUpRightV, frustum.nearDownRightV, 0.5f);
        farRightPoint = Vec3.Lerp(frustum.farUpRightV, frustum.farDownRightV, 0.5f);

        if (TOTAL_RAYS > 0)
        {
            newNear.Add(nearLeftPoint);
            newFar.Add(farLeftPoint);
        }

        float t = (1.0f / (TOTAL_RAYS - 1));

        for (int i = 0; i < TOTAL_RAYS - 1; i++)
        {
            float temp = t * (i + 1.0f);

            newNear.Add(Vec3.Lerp(nearLeftPoint, nearRightPoint, temp));
            newFar.Add(Vec3.Lerp(farLeftPoint, farRightPoint, temp));
        }

        nearPoint = newNear;
        farPoint = newFar;

        intersections = new List<Vec3>();

        BSP.CheckRoomBSP(actualRoom, rooms, nearPoint, farPoint, intersections);
    }

    private void CheckCameraPosition()
    {
        actualRoom = 5;

        foreach (Room room in rooms)
        {
            room.isVisible = false;
    
            if (room == null || mainCamera == null)
            {
                Debug.LogWarning("Falta asignar la habitación o la cámara." + room);
            }
            else
            {
                room.isVisible = BSP.PointInsideRoom(mainCamera.transform.position, room);
                //Debug.LogWarning("Habitacion" + room + room.isVisible);

                if (room.isVisible)
                {
                    actualRoom = room.room_ID;

                    foreach (Room toCheckRoom in rooms)
                    {
                        if (toCheckRoom.room_ID != actualRoom)
                        {
                            toCheckRoom.isVisible = false;
                        }
                    }
                    break;
                }
            }    
        }
    }
}
