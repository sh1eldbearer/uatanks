using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPositioner : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private Transform listenerTf;

    private void Awake()
    {
        // Singleton pattern
        if (GameManager.listenerPositioner == null)
        {
            GameManager.listenerPositioner = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Component reference assignments
        listenerTf = this.gameObject.GetComponent<Transform>();
    }

    /// <summary>
    /// Changes the position of the audio listener in the scene based on the dimensions of the game map.
    /// </summary>
    public void PositionListener()
    {
        // Sets the default new position (for a map that is an odd number of room high/wide)
        Vector3 newPosition = new Vector3((GameManager.gm.mapWidth - 2) * GameManager.gm.roomSpacing, 0f,
            (GameManager.gm.mapHeight - 2) * GameManager.gm.roomSpacing);

        // Checks for any even-numbered map dimensions, and adjusts the new position
        if (GameManager.gm.mapWidth % 2 == 0)
        {
            newPosition.x -= GameManager.gm.roomSpacing / 2;
        }
        if (GameManager.gm.mapHeight % 2 == 0)
        {
            newPosition.z -= GameManager.gm.roomSpacing / 2;
        }

        // Applies the new position
        listenerTf.position = newPosition;
    }
}
