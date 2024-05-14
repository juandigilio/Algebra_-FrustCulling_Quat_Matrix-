using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] public int room;
    [SerializeField] public List<Transform> normals = new List<Transform>();
    [SerializeField] public List<GameObject> objects = new List<GameObject>();
    //[SerializeField] public List<Plane> walls = new List<Plane>();
    [SerializeField] public bool isVisible = false;

    private List<Plane> planes = new List<Plane>();

    private void Start()
    {
        foreach (Transform normal in normals)
        {
            if (normal != null)
            {
                planes.Add(new Plane(normal.forward, normal.position));
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Transform normal in normals)
        {
            DrawPlane(normal.position, normal.forward);
        }

        if (isVisible)
        {
            foreach (GameObject obj in objects)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(obj.transform.position, 1.0f);
            }
        }
    }

    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;
        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.magenta);
    }
}
