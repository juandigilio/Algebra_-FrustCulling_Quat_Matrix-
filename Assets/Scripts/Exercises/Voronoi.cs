using System.Collections.Generic;
using UnityEngine;


public class Voronoi : MonoBehaviour
{
    [SerializeField] public List<GameObject> objectsToCull;
    [SerializeField] public GameObject player;

    private List<VoronoiPoint> voronoiPoints;



    private void Start()
    {
        voronoiPoints = new List<VoronoiPoint>();

        foreach (GameObject obj in objectsToCull)
        {
            AddPoint(obj);
        }

        OrderByDistance();
    }

    private void Update()
    {
        foreach (VoronoiPoint seccion in voronoiPoints)
        {
            bool isInside = true;

            foreach (Plane plane in seccion.nearestPlanes)
            {
                float distance = plane.GetDistanceToPoint(player.transform.position);

                if (distance < 0)
                {
                    isInside = false;
                    break;
                }
            }

            seccion.SwitchOnOff(isInside);
        }
    }

    public void AddPoint(GameObject gameObject)
    {
        voronoiPoints.Add(new VoronoiPoint(gameObject));
    }

    public void OrderByDistance()
    {
        List<VoronoiPoint> temporalPoints = new List<VoronoiPoint>();
        List<VoronoiPoint> orderedPoints = new List<VoronoiPoint>();


        for (int i = 0; i < voronoiPoints.Count; i++)
        {
            temporalPoints.Clear();
            orderedPoints.Clear();

            temporalPoints.AddRange(voronoiPoints);
            temporalPoints.RemoveAt(i);

            while (temporalPoints.Count > 0)
            {
                float minDistance = Mathf.Infinity;
                int minIndex = 0;

                for (int j = 0; j < temporalPoints.Count; j++)
                {
                    float distance = Vector3.Distance(voronoiPoints[i].gameObject.transform.position, temporalPoints[j].gameObject.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minIndex = j;
                    }
                }

                orderedPoints.Add(temporalPoints[minIndex]);
                temporalPoints.RemoveAt(minIndex);
            }

            voronoiPoints[i].SetNearestPlanes(orderedPoints);
        }
    }
}
