using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script handles the movement behaviors for tank objects.
 * 
 * This script should be attached to a tank object, and will automatically be 
 * attached if a TankData component is added to the tank first.
 */

public class TankMover : MonoBehaviour
{
    /* Public Variables */
    
    /* Private Variables */
    private TankData tankData;

    // Update is called once per frame
    private void Awake ()
    {
        // Component reference assignmentss
        tankData = this.gameObject.GetComponent<TankData>();
	}

    // Moves the tank forward
    public void Move(float axisValue)
    {
        tankData.tankCc.SimpleMove(tankData.tankTf.forward * axisValue * 
            tankData.currentMoveSpeed);
    }

    // Rotates the tank around the y-axis
    public void Rotate(float axisValue)
    {
        tankData.tankTf.Rotate(tankData.tankTf.transform.up * tankData.currentTurnSpeed * 
            axisValue * Time.deltaTime, Space.Self);
    }

    // Rotates the tank toward a target vector
    public void Rotate(Vector3 targetVector)
    {
        tankData.tankTf.rotation = Quaternion.RotateTowards(tankData.tankTf.rotation,
            Quaternion.LookRotation(targetVector), tankData.currentTurnSpeed * Time.deltaTime);
        tankData.tankTf.eulerAngles = new Vector3(0f, tankData.tankTf.eulerAngles.y, 0f);
    }

    // Rotates a part of the tank around the y-axis
    public void RotatePart(Transform rotateTf, float axisValue)
    {
        rotateTf.Rotate(rotateTf.transform.up * tankData.currentTurnSpeed *
            axisValue * Time.deltaTime, Space.Self);
    }

    // Rotates a part of the tank toward a target vector
    public void RotatePart(Transform rotateTf, Vector3 targetVector)
    {
        rotateTf.rotation = Quaternion.RotateTowards(rotateTf.rotation,
            Quaternion.LookRotation(targetVector), tankData.currentTurnSpeed * Time.deltaTime);
        rotateTf.eulerAngles = new Vector3(0f, rotateTf.eulerAngles.y, 0f);
    }
}
