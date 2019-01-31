using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Displays a visual representation of time left before a tank can fire again.
 * 
 * This script should be attached to the Canvas object that is a child of the tank object.
 */

public class DisplayFiringCooldown : MonoBehaviour
{
    /* Public Variables */
    public const float OVERFILL = 0.0001f;

    /* Private Variables */
    private TankData tankData; // Used to check the tank's firing cooldown
    private Slider slider; // Used to adjust the values of the slider object
    private Image fillArea; // Used to adjust the opacity of the slider based on the firing timer's cooldown

    private float fadeTimer; // The time remaining before the slider becomes completely transparent

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponentInParent<TankData>();
        slider = this.gameObject.GetComponent<Transform>().Find("Firing Cooldown Bar").GetComponent<Slider>();
        fillArea =slider.GetComponent<Transform>().Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    // Use this for initialization
    private void Start ()
    {
        // Sets the slider's minimum value to zero (mostly out of paranoia)
        slider.minValue = 0f;
    }
	
	private void LateUpdate ()
    {
        // Sets the slider's maximum value every LateUpdate (in case it changes via powerups)
        slider.maxValue = tankData.firingTimeout;

        // If the tank is waiting to be able to fire again
        if (tankData.firingTimer > 0f)
        {
            // Set the slider's value to slightly more than zero, so it will always display a tiny sliver once the firing cooldown ends
            slider.value = tankData.firingTimer + OVERFILL;
            // Maximize the fadeout timer of this bar
            fadeTimer = GameManager.gm.barFadeTime;
            // Make the slider fully opaque
            fillArea.color = new Color(fillArea.color.r, fillArea.color.g, fillArea.color.b, 1f);
        }
        // If the tank is able to fire again, and the fade out timer is still counting down
        else if (tankData.firingTimer <= 0f && fadeTimer > 0f)
        {
            // Lower the fade out timer
            fadeTimer -= Time.deltaTime;
            // Lower the opacity of the slider color so that it fades out as countdown gets closer to zero
            fillArea.color = new Color(fillArea.color.r, fillArea.color.g, fillArea.color.b, fadeTimer);
        }
        // The tank is able to fire and the fade out timer is at or below zero
        else
        {
            // Make the slider completely transparent
            fillArea.color = new Color(fillArea.color.r, fillArea.color.g, fillArea.color.b, 0f);
        }
    }
}
