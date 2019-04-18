using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerator for allowing designers to configure input managers for each tank
public enum PlayerNumber
{
    Player1,
    Player2
}

[DisallowMultipleComponent]

/*
 * This script handles player input for the game.
 * An InputController component should be placed in an empty game object within the scene.
 * 
 * Use the playerNumber field in the Unity editor to determine which player the InputController will control.
 */

public class InputController : MonoBehaviour
{
    /* Public Variables */
    public PlayerNumber playerNumber;

    /* Private Variables */
    // References to two input controller objects for singleton pattern
    private static InputController p1Input;
    private static InputController p2Input; 

    private TankData tankData; // The TankData component of this tank object
    private TankMover tankMover; // The TankMover component of this tank object
    private TankShooter tankShooter; // The TankShooter component of this tank object    

    private void Awake()
    {
        // Singleton pattern for input controllers
        if (p1Input != null && p2Input != null)
        {
            Destroy(this.gameObject);
        }
        else if (playerNumber == PlayerNumber.Player1)
        {
            if (p1Input == null)
            {
                p1Input = this;
                GameManager.gm.p1InputController = this.gameObject;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (p1Input != null)
            {
                Debug.LogWarning(string.Format("Warning: multiple InputControllers detected for player 1.\n" +
                    "Please reassign {0} to player 2. Destroying {0}.", this.name));
                Destroy(this.gameObject);
            }
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            if (p2Input == null)
            {
                p2Input = this;
                GameManager.gm.p2InputController = this.gameObject;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (p2Input != null)
            {
                Debug.LogWarning(string.Format("Warning: multiple InputControllers detected for player 1.\n" +
                    "Please reassign {0} to player 1. Destroying {0}.", this.name));
                Destroy(this.gameObject);
            }
        }

        this.gameObject.SetActive(false);
    }

    // Use this for initialization
    private void Start ()
    {
        // Gets the tank data component for the appropriate tank
        if (playerNumber == PlayerNumber.Player1)
        {
            // There should always be at least one player in the scene, but we'll be cautious anyway
            try
            {
                // Assign this input controller to player 1's tank object
                SetTankComponentReferences(GameManager.gm.players[0]);
            }
            catch
            {
                // No player 1 object found
                Debug.Log("No player object found in scene.");
                return;
            }
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            // No second player in the scene until I know I have a working game first!
            try
            {
                // Assign this input controller to player 2's tank object
                SetTankComponentReferences(GameManager.gm.players[1]);
            }
            catch
            {
                // No player 2 object found
                Debug.Log("No second player object found in scene.");
                return;
            }
        }
    }
	
	// Update is called once per frame
	private void Update ()
    {
        if (playerNumber == PlayerNumber.Player1)
        {
            // Forward/backward movement
            if (Input.GetAxisRaw("P1_FwdBack") > 0)
            {
                tankMover.Move(Input.GetAxisRaw("P1_FwdBack"));
            }
            else if (Input.GetAxisRaw("P1_FwdBack") < 0)
            {
                // Tanks move slower in reverse than they do moving forward
                tankMover.Move(Input.GetAxisRaw("P1_FwdBack") * GameManager.gm.reverseSpeedRate);
            }
            // Rotation left/right
            if (Input.GetAxisRaw("P1_Rotate") != 0)
            {
                tankMover.Rotate(Input.GetAxisRaw("P1_Rotate"));
            }
            // Shooting
            if (Input.GetAxisRaw("P1_Fire") != 0)
            {
                // Fires a bullet
                tankShooter.FireBullet();
            }
            // TEMP: Camera adjust
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameManager.gm.p1RotateCamera = !GameManager.gm.p1RotateCamera;
            }
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            // Forward/backward movement
            if (Input.GetAxisRaw("P2_FwdBack") > 0)
            {
                tankMover.Move(Input.GetAxisRaw("P2_FwdBack"));
            }
            else if (Input.GetAxisRaw("P2_FwdBack") < 0)
            {
                // Tanks move slower in reverse than they do moving forward
                tankMover.Move(Input.GetAxisRaw("P2_FwdBack") * GameManager.gm.reverseSpeedRate);
            }
            // Rotation left/right
            if (Input.GetAxisRaw("P2_Rotate") != 0)
            {
                tankMover.Rotate(Input.GetAxisRaw("P2_Rotate"));
            }
            // Shooting
            if (Input.GetAxisRaw("P2_Fire") != 0)
            {
                // Fires a bullet
                tankShooter.FireBullet();
            }
        }
    }

    /// <summary>
    /// Sets up this InputController's references to the new tank
    /// </summary>
    /// <param name="newTankData">The TankData component of the new tank being controlled by this InputController.</param>
    public void SetTankComponentReferences(TankData newTankData)
    {
        tankData = newTankData;
        tankShooter = newTankData.tankShooter;
        tankMover = newTankData.tankMover;
    }
}
