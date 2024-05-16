using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public static class BSP
{
    public static List<Vec3> outerPoints = new List<Vec3>();

    public static void CheckRoomBSP(int actualRoom, List<Room> rooms, List<Vec3> nearPoint, List<Vec3> farPoint, List<Vec3> intersections)
    {
        foreach (Room room in rooms)
        {
           

            if (actualRoom == room.room_ID)
            {
                Debug.LogWarning("ActualRoom " + actualRoom);
                Debug.LogWarning("Room_ID " + room.room_ID);

                List<int> intersectedRooms = new List<int>();
                outerPoints = new List<Vec3>();

                if (SameRoom(farPoint, room, rooms[5], outerPoints, actualRoom))
                {
                    break;
                }
                else
                {
                    CheckOuterPoints(rooms, outerPoints, intersectedRooms, actualRoom);
                }
                
            }
        }
    }  


    private static bool SameRoom(List<Vec3> farPoint, Room room, Room outRoom, List<Vec3> outerPoints, int actualRoom)
    {
        bool sameRoom = true;

        
        foreach (Vec3 point in farPoint)
        {
            if (actualRoom == 5)
            {
                if(!PointIsOutside(point, outRoom))
                {
                    sameRoom = false;
                    outerPoints.Add(point);
                }
            }
            else if (!PointInsideRoom(point, room))
            {
                sameRoom = false;
                outerPoints.Add(point);
            }
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

        //foreach (Vec3 point in outerPoints)
        //{
        //    int ID = 5;

        //    foreach (Room outerRoom in rooms)
        //    {
        //        if (outerRoom.room_ID != actualRoom)
        //        {
        //            if (PointInsideRoom(point, outerRoom))
        //            {
        //                ID = outerRoom.room_ID;
        //            }
        //        }

        //        intersectedRooms.Add(outerRoom.room_ID);
        //    }
        //}
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

    public static bool LineIntersectsWall(Vector3 lineStart, Vector3 lineEnd, Bounds wallBounds, out Vector3 intersectionPoint)
    {
        Vector3 lineDirection = (lineEnd - lineStart).normalized;

        Vector3 planeNormal = Vector3.up;
        Vector3 planePoint = wallBounds.center;

        float denominator = Vector3.Dot(planeNormal, lineDirection);

        if (Mathf.Abs(denominator) < Mathf.Epsilon)
        {
            intersectionPoint = Vector3.zero;
            return false;
        }

        float t = Vector3.Dot(planePoint - lineStart, planeNormal) / denominator;

        intersectionPoint = lineStart + t * lineDirection;

        if (wallBounds.Contains(intersectionPoint))
        {
            return true;
        }

        intersectionPoint = Vector3.zero;

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
