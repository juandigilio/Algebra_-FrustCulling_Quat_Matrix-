using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public static class BSP
{
    public static List<Vec3> outerPoints = new List<Vec3>();
    public static List<Vec3> inerPoints = new List<Vec3>();
    public static List<Vec3> normalsStart = new List<Vec3>();
    public static List<Vec3> normalsEnd = new List<Vec3>();

    public static void CheckRoomBSP(int actualRoom, List<Room> rooms, List<List<Vec3>> nearPoints, List<List<Vec3>> farPoints, List<Vec3> intersections)
    {
        foreach (Room room in rooms)
        {
            if (actualRoom == room.room_ID)
            {
                List<int> intersectedRooms = new List<int>();
                outerPoints = new List<Vec3>();
                inerPoints = new List<Vec3>();

                if (SameRoom(farPoints, nearPoints, room, rooms[4], actualRoom))
                {
                    break;
                }
                else
                {

                    CheckOuterPoints(rooms, intersectedRooms, actualRoom);

                    List<int> validConections = new List<int>();

                    if (!CheckConections(intersectedRooms, room, validConections))
                    {
                        break;
                    }
                    else
                    {
                        CheckRoomWalls(room, intersections);   
                        CheckRoomDoors(room, intersections);
                    }
                }
            }
        }
    }

    private static void CheckRoomWalls(Room room, List<Vec3> intersections)
    {
        foreach (GameObject obj in room.walls)
        {
            Vec3 intersectionPoint = new Vec3();
            Transform normal = obj.transform;
            MyPlane plane = new MyPlane(normal.up, normal.position);

            int iter = 0;
            foreach (Vec3 point in inerPoints)
            {
                Vec3 direction = outerPoints[iter] - inerPoints[iter];

                PlaneRaycast(inerPoints[iter], direction, plane, out intersectionPoint);

                float actualMagnitude = Vec3.Distance(inerPoints[iter], outerPoints[iter]);
                float newMagnitude = Vec3.Distance(inerPoints[iter], intersectionPoint);

                if (actualMagnitude > newMagnitude)
                {
                    intersections.Add(intersectionPoint);
                }

                iter++;
            }
        }
    }

    private static void CheckRoomDoors(Room room, List<Vec3> intersections)
    {
        foreach (GameObject obj in room.doors)
        {
            RoomConection roomConection = obj.GetComponent<RoomConection>();
            BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
            Bounds bounds = boxCollider.bounds;

            if (!boxCollider)
            {
                Debug.LogWarning("no hay BoxCollider");
            }
            else
            {
                foreach (Vec3 point in intersections)
                {
                    if (bounds.Contains(point))
                    {
                        roomConection.room1.isVisible = true;
                        roomConection.room2.isVisible = true;
                    }
                }
            }

        }
    }

    private static bool PlaneRaycast(Vec3 origin, Vec3 direction, MyPlane plane, out Vec3 collisionPoint)
    {
        float rayDistance;

        if (plane.CheckIntersectionRay(new Ray(origin, direction), out rayDistance))
        {
            collisionPoint = origin + direction.normalized * rayDistance;
            return true;
        }
        else
        {
            collisionPoint = Vector3.zero;
            return false;
        }
    }

    private static bool CheckConections(List<int> intersectedRooms, Room room, List<int> validConections)
    {
        bool hasConection = false;

        foreach (int roomToCheck in intersectedRooms)
        {
            foreach (int conectedRoom in room.conectedRooms)
            {
                if (conectedRoom == roomToCheck)
                {
                    hasConection = true;

                    bool isRepeated = false;

                    foreach (int validConection in validConections)
                    {
                        if (validConection == roomToCheck)
                        {
                            isRepeated = true;
                        }
                    }

                    if (!isRepeated)
                    {
                        validConections.Add(roomToCheck);
                    }
                }
            }
        }

        return hasConection;
    }

    private static bool SameRoom(List<List<Vec3>> farPoints, List<List<Vec3>> nearPoints, Room room, Room outRoom, int actualRoom)
    {
        bool sameRoom = true;

        int i = 0;
        foreach (List<Vec3> list in farPoints)
        {
            foreach (Vec3 point in list)
            {
                //Debug.LogWarning("Entra");
                int j = 0;
                if (actualRoom == 5)
                {
                    if (!PointIsOutside(point, outRoom))
                    {
                        sameRoom = false;
                        outerPoints.Add(point);
                        inerPoints.Add(nearPoints[i][j]);
                    }
                }
                else if (!PointInsideRoom(point, room))
                {
                    sameRoom = false;
                    outerPoints.Add(point);
                    inerPoints.Add(nearPoints[i][j]);
                }

                j++;
            }
            i++;
        }


        return sameRoom;
    }

    private static void CheckOuterPoints(List<Room> rooms, List<int> intersectedRooms, int actualRoom)
    {
        foreach (Room outerRoom in rooms)
        {
            if (outerRoom.room_ID != actualRoom)
            {
                foreach (Vec3 point in outerPoints)
                {
                    if (PointInsideRoom(point, outerRoom))
                    {
                        intersectedRooms.Add(outerRoom.room_ID);
                    }
                }
            }
        }
    }

    public static void GetBounds(GameObject wall, List<Wall> wallVertices, List<Bounds> wallsBounds)
    {
        Collider collider = wall.GetComponent<Collider>();

        if (collider != null)
        {
            Bounds bounds = collider.bounds;

            wallsBounds.Add(bounds);

            List<Vec3> vertices = new List<Vec3>();

            vertices = BSP.GetBoundsVertices(bounds);

            Wall newWall;
            newWall.vertex1 = vertices[0];
            newWall.vertex2 = vertices[1];
            newWall.vertex3 = vertices[2];
            newWall.vertex4 = vertices[3];

            wallVertices.Add(newWall);
        }
        else
        {
            Debug.LogWarning("No tiene collider");
        }
    }

    public static List<Vec3> GetBoundsVertices(Bounds bounds)
    {
        List<Vec3> vertices = new List<Vec3>();

        vertices.Add(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z));

        return vertices;
    }

    public static bool PointIsOnPositiveSide(Vec3 point, Vec3 normal, Vec3 position)
    {
        Vec3 pointToPosition = point - position;
        float dotProduct = Vector3.Dot(pointToPosition, normal);
        return dotProduct > 0f;
    }

    public static bool PointInsideRoom(Vec3 point, Room room)
    {
        bool isInside = true;

        foreach (GameObject obj in room.walls)
        {
            Transform normal = obj.transform;

            MyPlane plane = new MyPlane(normal.up, normal.position);

            if (!plane.GetSide(point))
            {
                isInside = false;

                break;
            }
        }

        return isInside;
    }

    private static bool PointIsOutside(Vec3 point, Room outRoom)
    {
        bool isOutside = false;

        foreach (GameObject obj in outRoom.walls)
        {
            Transform normal = obj.transform;

            MyPlane plane = new MyPlane(normal.up, normal.position);

            if (plane.GetSide(point))
            {
                isOutside = true;
                break;
            }
        }

        return isOutside;
    }    
}
