using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContainer : MonoBehaviour
{

    private void Awake()
    {
        // Singleton pattern
        if (GameManager.gameContainer == null)
        {
            GameManager.gameContainer = this.gameObject.GetComponent<Transform>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
