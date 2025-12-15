using TMPro;
using UnityEngine;

public class MainMenuCoins : MonoBehaviour
{
    TextMeshProUGUI textUI;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textUI.text = "Coins: " + PlayerPrefs.GetInt("Coins", 0).ToString();
    }
}
