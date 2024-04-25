using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("House");
    }
    public void ExitGame()
    {
        // Only works when the game is built
        Application.Quit();
    }
}
