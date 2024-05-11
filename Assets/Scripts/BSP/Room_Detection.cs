using UnityEngine;

public class Room_Detection : MonoBehaviour
{
    [SerializeField] public MeshFilter meshFilter;

    public void GetVertices()
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        Vector3 vertex1 = transform.TransformPoint(vertices[0]);
        Vector3 vertex2 = transform.TransformPoint(vertices[1]);
        Vector3 vertex3 = transform.TransformPoint(vertices[2]);
        Vector3 vertex4 = transform.TransformPoint(vertices[3]);


    }
}
