using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]

/*
 * This script automatically updates the materials displayed by a tank object when its personality
 * is changed the in the AIController script.
 * 
 * It will be automatically attached when an AIController script is attached to a tank object.
 */

public class TankUpdateMaterials : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public MeshRenderer bodyShell;
    [HideInInspector] public MeshRenderer cannonBase;
    [HideInInspector] public MeshRenderer cannonBarrel;

    /* Private Variables */
    [SerializeField] private AIController controller;

	// Use this for initialization

	void Start ()
    {
        // Component reference assignments
        controller = this.gameObject.GetComponent<AIController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        try
        {
            switch (controller.personality)
            {
                case AIPersonality.Standard:

                    bodyShell.material = GameManager.gm.enemyStandardBody;
                    cannonBase.material = GameManager.gm.enemyStandardCannon;
                    cannonBarrel.material = GameManager.gm.enemyStandardCannon;
                    break;
                case AIPersonality.Coward:
                    bodyShell.material = GameManager.gm.enemyCowardBody;
                    cannonBase.material = GameManager.gm.enemyCowardCannon;
                    cannonBarrel.material = GameManager.gm.enemyCowardCannon;
                    break;
                case AIPersonality.Reaper:
                    bodyShell.material = GameManager.gm.enemyReaperBody;
                    cannonBase.material = GameManager.gm.enemyReaperCannon;
                    cannonBarrel.material = GameManager.gm.enemyReaperCannon;
                    break;
                case AIPersonality.Captain:
                    bodyShell.material = GameManager.gm.enemyCaptainBody;
                    cannonBase.material = GameManager.gm.enemyCaptainCannon;
                    cannonBarrel.material = GameManager.gm.enemyCaptainCannon;
                    break;
            }
        }
        catch
        {
            Debug.LogWarning("Tank materials not found. Don't panic - this message will disappear after the current code base has been run once.");
        }
    }

    
}
