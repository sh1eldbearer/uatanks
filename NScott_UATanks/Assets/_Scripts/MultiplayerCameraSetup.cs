using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCameraSetup : MonoBehaviour
{
    private void Awake()
    {
        // Singleton pattern
        if (GameManager.cameraSetup == null)
        {
            GameManager.cameraSetup = this;
        }
        else
        {
            Destroy(this);
        }

        ConfigureViewports();
    }

    public void ConfigureViewports()
    {
        // Single player
        if (GameManager.gm.players.Count == 1)
        {
            GameManager.gm.player1Camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
        // Multiplayer
        else if (GameManager.gm.players.Count == 2)
        {
            GameManager.gm.player1Camera.rect = new Rect(0, 0f, 0.5f, 1f);
            GameManager.gm.player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
    }

}
