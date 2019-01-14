using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public Vector3 centerOfGravity = Vector3.zero; // The source's center of gravity
    public float gravitationalConstant = -9.8f;

    public bool distanceWeakensPull = true; // When an object moves farther away from this gravity source, does this source's pull increase?

    private float sourceMass;

    // Use this for initialization
    void Start ()
    {
        sourceMass = this.gameObject.GetComponentInParent<Rigidbody>().mass;
	}

    private void OnTriggerEnter(Collider other)
    {
        ObjectGravity otherObjGravity = other.GetComponent<ObjectGravity>();
        
        if (otherObjGravity != null)
        {
            otherObjGravity.currentAttractor = this;
        }
    }

    public void Attract(Rigidbody attractedBody)
    {
        Vector3 pullVector = FindSurface(attractedBody.transform);
        OrientBody(attractedBody.transform, pullVector);

        float pullForce;
        if (distanceWeakensPull)
        {
            // Standard gravity
            // Inverse square law -> Gravitational constant * ((mass1 * mass2) / distance^2)
            pullForce = gravitationalConstant * ((sourceMass * attractedBody.mass) /
                Mathf.Pow(Vector3.Distance(this.transform.position + centerOfGravity, attractedBody.position), 2));
        }
        else
        {
            // Objects are pulled harder the further away from the object they are
            pullForce = gravitationalConstant * (sourceMass * attractedBody.mass) /
                Vector3.Distance(this.gameObject.transform.position + centerOfGravity, attractedBody.position);
        }

        pullVector = attractedBody.position - centerOfGravity;
        Debug.Log(pullVector + " " + pullForce);
        attractedBody.AddForce(pullVector.normalized * pullForce * Time.deltaTime);
    }


    private Vector3 FindSurface(Transform attractedBodyTf)
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, attractedBodyTf.position);
        Vector3 surfaceNormal = Vector3.zero;

        RaycastHit hitInfo;
        if (Physics.Raycast(attractedBodyTf.position, -attractedBodyTf.up, out hitInfo, distance))
        {
            surfaceNormal = hitInfo.normal;
        }
        return surfaceNormal;
    }

    // Aligns the attracted object to the gravity source's surface normal
    private void OrientBody(Transform attractedBodyTf, Vector3 surfaceNormal)
    {
        attractedBodyTf.localRotation = Quaternion.FromToRotation(attractedBodyTf.up, surfaceNormal) * attractedBodyTf.rotation;
    }
}
