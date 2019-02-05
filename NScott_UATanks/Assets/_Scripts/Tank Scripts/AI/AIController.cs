using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerator for selecting tank personalities
public enum AIPersonality
{
    Standard, Coward, Mechanic, Captain, Reaper
}

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
    /* Public Variables */
    [HideInInspector] public TankData tankData;
    [HideInInspector] public AIVision vision;
    [HideInInspector] public AIHearing hearing;

    [Header("Vision Settings")]
    [Range(1f, 50f)] public float visionDistance = 10f;

    public Transform currentTarget;

    /* Private Variables */
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private AIPersonality personality;
#pragma warning restore IDE0044 // Add readonly modifier

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
        vision = this.gameObject.GetComponent<AIVision>();
        hearing = this.gameObject.GetComponent<AIHearing>();
    }

    // Use this for initialization
    private void Start ()
    {
		
	}

    // Update is called once per frame
    private void Update ()
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
            case AIPersonality.Reaper:
                break;
        }
	}

    public void ClearTarget()
    {
        currentTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        Transform tankTf = this.gameObject.GetComponent<Transform>();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(tankTf.position, tankTf.position + tankTf.forward * visionDistance);
    }
}
