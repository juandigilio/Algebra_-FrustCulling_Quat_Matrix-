using System.Collections.Generic;
using UnityEngine;
using CustomMath;



public class Voronoi : MonoBehaviour
{
    [SerializeField] public List<GameObject> objectsToCull;
    [SerializeField] public GameObject player;
    [SerializeField] public int selection;

    private List<VoronoiPoint> voronoiPoints;



    private void Start()
    {
        voronoiPoints = new List<VoronoiPoint>();

        int color = 0;

        foreach (GameObject obj in objectsToCull)
        {
            AddObject(obj, color);

            color++;
        }

        OrderByDistance();
    }

    private void Update()
    {
        int id = 0;

        foreach (VoronoiPoint seccion in voronoiPoints)
        {
            bool isInside = true;

            foreach (MyPlane plane in seccion.nearestPlanes)
            {
                float distance = plane.GetDistanceToPoint(player.transform.position);

                if (distance < 0)
                {
                    isInside = false;
                    break;
                }
            }

            seccion.SwitchOnOff(isInside);

            //seccion.DrawPlanes();

            if (id == selection)
            {
                seccion.DrawNormals();
                seccion.DrawPlanes();
            }


            id++;
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (VoronoiPoint seccion in voronoiPoints)
            {
                if (selection == seccion.colorID)
                {
                    Gizmos.color = seccion.GetColor();

                    foreach (Vec3 midpoint in seccion.midPoints)
                    {
                        Gizmos.DrawSphere(midpoint.ToVector3(), 0.1f);
                    }
                }
            }
        }
    }

    public void AddObject(GameObject gameObject, int color)
    {
        voronoiPoints.Add(new VoronoiPoint(gameObject, color));
    }

    public void OrderByDistance()
    {
        List<VoronoiPoint> temporalObjects = new List<VoronoiPoint>();
        List<VoronoiPoint> orderedObjects = new List<VoronoiPoint>();

        for (int i = 0; i < voronoiPoints.Count; i++)
        {
            temporalObjects.Clear();
            orderedObjects.Clear();

            temporalObjects.AddRange(voronoiPoints);
            temporalObjects.RemoveAt(i);

            while (temporalObjects.Count > 0)
            {
                float minDistance = Mathf.Infinity;
                int minIndex = 0;

                for (int j = 0; j < temporalObjects.Count; j++)
                {
                    float distance = Vector3.Distance(voronoiPoints[i].gameObject.transform.position, temporalObjects[j].gameObject.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minIndex = j;
                    }
                }

                orderedObjects.Add(temporalObjects[minIndex]);
                temporalObjects.RemoveAt(minIndex);
            }

            voronoiPoints[i].SetNearestPlanes(orderedObjects);
        }
    }
}
