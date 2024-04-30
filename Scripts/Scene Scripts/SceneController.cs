using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    
    }
    public void StartGame()
    {
        SceneManager.LoadScene("House");
    }
    public void ExitGame()
    {
        // Only works when the game is built
        Application.Quit();
    }
}
