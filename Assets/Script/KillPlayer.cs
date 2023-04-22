using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KillPlayer : MonoBehaviour
{
    
    // DECLARED IN UNITY. WILL RESPAWN USING THE SCENE INDEX BASED ON THE NUMBER PUT INTO UNITY.
    public int Respawn;
    
        
    void OnTriggerEnter2D(Collider2D other) 
    {
    
        // CHECKS FOR PLAYER TAG
        if(other.CompareTag("Player"))
        {
        
            // RESPAWNS THE PLAYER
            SceneManager.LoadScene(Respawn);

        }

    }

}
