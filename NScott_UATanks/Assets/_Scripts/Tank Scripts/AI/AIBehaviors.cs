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

    private float pursuitTimer = 0;

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
        vision.LookForATarget();
        // If the tank doesn't see anything, listens for something
        if (controller.targetTankData == null)
        {
            hearing.Listen("Player");
        }

        RaycastHit hitInfo;
        // So long as the tank has no player tank currently targeted, patrol waypoints
        if (controller.targetTankData == null &&
            (controller.currentTarget == null || controller.currentTarget.gameObject.GetComponent<TankData>() == null))
        {
            Patrol();

        }
        else
        {
            // Check for direct line of sight to player
            if (!vision.CanSeeTarget(tankTf, controller.currentTarget, // No direct line of sight to target
                controller.visionDistance * controller.avoidanceRange, out hitInfo))
            {
                if (hitInfo.collider == null || // Target out of vision range, or can be seen
                    hitInfo.collider.gameObject == controller.currentTarget.gameObject)
                {
                    MoveTowardTarget(controller.tankCloseEnough); // Pursue target
                    if (vision.CanSeeTarget(tankTf, controller.currentTarget,
                        controller.visionDistance, out hitInfo)) // Checks one more time for a line of sight to the target
                    {
                        // If the target can be seen, start firing
                        tankData.tankShooter.FireBullet();
                    }
                }
                else // There is an obstacle between this tank and its target
                {
                    ObstacleAvoidance(); // Attempt to move around the obstacle
                    if (vision.CanSeeTarget(tankTf, controller.currentTarget,
                        controller.visionDistance, out hitInfo)) // Checks one more time for a line of sight to the target
                    {
                        // If the target can be seen, start firing
                        tankData.tankShooter.FireBullet();
                    }
                }
            }
            else // Direct line of sight to target
            {
                MoveTowardTarget(controller.tankCloseEnough); // Pursue target
                tankData.tankShooter.FireBullet(); // Start firing
            }
            UpdateTimer(); // Update pursuit timer
        }
    }

    /// <summary>
    /// Coward AI tank behavior
    /// </summary>
    public void Coward()
    {
        vision.LookForATarget();
        // If the tank doesn't see anything, listens for something
        if (controller.targetTankData == null)
        {
            hearing.Listen("Player");
        }

        // So long as the tank has no player tank currently targeted, patrol waypoints
        if (controller.targetTankData == null &&
            (controller.currentTarget == null || controller.currentTarget.gameObject.GetComponent<TankData>() == null))
        {
            Patrol();
        }
        else
        {
            FleeFromTarget();
        }
    }

    // Captain AI tank behavior
    public void Captain()
    {
        Standard();

        if (controller.targetTankData != null)
        {
            foreach (var enemy in GameManager.gm.enemies)
            {
                if (enemy == tankData)
                {
                    continue;
                }
                else
                {
                    enemy.GetComponent<AIController>().SetTarget(controller.currentTarget);
                }
            }
        }
    }

    /// <summary>
    /// Reaper AI tank behavior
    /// </summary>
    public void Reaper()
    {
        // Looks for the closest player
        float p1Distance, p2Distance;
        try
        {
            p1Distance = Vector3.Distance(tankTf.position, GameManager.gm.players[0].tankTf.position);
        }
        catch
        {
            p1Distance = float.MaxValue;
        }
        try
        {
            p2Distance = Vector3.Distance(tankTf.position, GameManager.gm.players[1].tankTf.position);
        }
        catch
        {
            p2Distance = float.MaxValue;
        }

        if (p1Distance < p2Distance)
        {
            controller.SetTarget(GameManager.gm.players[0].tankTf);
        }
        else
        {
            controller.SetTarget(GameManager.gm.players[1].tankTf);
        }

        // Check for direct line of sight to player
        RaycastHit hitInfo;
        if (!vision.CanSeeTarget(tankTf, controller.currentTarget, // No direct line of sight to target
            controller.visionDistance * controller.avoidanceRange, out hitInfo))
        {
            if (hitInfo.collider == null || // Target out of vision range, or can be seen
                hitInfo.collider.GetComponent<TankData>() == controller.targetTankData)
            {
                MoveTowardTarget(controller.tankCloseEnough); // Pursue target
                if (vision.CanSeeTarget(tankTf, controller.currentTarget, 
                    controller.visionDistance, out hitInfo)) // Checks one more time for a line of sight to the target
                {
                    // If the target can be seen, start firing
                    tankData.tankShooter.FireBullet();
                }
            }
            else // There is an obstacle between this tank and its target
            {
                ObstacleAvoidance(); // Attempt to move around the obstacle
            }
        }        
        else // Direct line of sight to target
        {
            MoveTowardTarget(controller.tankCloseEnough); // Pursue target
            tankData.tankShooter.FireBullet(); // Start firing
        }
    }

    /// <summary>
    /// Attempts to avoid any obstacles between this tank and its current target.
    /// </summary>
    public void ObstacleAvoidance()
    {
        float leftHitDistance, rightHitDistance;
        // Checks for collisions along the forward vector of the left and right side of the tank body
        vision.ObstacleCheck(tankData.leftRaycastTf, tankData.rightRaycastTf,
            out leftHitDistance, out rightHitDistance);
        // Compares the distances between the collisions
        if (leftHitDistance == float.MaxValue && rightHitDistance == float.MaxValue) // No collisions
        {
            // Simply move forward
            tankData.originRayCastTf.rotation = tankTf.rotation;
            tankData.tankMover.Move(1f);
        }
        else
        {
            if (leftHitDistance < rightHitDistance) // Obstacle is closer to the left side of the tank
            {
                // Rotates to the right until the tank has a clear path foward
                while (leftHitDistance != float.MaxValue || rightHitDistance != float.MaxValue)
                {
                    tankData.tankMover.RotatePart(tankData.originRayCastTf, 1f);
                    vision.ObstacleCheck(tankData.leftRaycastTf, tankData.rightRaycastTf,
                        out leftHitDistance, out rightHitDistance);
                    tankData.tankMover.Rotate(tankData.originRayCastTf.forward);
                }
            }
            else  // Obstacle is closer to the right side of the tank, or the distances are equal
            {
                // Rotates to the left until the tank has a clear path foward
                while (leftHitDistance != float.MaxValue || rightHitDistance != float.MaxValue)
                {
                    tankData.tankMover.RotatePart(tankData.originRayCastTf, -1f);
                    vision.ObstacleCheck(tankData.leftRaycastTf, tankData.rightRaycastTf,
                        out leftHitDistance, out rightHitDistance);
                    tankData.tankMover.Rotate(tankData.originRayCastTf.forward);
                }
            }
        }
    }

    /// <summary>
    /// Updates this tank's pursuit timer
    /// </summary>
    public void UpdateTimer()
    {
        // If this tank has no tank targeted
        if (controller.targetTankData == null)
        {
            // Increases the timer until the maximum tims
            if (CheckTimer())
            {
                controller.ClearAllTargetInfo();
            }
        }
        else
        {
            ResetTimer();
        }
    }

    /// <summary>
    /// Checks to see if this tank has reached or exceeded the maximum pursuit 
    /// time of a target it can't see
    /// </summary>
    /// <returns>Returns true if the timer has reached or exceed the maximum time,
    /// or false if it has not.</returns>
    public bool CheckTimer()
    {
        if (pursuitTimer < controller.maxPursuitTime)
        {
            pursuitTimer += Time.deltaTime;
            return false;
        }
        else
        {
            return true; // The timer has reached or exceed the maximum time
        }
    }

    /// <summary>
    /// Resets the timer to zero
    /// </summary>
    public void ResetTimer()
    {
        pursuitTimer = 0f;
    }

    /// <summary>
    /// Checks the the distance between this tank's position and another object's 
    /// position
    /// </summary>
    /// <param name="targetPosition">The target object to check for the distance 
    /// between it and this tank.</param>
    /// <returns>The distance between this ank and the target object.</returns>
    public float DistanceCheck(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, tankTf.position);
    }

    /// <summary>
    /// Moves toward the targeted object
    /// </summary>
    /// <param name="closeEnoughDistance">The proximity at which this tank will
    /// consider itself "close enough" to the target object.</param>
    public void MoveTowardTarget(float closeEnoughDistance)
    {
        controller.UpdateTargetPosition();
        // Gets a normalized vector toward the target object
        Vector3 rotationVector = Vector3.Normalize(controller.targetPosition - tankTf.position);
        tankData.tankMover.Rotate(rotationVector);

        // When this tank gets within a certain distance of the target object, stops moving
        if (DistanceCheck(controller.targetPosition) > closeEnoughDistance)
        {
            if (tankTf.forward != rotationVector)
            {
                tankData.tankMover.Move(1 * GameManager.gm.reverseSpeedRate);
            }
            else
            {
                tankData.tankMover.Move(1);
            }
        }
        else
        {
            // Too close; stop moving
        }
    }

    /// <summary>
    /// Moves away from the targeted object
    /// </summary>
    public void FleeFromTarget()
    {
        controller.UpdateTargetPosition();
        // Gets a normalized vector away from the target object
        Vector3 fleeVector = Vector3.Normalize((controller.targetPosition - tankTf.position) * -1);
        tankData.tankMover.Rotate(fleeVector);

        // When this tank gets a certain distance away from the target object, clears this tank's target
        if (DistanceCheck(controller.targetPosition) > controller.visionDistance)
        {
            controller.ClearAllTargetInfo();
        }
        else
        {
            if (tankTf.forward != fleeVector)
            {
                tankData.tankMover.Move(1 * GameManager.gm.reverseSpeedRate);
            }
            else
            {
                tankData.tankMover.Move(1);
            }
        }
    }

    /// <summary>
    /// Patrols between any waypoints in the room this tank starts in
    /// </summary>
    public void Patrol()
    {
        // When the tank has no target, find the next waypoint in the list after the closest one
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
            GetNextWaypoint(closestWaypoint);
        }
        // When the tank get close enough to its current waypoint, looks for the next waypoint
        else if (DistanceCheck(controller.targetPosition) <= controller.waypointCloseEnough)
        {
            GetNextWaypoint(controller.currentTarget);
        }
        // Otherwise, move toward the current waypoint
        else
        {
            RaycastHit hitInfo;
            if (vision.CanSeeTarget(tankTf, controller.currentTarget, controller.visionDistance * controller.avoidanceRange, out hitInfo))
            {
                MoveTowardTarget(controller.waypointCloseEnough);
            }
            else
            {
                ObstacleAvoidance();
            }
        }
    }

    // Gets the next waypoint in the waypoint list
    private void GetNextWaypoint(Transform currentWaypoint)
    {
        try
        {
            controller.SetTarget(controller.roomData.roomWaypoints[controller.roomData.roomWaypoints.IndexOf(currentWaypoint) + 1]);
        }
        catch
        {
            controller.SetTarget(controller.roomData.roomWaypoints[0]);
        }
    }
}
