using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * Forces the canvas object this script is attached to to rotate to match the facing of its 
 * assigned camera.
 * 
 * This script should be attached to the Canvas object that is a child of the tank object.
 * The Canvas' render mode should be set to World Space.
 */

public class CanvasController : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private TankData tankData; // Used to get the player number of this tank object
    private Transform canvasTf; // Used to adjust the rotation of this canvas object
    private Transform cameraTf; // Used to reduce GetComponent calls

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponentInParent<TankData>();
        canvasTf = this.gameObject.GetComponent<Transform>();
    }

	private void LateUpdate ()
    {
        // TODO: Will need to be reworked when multiple cameras are in play

        try
        {
            // If no camera has been set
            if (cameraTf == null)
            {
                // Determines which camera this canvas should match rotations with
                if (this.tag == "Player")
                {
                    switch (tankData.playerNumber)
                    {
                        case 1: // Player 1 camera
                            cameraTf = GameManager.gm.player1Camera.GetComponent<Transform>();
                            break;
                        case 2: // Player 2 camera
                            cameraTf = GameManager.gm.player2Camera.GetComponent<Transform>();
                            break;
                        default:
                            Debug.LogError("Could not find corresponding camera for this player.");
                            break;
                    }
                }
                else if (this.tag == "Enemy")
                {
                    cameraTf = GameManager.gm.player1Camera.GetComponent<Transform>();
                }
            }

            // Forces the canvas to face the assigned camera
            canvasTf.rotation = cameraTf.rotation;
        }
        catch { }
    }
}
