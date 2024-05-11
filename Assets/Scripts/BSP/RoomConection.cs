using UnityEngine;

public class RoomConection : MonoBehaviour
{
    public Room room1;
    public Room room2;

    public RoomConection(Room room1, Room room2)
    {
        this.room1 = room1;
        this.room2 = room2;
    }
}
