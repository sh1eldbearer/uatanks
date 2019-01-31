using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Adjusts the health display (the hearts) for this tank.
 *  
 * This script should be attached to the Canvas object that is a child of the tank object.
 */

public class DisplayHP : MonoBehaviour
{
    /* Public Variables */
    [Header("Sprites")]
    [Tooltip("The sprite used to indicate that a tank has a full hit point.")]
    public Sprite fullHeart;
    [Tooltip("The sprite used to indicate that a tank is missing a hit point.")]
    public Sprite emptyHeart;

    /* Private Variables */
    public List<Image> heartDisplays; // A list of the heart images attached to this tank's canvas
    private TankData tankData; // Used to check the current and maximum health of this tank

    private const int numOfHearts = 15; // The number of heart displays present on the canvas attached to this tank

    // Use this for initialization
    private void Start ()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponentInParent<TankData>();

        // Finds all heart images attached to this canvas, add them to the list, and initially disable them
		for (int count = 0; count < numOfHearts; count++)
        {
            heartDisplays.Add(this.gameObject.transform.Find("Heart " + (count + 1)).GetComponent<Image>());
            heartDisplays[count].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	private void Update ()
    {
        // Enables/disbles heart displays as needed, and displays appropriate images
		foreach (Image heart in heartDisplays)
        {
            int index = heartDisplays.IndexOf(heart) + 1;

            // If the player does not have this much maximum HP
            if (tankData.maxHP < index)
            {
                // Disable the heart display
                heart.gameObject.SetActive(false);
            }
            else
            {
                // If they do have enough max HP, enable the heart display
                if (heart.gameObject.activeInHierarchy == false)
                {
                    heart.gameObject.SetActive(true);
                }

                // If the player does not have enough current HP, show an empty heart
                if (tankData.currentHP < index)
                {
                    heart.sprite = emptyHeart;
                }
                else
                {
                    // Otherwise, show a full heart
                    heart.sprite = fullHeart;
                }
            }
        }
    }
}
