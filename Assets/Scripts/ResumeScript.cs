using UnityEngine;

public class ResumeScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }
}
