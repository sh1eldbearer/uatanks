using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    public GravitySource currentAttractor; // The environment object currrently affecting the current object
    public Rigidbody objectRb; // The rigidbody attached to this game object

    void Awake()
    {
        objectRb = this.gameObject.GetComponent<Rigidbody>();
    }
	
	void LateUpdate ()
    {
        // As long as this object is affected by another object's gravity, allow this object to be pulled toward the gravity source
		if (currentAttractor != null && objectRb != null)
        {
            currentAttractor.Attract(objectRb);
        }
	}
}
