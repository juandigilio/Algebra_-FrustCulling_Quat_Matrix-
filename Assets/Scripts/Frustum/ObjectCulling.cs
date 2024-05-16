using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

[System.Serializable]
public class ObjectCulling : MonoBehaviour
{
    [SerializeField] public List<Room> rooms = new List<Room>();
    [SerializeField] public Frustum frustum;

    private List<FrustumPlane> planes = new List<FrustumPlane>();

    private void Start()
    {
        frustum = GetComponent<Frustum>();
    }

    private void Update()
    {
        planes = frustum.GetFrustum();
        CullObjects();
    }

    public List<Room> GetRooms()
    {
        return rooms;
    }

    private float PlanePointDistance(FrustumPlane plane, Vec3 pointToCheck)
    {
        float dist = Vec3.Dot(plane.normal, (pointToCheck - plane.vertexA));
        return dist;
    }

    private bool IsAnyVertexInFrustum(Vec3[] vertices)
    {
        foreach (Vec3 vertex in vertices)
        {
            bool isInsideFrustum = true;

            foreach (FrustumPlane plane in planes)
            {
                if (PlanePointDistance(plane, vertex) < 0.0f)
                {
                    isInsideFrustum = false;
                    break;
                }
            }

            if (isInsideFrustum)
            {
                return true;
            }
        }

        return false;
    }

    public static Bounds GetMeshBounds(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;

        if (vertices.Length == 0)
        {
            Debug.LogWarning("The mesh has no vertices!");
            return new Bounds(Vec3.Zero, Vec3.Zero);
        }

        Vec3 min = vertices[0];
        Vec3 max = vertices[0];

        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vec3.Min(min, vertices[i]);
            max = Vec3.Max(max, vertices[i]);
        }

        Bounds bounds = new Bounds((max + min) / 2f, max - min);
        return bounds;
    }

    /// <summary>
    /// Devuelve los vertices de la bounding box de una mesh transformados.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    private Vec3[] GetMeshBoundsVertex(Mesh mesh, Transform objTransform)
    {
        Bounds bounds = GetMeshBounds(mesh);
        Vec3 center = bounds.center;
        Vec3 extents = bounds.extents;

        // Nuestro frustum trabaja con vertices Vec3, por lo tanto debemos pasar nuestra variable Bounds a 8 vertices.
        Vec3[] corners = new Vec3[8];

        corners[0] = center + new Vec3(-extents.x, -extents.y, -extents.z);
        corners[1] = center + new Vec3(extents.x, -extents.y, -extents.z);
        corners[2] = center + new Vec3(-extents.x, -extents.y, extents.z);
        corners[3] = center + new Vec3(extents.x, -extents.y, extents.z);
        corners[4] = center + new Vec3(-extents.x, extents.y, -extents.z);
        corners[5] = center + new Vec3(extents.x, extents.y, -extents.z);
        corners[6] = center + new Vec3(-extents.x, extents.y, extents.z);
        corners[7] = center + new Vec3(extents.x, extents.y, extents.z);

        // Ya tenemos todas los vertices de la caja, pero tambien debemos transformar la posicion de estos a su posicion en el mundo.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = objTransform.TransformPoint(corners[i]);
        }

        return corners;
    }

    /// <summary>
    /// Transforma todos los vertices de una mesh y los devuelve en un array.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="objTransform"></param>
    /// <returns></returns>
    private Vec3[] GetMeshVertex(Mesh mesh, Transform objTransform)
    {
        Vec3[] meshVertex = new Vec3[mesh.vertexCount];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            meshVertex[i] = objTransform.TransformPoint(mesh.vertices[i]);
        }

        return meshVertex;
    }

    private void CullObjects()
    {
        foreach (Room room in rooms)
        {
            foreach (GameObject obj in room.objects)
            {
                MeshFilter mf = obj.GetComponent<MeshFilter>();
                if (!mf)
                {
                    Debug.LogWarning("Object " + obj.name + " does not have a mesh.");
                    continue;
                }

                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (!mr)
                {
                    Debug.LogWarning("Object " + obj.name + " does not have a mesh renderer.");
                    continue;
                }

                if (room.isVisible)
                {
                    Vec3[] boundingBoxVertex = GetMeshBoundsVertex(mf.mesh, obj.transform);

                    if (IsAnyVertexInFrustum(boundingBoxVertex))
                    {
                        Vec3[] meshVertex = GetMeshVertex(mf.mesh, obj.transform);
                        if (IsAnyVertexInFrustum(meshVertex))
                        {
                            mr.enabled = true;
                        }
                        else
                        {
                            mr.enabled = false;
                        }
                    }
                    else
                    {
                        mr.enabled = false;
                    }
                }
                else
                {
                    mr.enabled = false;
                }
            }
        }
    }
}
