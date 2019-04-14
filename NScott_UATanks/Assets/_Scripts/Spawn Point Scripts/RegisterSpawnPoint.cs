using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSpawnPoint : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public RoomData roomData;

    /* Private Variables */
    enum SpawnPointType
    {
        None,
        PlayerSpawnPoint,
        EnemySpawnPoint,
        PowerupSpawnPoint
    }

    private Transform spawnTf;
    #pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private SpawnPointType spawnPointType;
    #pragma warning restore IDE0044 // Add readonly modifier


    // Use this for initialization
    void Awake ()
    {
        spawnTf = this.gameObject.GetComponent<Transform>();
        roomData = spawnTf.GetComponentInParent<RoomData>();

        if (spawnPointType == SpawnPointType.None)
        {
            Debug.LogWarning(string.Format("The spawn point {0} has no type selected, and was not registered to any list.", this.name));
        }
        
        // Assigns this spawn point to the appropriate type
        switch (spawnPointType)
        {
            case SpawnPointType.PlayerSpawnPoint:
                GameManager.gm.playerSpawnPoints.Add(spawnTf);
                break;
            case SpawnPointType.EnemySpawnPoint:
                GameManager.gm.enemySpawnPoints.Add(spawnTf);
                break;
            case SpawnPointType.PowerupSpawnPoint:
                GameManager.gm.powerupSpawnPoints.Add(spawnTf);
                break;
        }
    }
}
