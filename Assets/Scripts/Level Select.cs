using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private int levelIndex; // The scene index of this level button
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        // Get the highest unlocked level stored in PlayerPrefs
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        // Enable button only if this level is unlocked
        button.interactable = levelIndex <= highestLevel;
    }

    public void LoadLevel()
    {
        // Only runs if button is unlocked
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1f;
    }
}
