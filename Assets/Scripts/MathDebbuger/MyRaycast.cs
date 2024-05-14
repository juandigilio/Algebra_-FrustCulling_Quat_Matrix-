using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public struct MyRaycastHit
{
    public Vector3 point;
    public Vector3 normal;
}


public static class MyRaycast
{
    public static bool CastRay(Vector3 origin, Vector3 direction, out MyRaycastHit hitInfo, List<Transform> walls, List<GameObject> doors, int door_ID, float maxDistance = Mathf.Infinity)
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

            RaycastHit hit;

            if (wallCollider.Raycast(new Ray(origin, direction), out hit, maxDistance))
            {
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    hitInfo.point = hit.point;
                    hitInfo.normal = hit.normal;
                    hitAnything = true;
                }
            }
        }

        foreach (GameObject door in doors)
        {
            Collider objCollider = door.GetComponent<Collider>();

            RoomConection roomConection = door.GetComponent<RoomConection>();

            if (roomConection == null)
            {
                Debug.LogWarning("Object does not have a Room conection.");
                continue;
            }

            if (objCollider == null)
            {
                Debug.LogWarning("Object does not have a Collider component.");
                continue;
            }

            RaycastHit hit;

            if (objCollider.Raycast(new Ray(origin, direction), out hit, maxDistance))
            {
                if (direction.magnitude > hit.distance)
                {
                    roomConection.room1.isVisible = true;
                    roomConection.room2.isVisible = true;

                    Debug.LogWarning("Colliding with door.");
                }
            }
        }

        return hitAnything;
    }
}
