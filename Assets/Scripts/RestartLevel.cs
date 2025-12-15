using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void RestartCurrentLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();             
        SceneManager.LoadScene(currentScene.name);                      
        Time.timeScale = 1f;                                           
    }
}
