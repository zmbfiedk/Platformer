using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    public enum CoinType { Gold, Silver, Bronze }
    public CoinType coinType = CoinType.Bronze;

    public static event Action<CoinType> OnCoinCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke(coinType);
            Destroy(gameObject);
        }
    }
}
