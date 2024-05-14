using UnityEngine;

[System.Serializable]
public class Room_Detection : MonoBehaviour
{
    [SerializeField] public Room room;
    [SerializeField] public Camera mainCamera;


    private void Awake()
    {
        room = GetComponent<Room>();
        
    }

    private void Update()
    {
        CheckCameraPosition();
    }

    private void CheckCameraPosition()
    {
        if (room != null && mainCamera != null)
        {
            room.isVisible = true;

            foreach (Transform normal in room.normals)
            {
                Plane plane = new Plane(normal.forward, normal.position);

                if (!plane.GetSide(mainCamera.transform.position))
                {
                    room.isVisible = false;
                    break;
                }
            }

            if (room.isVisible)
            {
                Debug.Log("La cámara está dentro de la habitación." + room);
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar la habitación o la cámara." + room);
        }
    }
}
