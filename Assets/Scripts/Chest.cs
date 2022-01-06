using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public AudioClip lootSound;
    public Bonus[] possibleBonuses;

    private Bonus _currentBonus;
    private bool _isLooted;

    private void Start()
    {
        _isLooted = false;
        _currentBonus = CalculateBonus();
    }

    private void Update()
    {
        // Destroy if far enough away from player
        if (transform.position.x - PlayerController.Position.x <= -2)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        PlayerLoadout playerLoadout = collision.GetComponent<PlayerLoadout>();

        if (!_isLooted)
        {
            if (player != null && playerLoadout != null)
            {
                _currentBonus.ApplyBonus(player, playerLoadout);
                player.PlaySound(lootSound);
                _isLooted = true;
            }
        }
    }

    private Bonus CalculateBonus()
    {
        float totalWeight = 0f;
        float currentWeight = 0f;

        // Calculate total weight of all probabilities
        foreach (Bonus bonus in possibleBonuses)
        {
            totalWeight += bonus.chance;
        }

        float random = Random.Range(0f, totalWeight);

        // Calculate bonus given the probability of each
        for (int i = 0; i < possibleBonuses.Length; i++)
        {
            currentWeight += possibleBonuses[i].chance;

            if (random <= currentWeight)
                return possibleBonuses[i];
        }

        // Return null if no bonuses are calculated
        return null;
    }
}
