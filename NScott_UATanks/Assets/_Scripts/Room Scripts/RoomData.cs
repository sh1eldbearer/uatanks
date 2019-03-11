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
    public List<Transform> roomWaypoints;
    
    /* Private Variables */

    
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
