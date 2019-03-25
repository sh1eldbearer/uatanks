using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is a simple noisemaker script for tanks. It will alert any nearby hostile tanks
 * if the attached tank performs an action that makes a noise.
 */

public class TankNoisemaker : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private TankData tankData; // The TankData component of this tank
    private float decayTimer = 0f; // The time since this tank has made sound

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the timer has reached zero, the tank is no longer making noise
        if (decayTimer <= 0f)
        {
            tankData.isMakingNoise = false;
        }
        else
        {
            decayTimer -= Time.deltaTime;
        }
	}

    /// <summary>
    /// Has the tank "make noise", so that enemy tanks can hear it
    /// </summary>
    public void MakeNoise()
    {
        tankData.isMakingNoise = true;
        decayTimer = tankData.noiseDecayTime;
    }
}
