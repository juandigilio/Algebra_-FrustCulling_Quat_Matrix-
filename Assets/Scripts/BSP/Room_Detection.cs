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
    public List<List<Vec3>> verticalNear = new List<List<Vec3>>();
    public List<List<Vec3>> verticalFar = new List<List<Vec3>>();

    private List<Room> rooms = new List<Room>();

    public List<Bounds> doorsBounds = new List<Bounds>();
    public List<Wall> doorsBoundsVertex = new List<Wall>();

    private List<Vec3> intersections = new List<Vec3>();

    private Vec3 nearLeftPoint;
    private Vec3 farLeftPoint;
    private Vec3 nearRightPoint;
    private Vec3 farRightPoint;

    [SerializeField] public int horizontalRays = 10;
    [SerializeField] public int verticalRays = 6;

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
        int j = 0;
        foreach (List<Vec3> list in verticalNear)
        {
            int i = 0;

            foreach (Vec3 near in list)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(list[i], verticalFar[j][i]);

                i++;
            }

            j++;
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

        //foreach (Vec3 point in BSP.outerPoints)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(point, 0.5f);
        //}

        //foreach (Vec3 point in BSP.inerPoints)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(point, 0.5f);
        //}
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

    private void UpdateHorizontal(List<Vec3> newNear, List<Vec3> newFar, float tempAngle)
    {
        nearLeftPoint = Vec3.Lerp(frustum.nearUpLeftV, frustum.nearDownLeftV, tempAngle);
        farLeftPoint = Vec3.Lerp(frustum.farUpLeftV, frustum.farDownLeftV, tempAngle);

        nearRightPoint = Vec3.Lerp(frustum.nearUpRightV, frustum.nearDownRightV, tempAngle);
        farRightPoint = Vec3.Lerp(frustum.farUpRightV, frustum.farDownRightV, tempAngle);

        if (horizontalRays == 1)
        {
            newNear.Add(Vec3.Lerp(nearLeftPoint, nearRightPoint, 0.5f));
            newFar.Add(Vec3.Lerp(farLeftPoint, farRightPoint, 0.5f));
        }
        else if (horizontalRays > 0)
        {
            newNear.Add(nearLeftPoint);
            newFar.Add(farLeftPoint);
        }

        float t = (1.0f / (horizontalRays - 1));

        for (int j = 0; j < horizontalRays - 1; j++)
        {
            float temp = t * (j + 1.0f);

            newNear.Add(Vec3.Lerp(nearLeftPoint, nearRightPoint, temp));
            newFar.Add(Vec3.Lerp(farLeftPoint, farRightPoint, temp));
        }
    }

    private void UpdateRaysPosition()
    {
        List<Vec3> newNear = new List<Vec3>();
        List<Vec3> newFar = new List<Vec3>();

        List<List<Vec3>> newVerticalNear = new List<List<Vec3>>();
        List<List<Vec3>> newVerticalFar = new List<List<Vec3>>();

        if (verticalRays > 0)
        {
            if (verticalRays == 1)
            {
                UpdateHorizontal(newNear, newFar, 0.5f);
            }
            else
            {
                UpdateHorizontal(newNear, newFar, 0);
            }           
        }
   
        newVerticalNear.Add(newNear);
        newVerticalFar.Add(newFar);

        float angle = (1.0f / (verticalRays - 1));

        for (int i = 0; i < verticalRays - 1; i++)
        {
            float tempAngle = angle * (i + 1.0f);

            UpdateHorizontal(newNear, newFar, tempAngle);

            newVerticalNear.Add(newNear);
            newVerticalFar.Add(newFar);
        }

        verticalNear = newVerticalNear;
        verticalFar = newVerticalFar;

        intersections = new List<Vec3>();

        BSP.CheckRoomBSP(actualRoom, rooms, verticalNear, verticalFar, intersections);
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
