using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the movement behaviors for tank objects.
 * 
 * This script should be attached to a tank object, and will automatically be attached if 
 * a TankData component is added to the tank first.
 */

public class TankMover : MonoBehaviour
{
    private TankData tankData;

	private void Awake ()
    {
        // Component reference assignmentss
        tankData = this.gameObject.GetComponent<TankData>();
	}

    public void Move(float axisValue)
    {
        tankData.tankCc.SimpleMove(tankData.tankTf.forward * axisValue * tankData.moveSpeed);
    }

    public void Rotate(float axisValue)
    {
        tankData.tankTf.Rotate(tankData.tankTf.transform.up * tankData.rotateSpeed * axisValue * Time.deltaTime, Space.Self);
    }
}
