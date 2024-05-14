using System.Collections.Generic;
using UnityEngine;

public struct MyRaycastHit
{
    public Vector3 point;
    public Vector3 normal;
}


public static class MyRaycast
{
    public static bool CastRay(Vector3 origin, Vector3 direction, out MyRaycastHit hitInfo, List<Transform> walls, float maxDistance = Mathf.Infinity)
    {
        hitInfo = new MyRaycastHit();

        float closestDistance = maxDistance;
        bool hitAnything = false;

        foreach (Transform wallTransform in walls)
        {
            Collider wallCollider = wallTransform.GetComponent<Collider>();
            if (wallCollider == null)
            {
                Debug.LogWarning("Wall transform does not have a Collider component.");
                continue;
            }

            // Castear un rayo contra el collider del muro
            RaycastHit hit;
            if (wallCollider.Raycast(new Ray(origin, direction), out hit, maxDistance))
            {
                // Verificar si la distancia del hit está dentro del rango del rayo
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    hitInfo.point = hit.point;
                    hitInfo.normal = hit.normal;
                    hitAnything = true;
                }
            }
        }

        return hitAnything;
    }
}
