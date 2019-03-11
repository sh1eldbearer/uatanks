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
    private AIVision vision;
    private AIHearing hearing;

    private void Awake()
    {
        // Component reference assignments
        controller = this.gameObject.GetComponent<AIController>();
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    private void Start()
    {
        // Component reference assignments dependant on other scripts
        tankTf = tankData.tankTf;
        vision = controller.vision;
        hearing = controller.hearing;
    }

    // Standard AI tank behavior
    public void Standard()
    {
        if (controller.targetData == null)
        {
            vision.LookForTarget();
            Patrol();
        }
        else
        {
            MoveTowardTarget(controller.tankCloseEnough);
            tankData.tankShooter.FireBullet();
        }
    }

    // Coward AI tank behavior
    public void Coward()
    {
        if (controller.targetData == null)
        {
            vision.LookForTarget();
            Patrol();
        }
        else
        {
            FleeFromTarget();
            if (DistanceCheck(controller.targetPosition) > controller.visionDistance)
            {
                controller.ClearTarget();
            }
        }
    }

    // Mechanic AI tank behavior
    public void Mechanic()
    {

    }

    // Captain AI tank behavior
    public void Captain()
    {

    }

    // Reaper AI tank behavior
    public void Reaper()
    {

    }

    public float DistanceCheck(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, tankTf.position);
    }

    public void MoveTowardTarget(float closeEnoughDistance)
    {
        if (controller.currentTarget != null)
        {
            controller.UpdateTargetPosition();
            Vector3 rotationVector = Vector3.Normalize(controller.targetPosition - tankTf.position);
            tankData.tankMover.Rotate(rotationVector);
            if (DistanceCheck(controller.targetPosition) > closeEnoughDistance)
            {
                if (tankTf.forward != rotationVector)
                {
                    tankData.tankMover.Move(1 * tankData.reverseSpeedRate);
                }
                else
                {
                    tankData.tankMover.Move(1);
                }
            }
        }
    }

    public void FleeFromTarget()
    {
        if (controller.currentTarget != null)
        {
            controller.SetTarget(controller.currentTarget);
            Vector3 fleeVector = Vector3.Normalize((controller.targetPosition - tankTf.position) * -1);
            tankData.tankMover.Rotate(fleeVector);
            if (tankTf.forward != fleeVector)
            {
                tankData.tankMover.Move(1 * tankData.reverseSpeedRate);
            }
            else
            {
                tankData.tankMover.Move(1);
            }
        }
    }

    public void Patrol()
    {
        if (controller.currentTarget == null)
        {
            Transform closestWaypoint = null;
            foreach (Transform waypoint in controller.roomData.roomWaypoints)
            {
                if (closestWaypoint == null)
                {
                    closestWaypoint = waypoint;
                }
                else
                {
                    if (DistanceCheck(waypoint.position) < DistanceCheck(closestWaypoint.position))
                    {
                        closestWaypoint = waypoint;
                    }
                }
            }
            try
            {
                controller.SetTarget(controller.roomData.roomWaypoints[controller.roomData.roomWaypoints.IndexOf(closestWaypoint) + 1]);
            }
            catch
            {
                controller.SetTarget(controller.roomData.roomWaypoints[0]);
            }
        }
        else if (DistanceCheck(controller.targetPosition) <= controller.waypointCloseEnough)
        {
            try
            {
                controller.SetTarget(controller.roomData.roomWaypoints[controller.roomData.roomWaypoints.IndexOf(controller.currentTarget) + 1]);
            }
            catch
            {
                controller.SetTarget(controller.roomData.roomWaypoints[0]);
            }
        }
        else
        {
            MoveTowardTarget(controller.waypointCloseEnough);
        }
    }
}
