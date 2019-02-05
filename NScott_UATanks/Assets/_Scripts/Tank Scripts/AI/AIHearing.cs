using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script allows an enemy (AI-controlled) tank to "hear" a player tank.
 * 
 * It should be attached to any enemy tank object, and will automatically be 
 * attached if an AIController component is added to the tank first.
 */

public class AIHearing : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private AIController controller;
    private TankData tankData;

    private void Awake()
    {
        // Component reference assignments
        controller = this.gameObject.GetComponent<AIController>();
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    private void Start ()
    {
		
	}

    // Update is called once per frame
    private void Update ()
    {

	}
}
