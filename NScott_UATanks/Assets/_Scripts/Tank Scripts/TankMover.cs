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
            tankData.moveSpeed);
    }

    // Rotates the tank around the y-axis
    public void Rotate(float axisValue)
    {
        tankData.tankTf.Rotate(tankData.tankTf.transform.up * tankData.rotateSpeed * 
            axisValue * Time.deltaTime, Space.Self);
    }

    // Rotates the tank toward a target vector
    public void Rotate(Vector3 targetVector)
    {
        tankData.tankTf.rotation = Quaternion.RotateTowards(tankData.tankTf.rotation,
            Quaternion.LookRotation(targetVector), tankData.rotateSpeed * Time.deltaTime);
        tankData.tankTf.eulerAngles = new Vector3(0f, tankData.tankTf.eulerAngles.y, 0f);
    }
}
