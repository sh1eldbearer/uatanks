using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AdjustObstaclePositions : MonoBehaviour
{
    [Header("Ground")]
    public GameObject ground;

    [Header("Outer Walls")]
    public GameObject northOuterWall;
    public GameObject southOuterWall;
    public GameObject eastOuterWall;
    public GameObject westOuterWall;

    [Header("Large Inner Walls")]
    public GameObject largeNWInnerWall;
    public GameObject largeNEInnerWall;
    public GameObject largeSWInnerWall;
    public GameObject largeSEInnerWall;

    [Header("Small Inner Walls")]
    public GameObject smallNorthInnerWall;
    public GameObject smallSouthInnerWall;
    public GameObject smallEastInnerWall;
    public GameObject smallWestInnerWall;

    [Header("Positions/Scales")]
    public float groundScale;
    [Space]
    public float outerWallPosition;
    public float outerWallScale;
    [Space]
    public float largeInnerWallPosition;
    public float largeInnerWallScale;
    [Space]
    public float smallInnerWallPosition;
    public float smallInnerWallScale;

    protected enum XorZ { X, Z };

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform(ground, groundScale);

        // North and east are positive directions
        UpdateTransform(northOuterWall, outerWallScale, XorZ.Z, outerWallPosition, false);
        UpdateTransform(southOuterWall, outerWallScale, XorZ.Z, outerWallPosition, true);
        UpdateTransform(eastOuterWall, outerWallScale, XorZ.X, outerWallPosition, false);
        UpdateTransform(westOuterWall, outerWallScale, XorZ.X, outerWallPosition, true);

        UpdateTransform(smallNorthInnerWall, smallInnerWallScale, XorZ.Z, smallInnerWallPosition, false);
        UpdateTransform(smallSouthInnerWall, smallInnerWallScale, XorZ.Z, smallInnerWallPosition, true);
        UpdateTransform(smallEastInnerWall, smallInnerWallScale, XorZ.X, smallInnerWallPosition, false);
        UpdateTransform(smallWestInnerWall, smallInnerWallScale, XorZ.X, smallInnerWallPosition, true);

        UpdateTransform(largeNEInnerWall, largeInnerWallScale, largeInnerWallPosition, false, false);
        UpdateTransform(largeNWInnerWall, largeInnerWallScale, largeInnerWallPosition, true, false);
        UpdateTransform(largeSWInnerWall, largeInnerWallScale, largeInnerWallPosition, true, true);
        UpdateTransform(largeSEInnerWall, largeInnerWallScale, largeInnerWallPosition, false, true);
    }

    void UpdateTransform(GameObject gameObject, float scale)
    {
        Transform tf = gameObject.transform;
        tf.localScale = new Vector3(scale, 1f, scale);
    }

    void UpdateTransform(GameObject gameObject, float scale, XorZ xz, float positionValue, bool negativeDirection)
    {
        Transform tf = gameObject.transform;

        tf.localScale = new Vector3(tf.localScale.x, scale, tf.localScale.z);
        if (xz == XorZ.X)
        {
            if (negativeDirection)
            {
                tf.position = new Vector3(-positionValue, tf.position.y, tf.position.z);
            }
            else
            {
                tf.position = new Vector3(positionValue, tf.position.y, tf.position.z);
            }
        }
        else if (xz == XorZ.Z)
        {
            if (negativeDirection)
            {
                tf.position = new Vector3(tf.position.x, tf.position.y, -positionValue);
            }
            else
            {
                tf.position = new Vector3(tf.position.x, tf.position.y, positionValue);
            }
        }
    }

    void UpdateTransform(GameObject gameObject, float scale, float positionValue, bool negativeXDirection, bool negativeZDirection)
    {
        Transform tf = gameObject.transform;
        
        tf.localScale = new Vector3(tf.localScale.x, scale, tf.localScale.z);

        if (negativeXDirection)
        {
            if (negativeZDirection)
            {
                tf.position = new Vector3(-positionValue, tf.position.y, -positionValue);
            }
            else
            {
                tf.position = new Vector3(-positionValue, tf.position.y, positionValue);
            }
        }
        else
        {
            if (negativeZDirection)
            {
                tf.position = new Vector3(positionValue, tf.position.y, -positionValue);
            }
            else
            {
                tf.position = new Vector3(positionValue, tf.position.y, positionValue);
            }
        }
    }
}
