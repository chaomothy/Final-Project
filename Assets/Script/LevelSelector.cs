using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    
    public int level;

    public void OpenScene()
    {
    
        SceneManager.LoadScene("Level " + level.ToString());

    }

    public void OpenSandbox()
    {
    
        SceneManager.LoadScene(2);

    }

    public void BackToTitle() 
    {
    
        SceneManager.LoadScene(0);

    }

}
