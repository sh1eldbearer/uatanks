using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * Handles the movement of camera objects.
 * This script should be attached to any camera objects in the game scene
 */

public class CameraController : MonoBehaviour
{
    /* Public Variables */
    [Tooltip("The game object this camera is going to follow.")]
    public GameObject followObject;

    /* Private Variables */
    private Camera thisCamera; // Easy reference to the camera component for determining which settings to check
    private Transform followTf, cameraTf; // Transform components for this camera and the object it's following
    private Vector3 offsetVector; // The position of the camera in relation to the object it's following
    private Quaternion initialRotation; // The initial rotation of the camera when the game starts
    private bool canRotate; // Holds the value of the appropriate rotation setting in the GameManager

    // Use this for initialization
    private void Start ()
    {
        // Component reference assignments
        thisCamera = this.gameObject.GetComponent<Camera>();
        followTf = followObject.GetComponent<Transform>();
        cameraTf = this.gameObject.GetComponent<Transform>();

        // Gets the camera's initial offset position
        offsetVector = cameraTf.position - followTf.position;

        // Gets the camera's inital rotation
        initialRotation = cameraTf.rotation;
    }
	
	// Update is called once per frame
	private void Update ()
    {
        // Checks to see which settings it should pull from the GameManager
        if (thisCamera == GameManager.gm.player1Camera)
        {
            canRotate = GameManager.gm.p1RotateCamera;
        }
        else if (thisCamera == GameManager.gm.player2Camera)
        {
            canRotate = GameManager.gm.p2RotateCamera;
        }
        else
        {
            Debug.LogError("Could not determine which player this camera is meant to follow.");
        }

        /* Camera rotation is a work in progress (will rework once improved tank model is in game) */
        if (canRotate)
        {
            if (cameraTf.parent == null)
            {
                // Sets the camera as a child of the object it's following
                cameraTf.parent = followObject.GetComponent<Transform>();

                // Determine how to scale the position of the camera relative to the scale of the object it's now attached to
                if (followTf.localScale.x >= 1)
                {
                    // Objects that are scaled up need the offset position to be scaled down
                    cameraTf.localPosition = Vector3.Scale(offsetVector, 
                        new Vector3(1 / followTf.localScale.x, 1 / followTf.localScale.y, 1 / followTf.localScale.z));
                }
                else if (followTf.localScale.x < 1)
                {
                    // Objects that are scaled down need the offset position to be scaled up
                    cameraTf.localPosition = Vector3.Scale(offsetVector, followTf.localScale);
                }

                // The camera's rotation needs to be set according to the local rotation
                cameraTf.localRotation = initialRotation;
            }
            else
            {
                // If the camera can rotate, and it already has a parent, no further action needs to be taken
            }
        }
        else
        {
            if (cameraTf.parent != null)
            {
                // Removes the camera as a child of the object it's supposed to be following
                cameraTf.parent = null;
                // Returns camera to its original rotation)
                cameraTf.rotation = initialRotation;
            }

            // Maintains the camera's original offset from the object it's following, wherever that object goes
            cameraTf.position = followTf.position + offsetVector;
        }
	}
}
