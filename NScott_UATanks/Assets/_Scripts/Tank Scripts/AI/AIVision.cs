using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script allows an enemy (AI-controlled) tank to "see" a player tank.
 * 
 * It should be attached to any enemy tank object, and will automatically be 
 * attached if an AIController component is added to the tank first.
 */

public class AIVision : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private AIController controller;
    private TankData tankData;
    private Transform tankTf;

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
    }

    /// <summary>
    /// Checks to see if either of the players is within the vision range of this tank
    /// </summary>
    public void LookForATarget()
    {
        // Information about both players' visibility and distance in relation to this tank
        Vector3[] vectorToPlayers = { Vector3.zero, Vector3.zero };
        float[] angleToPlayers = { 0f, 0f };
        bool[] canSeePlayers = { false, false };

        foreach (var player in GameManager.gm.players)
        {
            int index = GameManager.gm.players.IndexOf(player);

            // Checks to see if the player is within this tank's cone of vision
            vectorToPlayers[index] = player.tankTf.position - tankTf.position;
            angleToPlayers[index] = Vector3.Angle(vectorToPlayers[index], tankTf.forward);

            if (angleToPlayers[index] < controller.visionAngle / 2)
            {
                // Looks to see if there is a direct line of sight to the player
                RaycastHit hitInfo;

                if (Physics.Raycast(tankTf.position, vectorToPlayers[index], out hitInfo, 
                    controller.visionDistance))
                {
                    if (hitInfo.collider.gameObject == player.gameObject)
                    {
                        // Tank has a direct line of sight to this player
                        canSeePlayers[index] = true;
                    }
                    else
                    {
                        // Tank does not have a direct line of sight to the player, or the 
                        // player is too far away
                        canSeePlayers[index] = false;
                    }
                }
                else
                {
                    // No other objects were seen by this tank
                    canSeePlayers[index] = false;
                }
            }
            else
            {
                // Player is outside of this tank's cone of vision
                canSeePlayers[index] = false;
            }
        }

        if (canSeePlayers[0] == true && canSeePlayers[1] == true)
        {
            // If both players are visible, uses whichever player is closest
            if (controller.behaviors.DistanceCheck(GameManager.gm.players[0].tankTf.position) < 
                controller.behaviors.DistanceCheck(GameManager.gm.players[1].tankTf.position))
            {
                controller.SetTarget(GameManager.gm.players[0].tankTf);
            }
            else
            {
                controller.SetTarget(GameManager.gm.players[1].tankTf);
            }
        }
        else if (canSeePlayers[0] == true)
        {
            // Only player 1 is visible
            controller.SetTarget(GameManager.gm.players[0].tankTf);
        }
        else if (canSeePlayers[1] == true)
        {
            // Only player 2 is visible
            controller.SetTarget(GameManager.gm.players[1].tankTf);
        }
        else
        {
            // No targets are visible
            controller.ClearTargetData();
        }
    }

    /// <summary>
    /// Checks for any obstacles in front of the tank by raycasting from an origin point to directly 
    /// in front of the tank.
    /// </summary>
    /// <param name="origin">The transform position from which to start the raycast.</param>
    /// <param name="distance">Returns the distance between the raycast origin point and the object
    /// hit by the raycast. Returns float.maxValue if no object was hit.</param>
    /// <param name="reduceDistance">Determines whether or not to reduce the distance at which a 
    /// raycast checks for a collision. Defaults to true.</param>
    /// <returns>If an object was hit by the raycast, returns true. If no object was hit, returns 
    /// false.</returns>
    private bool ObstacleRaycast(Transform origin, out float distance, bool reduceDistance = true)
    {
        RaycastHit hitInfo; // Stores information about the raycast collision

        if (reduceDistance)
        {
            Physics.Raycast(origin.position, origin.forward, out hitInfo, 
                controller.visionDistance * controller.avoidanceRange);
        }
        else
        {
            Physics.Raycast(origin.position, origin.forward, out hitInfo, 
                controller.visionDistance);
        }

        if (hitInfo.collider == null)
        {
            distance = float.MaxValue;
            return false;
        }
        else
        {
            distance = Vector3.Distance(origin.position, hitInfo.point);
            return true;
        }
    }

    /// <summary>
    /// Checks to see if there is a direct line of sight between this tank and its target.
    /// </summary>
    /// <param name="thisTankTf">The Transform component of this tank.</param>
    /// <param name="targetTf">The Transform component of the target.</param>
    /// <param name="visionDistance">The distance at which to check for a line of sight.</param>
    /// <param name="hitInfo">Returns information about the first object between this tank and
    /// its target.</param>
    /// <returns>Returns true if the first object seen is the target. Otherwise, returns false.</returns>
    public bool CanSeeTarget(Transform thisTankTf, Transform targetTf, float visionDistance, out RaycastHit hitInfo)
    {
        if (Physics.Raycast(thisTankTf.position, (targetTf.position - thisTankTf.position).normalized, out hitInfo, visionDistance))
        {
            if (hitInfo.collider.GetComponent<TankData>() == targetTf.GetComponent<TankData>())
            {
                // If the object seen is the target tank
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (targetTf.GetComponent<AddWaypointToList>() != null && hitInfo.collider == null)
            {
                // If the target is a waypoint, and there is nothing between the tank and the waypoint
                return true;
            }
            else
            {
                return false;
            }
        }
    }
       
    /// <summary>
    /// Checks for obstacles in front of the tank by raycasting forward from the left and right edges
    /// of the tank, as well as from the center of the tank object. Returns the distance between the
    /// raycast origin point and any collisions in front of it, or float.maxValue if no collision was
    /// detected.
    /// </summary>
    /// <param name="leftOrigin"></param>
    /// <param name="rightOrigin"></param>
    /// <param name="leftHitDistance"></param>
    /// <param name="rightHitDistance"></param>
    public void ObstacleCheck(Transform leftOrigin, Transform rightOrigin,
        out float leftHitDistance, out float rightHitDistance)
    {
        ObstacleRaycast(leftOrigin, out leftHitDistance);
        ObstacleRaycast(rightOrigin, out rightHitDistance);
    }

    /// <summary>
    /// Calculates a new Vector3 that is a x of degrees off of the object's forward axis
    /// </summary>
    /// <param name="angle">The angle between the outer edge of the the tank's cone of vision
    /// and the Vector3.Forward</param>
    /// <returns>The vector representing the outer edge of the tank's cone of vision.</returns>
    public Vector3 VectorFromForward(float angle)
    {
        Transform tankTf = this.gameObject.GetComponent<Transform>();
        angle += tankTf.localEulerAngles.z;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
