using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPointType
{
    None,
    PlayerSpawnPoint,
    EnemySpawnPoint,
    PowerupSpawnPoint
}
public class RegisterSpawnPoint : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public RoomData roomData;
    public SpawnPointType spawnPointType;

    /* Private Variables */

    private Transform spawnTf;  


    // Use this for initialization
    void Awake ()
    {
        spawnTf = this.gameObject.GetComponent<Transform>();
        roomData = spawnTf.GetComponentInParent<RoomData>();

        // Error message for designers
        if (spawnPointType == SpawnPointType.None)
        {
            Debug.LogWarning(string.Format("The spawn point {0} has no type selected, and was not registered to any list.", this.name));
        }

        RegisterThisSpawnPoint();
    }

    /// <summary>
    /// Adds this spawn point to the appropriate list in the GameManager.
    /// </summary>
    public void RegisterThisSpawnPoint()
    {
        bool alreadyRegistered = false;

        // Assigns this spawn point to the appropriate type
        switch (spawnPointType)
        {
            case SpawnPointType.PlayerSpawnPoint:
                foreach (var spawnPoint in GameManager.gm.playerSpawnPoints)
                {
                    // Checks to see if this spawn point is already in the GameManager list
                    if (spawnPoint == this.spawnTf)
                    {
                        alreadyRegistered = true;
                        break;
                    }
                }
                // Only adds the spawn point if it is not already in the list
                if (!alreadyRegistered)
                {
                    GameManager.gm.playerSpawnPoints.Add(spawnTf);
                }
                break;
            case SpawnPointType.EnemySpawnPoint:
                foreach (var spawnPoint in GameManager.gm.enemySpawnPoints)
                {
                    // Checks to see if this spawn point is already in the GameManager list
                    if (spawnPoint == this.spawnTf)
                    {
                        alreadyRegistered = true;
                        break;
                    }
                }
                // Only adds the spawn point if it is not already in the list
                if (!alreadyRegistered)
                {
                    GameManager.gm.enemySpawnPoints.Add(spawnTf);
                }
                break;
            case SpawnPointType.PowerupSpawnPoint:
                foreach (var spawnPoint in GameManager.gm.powerupSpawnPoints)
                {
                    // Checks to see if this spawn point is already in the GameManager list
                    if (spawnPoint == this.spawnTf)
                    {
                        alreadyRegistered = true;
                        break;
                    }
                }
                // Only adds the spawn point if it is not already in the list
                if (!alreadyRegistered)
                {
                    GameManager.gm.powerupSpawnPoints.Add(spawnTf);
                }
                break;
        }
    }
}
