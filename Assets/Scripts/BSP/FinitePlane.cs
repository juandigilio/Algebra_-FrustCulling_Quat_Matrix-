using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomMath;

public class FinitePlane
{
    public Vec3 Normal { get; private set; }
    public float Distance { get; private set; }

    public FinitePlane(Vec3 normal, float distance)
    {
        Normal = normal.normalized;
        Distance = distance;
    }

    public bool Contains(Vector3 point)
    {
        float distance = Vector3.Dot(Normal, point) - Distance;

        return Mathf.Approximately(distance, 0f);
    }

    public bool Raycast(Ray ray, out float enter)
    {
        float denom = Vec3.Dot(ray.direction, Normal);

        if (Mathf.Approximately(denom, 0))
        {
            enter = 0;
            return false;
        }

        float t = -(Vec3.Dot(ray.origin, Normal) + Distance) / denom;
        enter = t;
        return t >= 0;
    }

    public static FinitePlane GetFinitePlaneFromVertices(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4)
    {
        Vector3 edge1 = vertex2 - vertex1;
        Vector3 edge2 = vertex3 - vertex1;
        Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

        Vector3 planePoint = (vertex1 + vertex2 + vertex3 + vertex4) / 4f;

        float distance = Vector3.Dot(normal, planePoint);

        return new FinitePlane(normal, distance);
    }
}


