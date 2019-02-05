using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles player input for the game.
 * An InputController component should be placed in an empty game object within the scene.
 * 
 * Use the playerNumber field in the Unity editor to determine which player the InputController will control.
 */

public class InputController : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    // References to two input controller objects for singleton pattern
    private static InputController p1Input;
    private static InputController p2Input; 

    private TankData tankData; // The TankData component of this tank object
    private TankMover tankMover; // The TankMover component of this tank object
    private TankShooter tankShooter; // The TankShooter component of this tank object

    // Enumerator for allowing designers to configure input managers for each tank
    private enum PlayerNumber { Player1, Player2 }
    [SerializeField] private PlayerNumber playerNumber;

    private void Awake()
    {
        // Singleton pattern for input controllers
        if (p1Input != null && p2Input == null)
        {
            Destroy(this.gameObject);
        }
        else if (playerNumber == PlayerNumber.Player1)
        {
            if (p1Input == null)
            {
                p1Input = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (p2Input == null)
            {
                Debug.LogWarning("Warning: multiple InputControllers detected for player 1.\n" +
                    "Please reassign " + this.name + " to player 2.");
                
            }
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            if (p2Input == null)
            {
                p2Input = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (p1Input == null)
            {
                Debug.LogWarning("Warning: multiple InputControllers detected for player 2.\n" +
                    "Please reassign " + this.name + " to player 1.");

            }
        }
    }

    // Use this for initialization
    private void Start ()
    {
        // Gets the tank data component for the appropriate tank
        // TODO: Need to figure out how to handle enemy tanks - will probably change this entirely
        if (playerNumber == PlayerNumber.Player1)
        {
            // There should always be at least one player in the scene, but we'll be cautious anyway
            try
            {
                // Assign this input controller to player 1's tank object
                tankData = GameManager.gm.players[0];
            }
            catch
            {
                // No player 1 object found
                Debug.Log("No player object found in scene.");
                return;
            }
            tankData = GameManager.gm.players[0];
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            // No second player in the scene until I know I have a working game first!
            try
            {
                // Assign this input controller to player 2's tank object
                tankData = GameManager.gm.players[1];
            }
            catch
            {
                // No player 2 object found
                Debug.Log("No second player object found in scene.");
                return;
            }
        }

        // Component reference assignments
        tankMover = tankData.gameObject.GetComponent<TankMover>();
        tankShooter = tankData.gameObject.GetComponent<TankShooter>();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        // TODO: Build dictionary and delegate system to allow keybind customization

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
                tankMover.Move(Input.GetAxisRaw("P1_FwdBack") * tankData.reverseSpeedRate);
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
                tankMover.Move(Input.GetAxisRaw("P2_FwdBack") * tankData.reverseSpeedRate);
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
}
