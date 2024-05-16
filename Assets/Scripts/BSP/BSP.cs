using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public static class BSP
{
    public static List<Vec3> outerPoints = new List<Vec3>();

    public static void CheckRoomBSP(int actualRoom, List<Room> rooms, List<Vec3> nearPoints, List<Vec3> farPoints, List<Vec3> intersections)
    {
        foreach (Room room in rooms)
        {
            if (actualRoom == room.room_ID)
            {
                List<int> intersectedRooms = new List<int>();
                List<int> collidingRays = new List<int>();
                outerPoints = new List<Vec3>();

                if (SameRoom(farPoints, room, rooms[5], outerPoints, collidingRays, actualRoom))
                {
                    break;
                }
                else
                {
                    CheckOuterPoints(rooms, outerPoints, intersectedRooms, actualRoom);

                    List<int> validConections = new List<int>();

                    if (!CheckConections(intersectedRooms, room, validConections))
                    {
                        break;
                    }
                    else
                    {
                        CheckRoomWalls(room, nearPoints, farPoints, intersections);
                        //CheckRoomDoors(room, nearPoints, farPoints, intersections);
                        
                    }
                }
            }
        }
    }

    //chequear puertas
    private static void CheckRoomDoors(Room room, List<Vec3> nearPoints, List<Vec3> farPoints, List<Vec3> intersections)
    {
        foreach (RoomConection door in room.doors)
        {
            int iter = 0;
            Vector3 intersectionPoint = new Vec3();

            Transform normal = door.transform;

            Plane plane = new Plane(normal.up, normal.position);

            foreach (Vec3 point in nearPoints)
            {
                Vector3 direction = farPoints[iter] - nearPoints[iter];

                if (PlaneRaycast(nearPoints[iter], direction, plane, out intersectionPoint))
                {
                    float actualMagnitude = Vec3.Distance(nearPoints[iter], farPoints[iter]);
                    float newMagnitude = Vec3.Distance(nearPoints[iter], intersectionPoint);

                    if (actualMagnitude > newMagnitude)
                    {
                        door.room1.isVisible = true;
                        door.room2.isVisible = true;
                    }
                }
                iter++;
            }
        }
    }

    private static void CheckRoomWalls(Room room, List<Vec3> nearPoints, List<Vec3> farPoints, List<Vec3> intersections)
    {
        foreach (GameObject obj in room.walls)
        {
            int iter = 0;
            Vector3 intersectionPoint = new Vec3();

            Transform normal = obj.transform;

            Plane plane = new Plane(normal.up, normal.position);

            foreach (Vec3 point in nearPoints)
            {
                Vector3 direction = farPoints[iter] - nearPoints[iter];

                PlaneRaycast(nearPoints[iter], direction, plane, out intersectionPoint);

                float actualMagnitude = Vec3.Distance(nearPoints[iter], farPoints[iter]);
                float newMagnitude = Vec3.Distance(nearPoints[iter], intersectionPoint);

                if (actualMagnitude > newMagnitude)
                {
                    intersections.Add(intersectionPoint);
                }

                iter++;
            }
        }
    }

    private static bool PlaneRaycast(Vector3 origin, Vector3 direction, Plane plane, out Vector3 collisionPoint)
    {
        float rayDistance;
        if (plane.Raycast(new Ray(origin, direction), out rayDistance))
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

    private static bool SameRoom(List<Vec3> farPoint, Room room, Room outRoom, List<Vec3> outerPoints, List<int> collidingRays, int actualRoom)
    {
        bool sameRoom = true;

        int it = 0;
        foreach (Vec3 point in farPoint)
        {
            if (actualRoom == 5)
            {
                if(!PointIsOutside(point, outRoom))
                {
                    sameRoom = false;
                    outerPoints.Add(point);
                    collidingRays.Add(it);
                    Debug.LogWarning("no esta afuera");
                }
            }
            else if (!PointInsideRoom(point, room))
            {
                sameRoom = false;
                outerPoints.Add(point);
                collidingRays.Add(it);
            }

            it++;
        }

        return sameRoom;
    }

    private static void CheckOuterPoints(List<Room> rooms, List<Vec3> outerPoints, List<int> intersectedRooms, int actualRoom)
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

    public static bool LineIntersectsWall(Vec3 lineStart, Vec3 lineEnd, Wall wall, out Vec3 intersectionPoint)
    {
        Vec3 lineDirection = (lineEnd - lineStart).normalized;

        Vec3 p1 = wall.vertex1;
        Vec3 p2 = wall.vertex2;
        Vec3 p3 = wall.vertex3;
        Vec3 p4 = wall.vertex4;

        Vec3 planeNormal = Vec3.Cross(p2 - p1, p3 - p1).normalized;
        Vec3 planePoint = p1;

        float denominator = Vec3.Dot(planeNormal, lineDirection);

        if (Mathf.Abs(denominator) < Mathf.Epsilon)
        {
            intersectionPoint = Vec3.Zero;
            return false;
        }

        float t = Vec3.Dot(planePoint - lineStart, planeNormal) / denominator;

        intersectionPoint = lineStart + t * lineDirection;

        if (IsPointInsideWallBounds(intersectionPoint, p1, p2, p3, p4))
        {
            return true;
        }

        intersectionPoint = Vec3.Zero;
        return false;
    }

    public static bool IsPointInsideWallBounds(Vec3 point, Vec3 p1, Vec3 p2, Vec3 p3, Vec3 p4)
    {
       Vec3 normal1 = Vec3.Cross(p2 - p1, point - p1);
       Vec3 normal2 = Vec3.Cross(p3 - p2, point - p2);
       Vec3 normal3 = Vec3.Cross(p4 - p3, point - p3);
       Vec3 normal4 = Vec3.Cross(p1 - p4, point - p4);

        if (Vec3.Dot(normal1, normal2) >= 0 && Vec3.Dot(normal2, normal3) >= 0 && Vec3.Dot(normal3, normal4) >= 0 && Vec3.Dot(normal4, normal1) >= 0)
        {
            return true;
        }

        return false;
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
