using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script defines all enemy (AI-controlled tank behaviors), allows the AI Controller 
 * to dictate which behavior is run.
 * 
 * It should be attached to any enemy tank object, and will automatically be 
 * attached if an AIController component is added to the tank first.
 */

public class AIBehaviors : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private AIController controller;
    private TankData tankData;
    private Transform tankTf;

    private void Awake()
    {
        // Component reference assignments
        controller = this.gameObject.GetComponent<AIController>();
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    private void Start ()
    {
        // Component reference assignments dependant on other scripts
        tankTf = tankData.tankTf;
    }

    // Update is called once per frame
    private void Update ()
    {
		if (controller.currentTarget != null)
        {
            tankData.tankMover.Rotate(Vector3.Angle(tankTf.position, controller.currentTarget.position));
        }
	}
}
