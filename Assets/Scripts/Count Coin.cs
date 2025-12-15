using TMPro;
using UnityEngine;

public class CountCoin : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); 
        Player.OnCoinPickup += UpdateUI;
    }

    void UpdateUI(int CoinValue)
    {
        textMesh.text = "Coins: " + CoinValue.ToString();
    }
}
