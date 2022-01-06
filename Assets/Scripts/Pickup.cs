using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Bonus bonus;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        PlayerLoadout playerLoadout = collision.gameObject.GetComponent<PlayerLoadout>();

        if (player != null)
        {
            bonus.ApplyBonus(player, playerLoadout);
            player.PlaySound(collectSound);
            Progression.Instance.SetBonus(bonus.rewardAmount, bonus.type.ToString());
            Destroy(gameObject);
        }
    }
}
