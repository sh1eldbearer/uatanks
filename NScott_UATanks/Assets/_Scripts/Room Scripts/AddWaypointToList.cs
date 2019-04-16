using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]

/*
 * This script adds a waypoint object to the waypoint list for whichever room
 * the waypoint is placed in.
 * 
 * It should be attached to a waypoint object.
 */

public class AddWaypointToList : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private RoomData roomData;


    // Use this for initialization
    void Start ()
    {
        roomData = this.gameObject.GetComponentInParent<RoomData>();
        roomData.roomWaypoints.Add(this.gameObject.GetComponent<Transform>());
        roomData.roomWaypoints = roomData.roomWaypoints.OrderBy(transform => transform.gameObject.name).ToList<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
