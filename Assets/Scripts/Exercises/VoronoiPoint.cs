using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class VoronoiPoint
{
    public List<MyPlane> nearestPlanes = new List<MyPlane>();
    public GameObject gameObject;
    public Renderer renderer;
    public int colorID;

    public List<Vec3> midPoints = new List<Vec3>();
    public List<Vec3> normals = new List<Vec3>();

    public VoronoiPoint(GameObject gameObject, int color)
    {
        this.gameObject = gameObject;
        renderer = this.gameObject.GetComponent<Renderer>();
        colorID = color;
    }

    public void SwitchOnOff(bool isInside)
    {
        if (isInside)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = GetColor();
        }
    }

    public void SetNearestPlanes(List<VoronoiPoint> orederedPints)
    {
        nearestPlanes.Clear();

        foreach (VoronoiPoint point in orederedPints)
        {
            CreatePlane(point);
        }
    }

    public void CreatePlane(VoronoiPoint otherPoint)
    {
        Vec3 midPoint = (gameObject.transform.position + otherPoint.gameObject.transform.position) / 2.0f;
        Vec3 normal = (gameObject.transform.position - otherPoint.gameObject.transform.position).normalized;

        midPoints.Add(midPoint);
        normals.Add(normal);

        MyPlane plane = new MyPlane(normal, midPoint);

        nearestPlanes.Add(plane);
    }

    public void DrawNormals()
    {
        int i = 0;

        foreach (Vec3 midpoint in midPoints)
        {
            Debug.DrawLine(midpoint, (midpoint + nearestPlanes[i].normal), GetColor());

            i++;
        }
    }

    public void DrawPlanes()
    {
        float planeSize = 5f;

        foreach (MyPlane plane in nearestPlanes)
        {
            Vec3 planeCenter = -plane.normal * plane.distance;

            Vec3 planeRight = Vec3.Cross(plane.normal, Vec3.Up).normalized;

            if (planeRight.magnitude < 0.1f)
            {
                planeRight = Vec3.Cross(plane.normal, Vec3.Right).normalized;
            }

            Vec3 planeForward = Vec3.Cross(plane.normal, planeRight).normalized;

            Vec3 corner1 = planeCenter + (planeRight + planeForward) * planeSize;
            Vec3 corner2 = planeCenter + (planeRight - planeForward) * planeSize;
            Vec3 corner3 = planeCenter + (-planeRight - planeForward) * planeSize;
            Vec3 corner4 = planeCenter + (-planeRight + planeForward) * planeSize;

            Debug.DrawLine(corner1, corner2, GetColor());
            Debug.DrawLine(corner2, corner3, GetColor());
            Debug.DrawLine(corner3, corner4, GetColor());
            Debug.DrawLine(corner4, corner1, GetColor());
        }
    }

    public Color GetColor()
    {
        switch (colorID)
        {
            case 0:
                {
                    return Color.green;
                }
            case 1:
                {
                    return Color.blue;
                }
            case 2:
                {
                    return Color.cyan;
                }
            case 3:
                {
                    return Color.magenta;
                }
            case 4:
                {
                    return Color.yellow;
                }
            default:
                {
                    return Color.black;
                }
        }
    }
}
