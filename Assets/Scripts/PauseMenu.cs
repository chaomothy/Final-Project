using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public AudioSource music;
    
    
    void Start()
    {
    
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

    }    
    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
        
            if (GameIsPaused) 
            {
            
                Resume();
                music.pitch = 1.0f;
                music.volume = 0.05f;

            }
            else 
            {
            
                Pause();
                music.pitch = 0.9f;
                music.volume = 0.02f;
            
            }

        }

    }

    public void Resume()
    {
    
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    public void Pause()
    {
    
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    
    }


    public void ReturnToTitle()
    {
    
        SceneManager.LoadScene(0);
        GameIsPaused = false;
        music.pitch = 1.0f;
        music.volume = 0.05f;
        
    }

    public void LevelSelect() 
    {
    
        SceneManager.LoadScene(1);
        GameIsPaused = false;
        music.pitch = 1.0f;
        music.volume = 0.05f;

    }

    
}
