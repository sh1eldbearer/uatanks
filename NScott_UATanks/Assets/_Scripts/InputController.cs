using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public TankMover tankMover;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxisRaw("P1_FwdBack") != 0)
        {
            tankMover.Move(Input.GetAxisRaw("P1_FwdBack"));
        }
        if (Input.GetAxisRaw("P1_Rotate") != 0)
        {
            tankMover.Rotate(Input.GetAxisRaw("P1_Rotate"));
        }
    }
}
