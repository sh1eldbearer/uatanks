using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script allows an enemy (AI-controlled) tank to "see" a player tank.
 * 
 * It should be attached to any enemy tank object, and will automatically be 
 * attached if an AIController component is added to the tank first.
 */

public class AIVision : MonoBehaviour
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
        RaycastHit objectSeen;

        if (Physics.Raycast(tankTf.position, tankTf.forward, out objectSeen, controller.visionDistance))
        {
            if (objectSeen.collider.tag == "Player" && objectSeen.collider.GetComponent<TankData>())
            {
                controller.currentTarget = objectSeen.collider.GetComponent<Transform>();
            }
            else
            {
                controller.ClearTarget();
            }
        }
        else
        {
            controller.ClearTarget();
        }
    }
}
