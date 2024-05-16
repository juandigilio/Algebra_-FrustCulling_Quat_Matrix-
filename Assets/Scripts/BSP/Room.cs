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
    [SerializeField] public bool showWallsBoundsVertices = false;
    [SerializeField] public bool showWallsBoundsCenter = false;
    [SerializeField] public bool showNormals = true;
    [SerializeField] public List<int> conectedRooms = new List<int>();
    [SerializeField] public List<RoomConection> doors = new List<RoomConection>();

    public List<Bounds> wallsBounds = new List<Bounds>();
    public List<Wall> wallsVertices = new List<Wall>();

    public List<Vec3> normals = new List<Vec3>();
    public List<Vec3> normalsPositions = new List<Vec3>();

    private void Start()
    {
        foreach (GameObject wall in walls)
        {
            BSP.GetBounds(wall, wallsVertices, wallsBounds);
            GetNormals();
        }
    }

    private void OnDrawGizmos()
    {
        if (showWallsBoundsVertices)
        {
            foreach (Wall wall in wallsVertices)
            {
                Debug.LogWarning("Dibuja vertices");
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(wall.vertex1, 0.5f);
                Gizmos.DrawSphere(wall.vertex2, 0.5f);
                Gizmos.DrawSphere(wall.vertex3, 0.5f);
                Gizmos.DrawSphere(wall.vertex4, 0.5f);
            }
        }

        if (showNormals)
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

        if (showNormals)
        {
            DrawNormals();
        }
        
    }

    private void GetNormals()
    {
        foreach (GameObject wall in walls)
        {
            normals.Add(wall.transform.position + wall.transform.up);
            normalsPositions.Add(transform.position);
        }
    }

    private void DrawNormals()
    {
        foreach (GameObject wall in walls)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(wall.transform.position, wall.transform.position + wall.transform.up);
        }
    }
}
