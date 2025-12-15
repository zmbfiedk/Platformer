using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private GameObject _gameUi;

    [SerializeField] private GameObject[] SceneObjects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
            if (currentScene >= highestLevel)
                PlayerPrefs.SetInt("HighestLevel", currentScene + 1);


            int sessionCoins = PlayerPrefs.GetInt("SessionCoins", 0);
            int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            PlayerPrefs.SetInt("TotalCoins", totalCoins + sessionCoins);


            PlayerPrefs.Save();

            disableAllscene();

            Time.timeScale = 0f;
            _gameUi.SetActive(true);
        }
    }

    void disableAllscene()
    {
        foreach (GameObject obj in SceneObjects)
        {
            obj.SetActive(false);
        }
    }
}
