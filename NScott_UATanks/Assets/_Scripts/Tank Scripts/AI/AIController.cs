using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerator for selecting tank personalities
public enum AIPersonality
{
    Standard, Coward, Captain, Reaper
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
    [HideInInspector] public RoomData roomData;
    [HideInInspector] public TankData tankData;
    [HideInInspector] public AIBehaviors behaviors;
    [HideInInspector] public AIVision vision;
    [HideInInspector] public AIHearing hearing;

    public AIPersonality personality;
    
    [Header("Vision Settings")]
    [Range(1f, 50f)] public float visionDistance = 10f;
    [Range(10f, 180f)] public float visionAngle = 90f;
    [Range(0.01f, 0.99f)] public float avoidanceRange = 0.5f;

    [Header("Hearing Settings")]
    [Range(2f, 50f)] public float hearingRadius = 10f;

    [Header("Target Information")]
    public Transform currentTarget;
    public Vector3 targetPosition;
    public TankData targetTankData;
    public bool canHearPlayer;

    [Header("Target Settings")]
    [HideInInspector] public float waypointCloseEnough = 2f;
    [HideInInspector] public float tankCloseEnough = 20f;
    [HideInInspector] public float maxPursuitTime = 3f;

    /* Private Variables */

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
        behaviors = this.gameObject.GetComponent<AIBehaviors>();
        vision = this.gameObject.GetComponent<AIVision>();
        hearing = this.gameObject.GetComponent<AIHearing>();
        roomData = this.gameObject.GetComponentInParent<RoomData>();
    }

    // Use this for initialization
    private void Start ()
    {
        waypointCloseEnough = GameManager.gm.waypointCloseEnoughDistance;
        tankCloseEnough = GameManager.gm.tankCloseEnoughDistance;
        maxPursuitTime = GameManager.gm.maxPursuitTime;
    }

    // Update is called once per frame
    private void Update ()
    {
        // Depending on the tank's personality, runs their programmed behavior
		switch (personality)
        {
            case AIPersonality.Standard:
                behaviors.Standard();
                break;
            case AIPersonality.Coward:
                behaviors.Coward();
                break;
            case AIPersonality.Captain:
                behaviors.Captain();
                break;
            case AIPersonality.Reaper:
                behaviors.Reaper();
                break;
        }
	}

    /// <summary>
    /// Sets the target and all related information of this AI tank.
    /// </summary>
    /// <param name="newTarget">The Transform component of the tank's new target.</param>
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        UpdateTargetPosition();
        targetTankData = newTarget.gameObject.GetComponent<TankData>();
    }

    /// <summary>
    /// Updates the position of this tank's current target.
    /// </summary>
    public void UpdateTargetPosition()
    {
        targetPosition = new Vector3(currentTarget.position.x, 
            tankData.tankTf.position.y, currentTarget.position.z);
    }

    /// <summary>
    /// Clears all information about this tank's current target.
    /// </summary>
    public void ClearAllTargetInfo()
    {
        ClearCurrentTarget();
        ClearTargetData();
        canHearPlayer = false;
    }

    /// <summary>
    /// Clears the transform and position of this tank's current target.
    /// </summary>
    public void ClearCurrentTarget()
    {
        currentTarget = null;
        targetPosition = Vector3.zero;
    }

    /// <summary>
    /// Clears the TankData component of this tank's current target (retains the transform
    /// and position of the target to allow this tank to investigate the last known position
    /// of the other tank).
    /// </summary>
    public void ClearTargetData()
    {
        targetTankData = null;
    }

    // Shows a visual representation of the size of this tank's cone of vision
    private void OnDrawGizmosSelected()
    {
        TankData tankData = this.gameObject.GetComponent<TankData>();
        Transform tankTf = this.gameObject.GetComponent<Transform>();
        AIVision vision = this.gameObject.GetComponent<AIVision>();            

        // Previews the tank's vision distance
        Gizmos.color = Color.red;
        Gizmos.DrawLine(tankTf.position, tankTf.position + tankTf.forward * visionDistance);
        Gizmos.DrawWireSphere(tankTf.position, hearingRadius);

        // Previews the tank's vision angle
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(tankTf.position, tankTf.position + vision.VectorFromForward(-visionAngle / 2) * visionAngle);
        Gizmos.DrawLine(tankTf.position, tankTf.position + vision.VectorFromForward(visionAngle / 2) * visionAngle);

        // Previews the tank's obstacle checking
        Gizmos.color = Color.white;
        Gizmos.DrawLine(tankData.leftRaycastTf.position, tankData.leftRaycastTf.position + tankData.leftRaycastTf.forward * (visionDistance * avoidanceRange));
        Gizmos.DrawLine(tankData.rightRaycastTf.position, tankData.rightRaycastTf.position + tankData.rightRaycastTf.forward * (visionDistance * avoidanceRange));
        Gizmos.DrawLine(tankData.originRayCastTf.position, tankData.originRayCastTf.position + tankData.originRayCastTf.forward * (visionDistance * avoidanceRange));
    }
}
