using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;
    private static Music _instance;
    

    private void Awake()
    {
         
        if(!_instance)
        {
        
            _instance = this;

        }
        else
        {
        
            Destroy(this.gameObject);

        }
        
        
        DontDestroyOnLoad(transform.gameObject);
         _audioSource = GetComponent<AudioSource>();
    }
 

    public void PlayMusic()
    {
         if (_audioSource.isPlaying) return;
         _audioSource.Play();
    }
 

    public void StopMusic()
    {
         _audioSource.Stop();
    }
}
