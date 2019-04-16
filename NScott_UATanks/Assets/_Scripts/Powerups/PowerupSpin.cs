using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpin : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private Transform thisTf;
    private Vector3 startingPosition;
    private Vector3 newPosition;

    // Use this for initialization
    void Start ()
    {
        // Component reference assignments
        thisTf = this.gameObject.GetComponent<Transform>();
        // Stores the original position of the powerup, for reference later
        startingPosition = thisTf.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Rotates the powerup
        thisTf.Rotate(thisTf.up, GameManager.gm.powerupRotateSpeed * Time.deltaTime);

        // Bounces the powerup up and down
        newPosition = startingPosition;
        newPosition.y = startingPosition.y + GameManager.gm.powerupBounceAmplitude * 
            Mathf.Sin(GameManager.gm.powerupBounceSpeed* Time.time);
        thisTf.position = newPosition;
	}
}
