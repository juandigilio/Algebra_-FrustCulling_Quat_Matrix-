using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlane : MonoBehaviour
{
    public Vector3 normal;
    public float distance;

    public MyPlane(Vector3 inNormal, Vector3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vector3.Dot(normal, inPoint);
    }

    public MyPlane(Vector3 inNormal, float d)
    {
        normal = inNormal.normalized;
        distance = d;
    }

    public MyPlane(Vector3 a, Vector3 b, Vector3 c)
    {
        normal = Vector3.Cross(b - a, c - a).normalized;
        distance = -Vector3.Dot(normal, a);
    }

    public MyPlane(Plane plane)
    {
        normal = plane.normal;
        distance = plane.distance;
    }

    public MyPlane flipped => new MyPlane(-normal, distance);

    public static MyPlane Translate(MyPlane plane, Vector3 translation)
    {
        return new MyPlane(plane.normal, plane.distance - Vector3.Dot(plane.normal, translation));
    }

    public Vector3 ClosestPointOnPlane(Vector3 point)
    {
        return point - normal * (Vector3.Dot(normal, point) + distance);
    }

    public void Flip()
    {
        normal = -normal;
        distance = -distance;
    }

    public float GetDistanceToPoint(Vector3 point)
    {
        return Vector3.Dot(normal, point) + distance;
    }

    public bool GetSide(Vector3 point)
    {
        return GetDistanceToPoint(point) > 0;
    }

    public bool Raycast(Ray ray, out float enter)
    {
        float denom = Vector3.Dot(ray.direction, normal);

        if (Mathf.Approximately(denom, 0))
        {
            enter = 0;
            return false;
        }

        float t = -(Vector3.Dot(ray.origin, normal) + distance) / denom;
        enter = t;
        return t >= 0;
    }

    public bool SameSide(Vector3 inPt0, Vector3 inPt1)
    {
        float d0 = GetDistanceToPoint(inPt0);
        float d1 = GetDistanceToPoint(inPt1);
        return (d0 > 0 && d1 > 0) || (d0 <= 0 && d1 <= 0);
    }

    public void Set3Points(Vector3 a, Vector3 b, Vector3 c)
    {
        normal = Vector3.Cross(b - a, c - a).normalized;
        distance = -Vector3.Dot(normal, a);
    }

    public void SetNormalAndPosition(Vector3 inNormal, Vector3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vector3.Dot(inNormal, inPoint);
    }

    public override string ToString()
    {
        return $"Plane(normal: {normal}, distance: {distance})";
    }

    public void Translate(Vector3 translation)
    {
        distance -= Vector3.Dot(normal, translation);
    }
}
