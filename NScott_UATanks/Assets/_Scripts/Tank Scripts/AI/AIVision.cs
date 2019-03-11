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
    
    private Vector3[] vectorToPlayers = { Vector3.zero, Vector3.zero };
    private float[] angleToPlayers = { 0f, 0f };
    private bool[] canSeePlayers = { false, false };

    [SerializeField] private Vector3 leftPoint, rightPoint;

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

    // Checks to see if either of the players is within the vision range of this tank
    public void LookForTarget()
    {
        foreach (var player in GameManager.gm.players)
        {
            int index = GameManager.gm.players.IndexOf(player);

            vectorToPlayers[index] = player.tankTf.position - tankTf.position;
            angleToPlayers[index] = Vector3.Angle(vectorToPlayers[index], tankTf.forward);

            if (angleToPlayers[index] < controller.visionAngle / 2)
            {
                RaycastHit hitInfo;

                if (Physics.Raycast(tankTf.position, vectorToPlayers[index], out hitInfo, 
                    controller.visionDistance))
                {
                    if (hitInfo.collider.gameObject == player.gameObject)
                    {
                        canSeePlayers[index] = true;
                    }
                    else
                    {
                        canSeePlayers[index] = false;
                    }
                }
                else
                {
                    canSeePlayers[index] = false;
                }
            }
            else
            {
                canSeePlayers[index] = false;
            }
        }

        if (canSeePlayers[0] == true && canSeePlayers[1] == true)
        {
            // This shouldn't be running right now
        }
        else if (canSeePlayers[0] == true)
        {
            controller.SetTarget(GameManager.gm.players[0].tankTf);
        }
        else if (canSeePlayers[1] == true)
        {
            controller.SetTarget(GameManager.gm.players[1].tankTf);
        }
        else
        {
            controller.ClearTargetData();
        }
    }

    // Calculates a new Vector3 that is a x of degrees off of the object's forward axis
    public Vector3 VectorFromForward(float angle)
    {
        Transform tankTf = this.gameObject.GetComponent<Transform>();
        angle += tankTf.eulerAngles.z;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
