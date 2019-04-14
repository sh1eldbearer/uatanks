using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script contains information about this "room" tile.
 * 
 * It should attached to the root game object of any room that will used
 * in the game.
 */

public class RoomData : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public Transform roomTf;
    [Tooltip("The list of waypoints in this room.")]
    public List<Transform> roomWaypoints;

    /* Private Variables */

    private void Start()
    {
        // Component reference assignments
        roomTf = this.GetComponent<Transform>();
    }
}
