using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : MonoBehaviour
{
    private Transform tankTf;
    private CharacterController tankCc;
    private TankData tankData;

	void Awake ()
    {
        // Component reference assignmentss
        tankTf = this.gameObject.transform;
        tankCc = this.gameObject.GetComponent<CharacterController>();
        tankData = this.gameObject.GetComponent<TankData>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void Move(float axisValue)
    {
        tankCc.SimpleMove(tankTf.forward * axisValue * tankData.moveSpeed);
    }

    public void Rotate(float axisValue)
    {
        tankTf.Rotate(tankTf.transform.up * tankData.rotateSpeed * axisValue * Time.deltaTime, Space.Self);
    }
}
