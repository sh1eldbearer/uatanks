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
    private TankData tankData;

    private float decayTimer = 0f;

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
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
    /// 
    /// </summary>
    public void MakeNoise()
    {
        tankData.isMakingNoise = true;
        decayTimer = tankData.noiseDecayTime;
    }
}
