using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;

public class VoronoiPoint
{
    //public Vector3 position;

    public List<Plane> nearestPlanes = new List<Plane>();
    public GameObject gameObject;
    public Renderer renderer;


    public VoronoiPoint(GameObject gameObject)
    {
        this.gameObject = gameObject;
        renderer = this.gameObject.GetComponent<Renderer>();
    }

    public void SwitchOnOff(bool isInside)
    {
        if (isInside)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.white;
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
        Vector3 midPoint = (gameObject.transform.position + otherPoint.gameObject.transform.position) / 2.0f;
        Vector3 normal = (gameObject.transform.position - otherPoint.gameObject.transform.position).normalized;

        Plane plane = new Plane(normal, midPoint);

        nearestPlanes.Add(plane);

        //if (nearestPlanes.Count < 2)
        //{
        //    nearestPlanes.Add(plane);
        //}
        //else
        //{
        //    //si plane plane intersecta con el poligono formado por los planos que hayan en nearestPlanes, agregarlo a nearestPlanes
        //}
    }
}
