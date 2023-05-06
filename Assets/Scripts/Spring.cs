using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    
    private float bounce = 24f;
    public AudioClip springSound;
    private AudioSource springAudio;
    

    void Start()
    {
    
        springAudio = GetComponent<AudioSource>();

    }
    
    // CHECK IF SOMETHING COLLIDES WITH SPRING 
    private void OnCollisionEnter2D(Collision2D collision) 
    {
    
        // CHECK TO SEE IF OBJECT IS THE PLAYER
        if(collision.gameObject.CompareTag("Player"))
        {
        
            // APPLY AN UPWARD FORCE TO THE PLAYER BASED ON A VARIABLE
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            springAudio.PlayOneShot(springSound, 1.0f);

        }

    }

}
