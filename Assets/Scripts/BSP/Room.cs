using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using CustomMath;

public struct Wall
{
    public Vec3 vertex1;
    public Vec3 vertex2;
    public Vec3 vertex3;
    public Vec3 vertex4;
}

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] public int room_ID;
    [SerializeField] public List<GameObject> walls = new List<GameObject>();
    [SerializeField] public List<GameObject> objects = new List<GameObject>();
    [SerializeField] public bool isVisible = false;
    [SerializeField] public bool wallsBoundsVertices = false;
    [SerializeField] public bool wallsBoundsCenter = false;
    [SerializeField] public List<int> conectedRooms = new List<int>();
    [SerializeField] public List<RoomConection> doors = new List<RoomConection>();

    public List<Bounds> wallsBounds = new List<Bounds>();
    public List<Wall> wallsVertices = new List<Wall>();

    private void Start()
    {
        foreach (GameObject wall in walls)
        {
            BSP.GetBounds(wall, wallsVertices, wallsBounds);
        }       
    }

    private void OnDrawGizmos()
    {
        if (wallsBoundsVertices)
        {
            foreach (Wall wall in wallsVertices)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(wall.vertex1, 0.5f);
                Gizmos.DrawSphere(wall.vertex2, 0.5f);
                Gizmos.DrawSphere(wall.vertex3, 0.5f);
                Gizmos.DrawSphere(wall.vertex4, 0.5f);
            }
        }

        if (wallsBoundsCenter)
        {
            foreach (Bounds bounds in wallsBounds)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(bounds.center, 0.5f);
            }
        }

        if (isVisible)
        {
            foreach (GameObject obj in objects)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(obj.transform.position, 0.5f);
            }
        }
    }
  
}
