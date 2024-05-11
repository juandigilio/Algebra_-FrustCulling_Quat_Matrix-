using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTree : MonoBehaviour
{
    public List<RoomConection> roomConection = new List<RoomConection>();

    public Room rootRoom { get; set; }
}
