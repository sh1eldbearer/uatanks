using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AIVision))]
[RequireComponent(typeof(AIHearing))]
[RequireComponent(typeof(AIBehaviors))]

/*
 * This script allows a designer to set the personality for an enemy (AI-controlled) tank.
 * 
 * It should be attached to any enemy tank object.
 */

public class AIController : MonoBehaviour
{
    // Enumerator for selecting tank personalities
    private enum AIPersonality
    {
        Standard, Coward, Mechanic, Captain
    }

    /* Public Variables */


    /* Private Variables */
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private AIPersonality personality;
#pragma warning restore IDE0044 // Add readonly modifier

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Depending on the tank's personality, runs their programmed behavior
		switch (personality)
        {
            case AIPersonality.Standard:
                break;
            case AIPersonality.Coward:
                break;
            case AIPersonality.Mechanic:
                break;
            case AIPersonality.Captain:
                break;
            default:
                break;
        }
	}
}
