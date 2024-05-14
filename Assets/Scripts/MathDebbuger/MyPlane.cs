using UnityEngine;
using CustomMath;

public class MyPlane : MonoBehaviour
{
    public Vec3 normal;
    public float distance;

    public MyPlane(Vec3 inNormal, Vec3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vec3.Dot(normal, inPoint);
    }

    public MyPlane(Vec3 inNormal, float d)
    {
        normal = inNormal.normalized;
        distance = d;
    }

    public MyPlane(Vec3 a, Vec3 b, Vec3 c)
    {
        normal = Vec3.Cross(b - a, c - a).normalized;
        distance = -Vec3.Dot(normal, a);
    }

    public MyPlane(Plane plane)
    {
        normal = plane.normal;
        distance = plane.distance;
    }

    public MyPlane flipped => new MyPlane(-normal, distance);

    public static MyPlane Translate(MyPlane plane, Vec3 translation)
    {
        return new MyPlane(plane.normal, plane.distance - Vec3.Dot(plane.normal, translation));
    }

    public Vec3 ClosestPointOnPlane(Vec3 point)
    {
        return point - normal * (Vec3.Dot(normal, point) + distance);
    }

    public void Flip()
    {
        normal = -normal;
        distance = -distance;
    }

    public float GetDistanceToPoint(Vec3 point)
    {
        return Vec3.Dot(normal, point) + distance;
    }

    public bool GetSide(Vec3 point)
    {
        return GetDistanceToPoint(point) > 0;
    }

    public bool Raycast(Ray ray, out float enter)
    {
        float denom = Vec3.Dot(ray.direction, normal);

        if (Mathf.Approximately(denom, 0))
        {
            enter = 0;
            return false;
        }

        float t = -(Vec3.Dot(ray.origin, normal) + distance) / denom;
        enter = t;
        return t >= 0;
    }

    public bool SameSide(Vec3 inPt0, Vec3 inPt1)
    {
        float d0 = GetDistanceToPoint(inPt0);
        float d1 = GetDistanceToPoint(inPt1);
        return (d0 > 0 && d1 > 0) || (d0 <= 0 && d1 <= 0);
    }

    public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
    {
        normal = Vec3.Cross(b - a, c - a).normalized;
        distance = -Vec3.Dot(normal, a);
    }

    public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
    {
        normal = inNormal.normalized;
        distance = -Vec3.Dot(inNormal, inPoint);
    }

    public override string ToString()
    {
        return $"Plane(normal: {normal}, distance: {distance})";
    }

    public void Translate(Vec3 translation)
    {
        distance -= Vec3.Dot(normal, translation);
    }
}
