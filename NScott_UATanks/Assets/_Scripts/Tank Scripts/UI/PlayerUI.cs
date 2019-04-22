using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerNumber playerNumber;

    public Text scoreText;
    public Text livesText;

    private TankData tankData;

	// Use this for initialization
	void Start ()
    {
		if (playerNumber == PlayerNumber.Player1)
        {
            tankData = InputController.p1Input.tankData;
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            tankData = InputController.p2Input.tankData;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (tankData != null)
        {
            if (playerNumber == PlayerNumber.Player1)
            {
                scoreText.text = "Score: " + GameManager.gm.p1Score;
            }
            else if (playerNumber == PlayerNumber.Player2)
            {
                scoreText.text = "Score: " + GameManager.gm.p2Score;
            }
            livesText.text = "Lives: " + tankData.currentLives;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
	}
}
