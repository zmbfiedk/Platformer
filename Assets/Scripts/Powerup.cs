using System;
using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public static event Action OnPowerupPickup;

    [Header("Powerup Settings")]
    [SerializeField] private float respawnTime = 5f;

    public enum PowerupType
    {
        Invincibility,
        ExtraJump,
        HealthPack,
    }

    [SerializeField] private PowerupType powerupType;
    [SerializeField] private int healthAmount = 20;
    [SerializeField] private float invincibilityDuration = 3f;

    private Collider2D col;
    private SpriteRenderer sprite;
    private bool collected;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;
        if (!collision.CompareTag("Player")) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        collected = true;
        col.enabled = false;
        sprite.enabled = false;

        ApplyPowerup(player);
        OnPowerupPickup?.Invoke();

        StartCoroutine(Respawn());
    }

    private void ApplyPowerup(Player player)
    {
        switch (powerupType)
        {
            case PowerupType.Invincibility:
                player.SetInvincible(invincibilityDuration);
                break;

            case PowerupType.ExtraJump:
                player.AddExtraJump(1);
                break;

            case PowerupType.HealthPack:
                player.Heal(healthAmount);
                break;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        collected = false;
        col.enabled = true;
        sprite.enabled = true;
    }
}
