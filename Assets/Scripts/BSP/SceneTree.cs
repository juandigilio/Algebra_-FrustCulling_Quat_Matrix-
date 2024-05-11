using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTree : MonoBehaviour
{
    public List<RoomConection> roomConection = new List<RoomConection>();

    public Room rootRoom { get; set; }

    public SceneTree()
    {
        
    }

    public void AddConection(Room room1, Room room2)
    {
        RoomConection conection = new RoomConection(room1, room2);
        roomConection.Add(conection);
    }
}
