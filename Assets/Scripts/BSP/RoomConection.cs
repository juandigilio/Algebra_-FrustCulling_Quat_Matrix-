using UnityEngine;
using System.Collections.Generic;
using CustomMath;

[System.Serializable]
public class RoomConection : MonoBehaviour
{
    [SerializeField]  public int door_ID;

    public Room room1;
    public Room room2;

    public Bounds bounds = new Bounds();
    public Wall door;

    private void Awake()
    {
        GetBounds();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(door.vertex1, 0.5f);
        Gizmos.DrawSphere(door.vertex2, 0.5f);
        Gizmos.DrawSphere(door.vertex3, 0.5f);
        Gizmos.DrawSphere(door.vertex4, 0.5f);
    }

    public static List<int> GetPosibleDoors()
    {
        List<int> availables = new List<int>();

        return availables;
    }

    private void GetBounds()
    {
        Collider collider = this.GetComponent<Collider>();

        if (collider != null)
        {
            bounds = collider.bounds;

            List<Vec3> vertices = new List<Vec3>();

            vertices = GetBoundsVertices(bounds);

            door.vertex1 = vertices[0];
            door.vertex2 = vertices[1];
            door.vertex3 = vertices[2];
            door.vertex4 = vertices[3];

        }
        else
        {
            Debug.LogWarning("No tiene collider");
        }
    }

    public static List<Vec3> GetBoundsVertices(Bounds bounds)
    {
        List<Vec3> vertices = new List<Vec3>();

        vertices.Add(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z));      
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z));       
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z));

        return vertices;
    }
}
