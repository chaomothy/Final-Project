using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
    
        SceneManager.LoadScene(3);

    }

    public void LevelSelect() 
    {
    
        SceneManager.LoadScene(1);

    }
    
    public void QuitGame()
    {
    
        Application.Quit();
    
    }

    public void ReturnToTitle()
    {
    
        SceneManager.LoadScene(0);

    }


}
