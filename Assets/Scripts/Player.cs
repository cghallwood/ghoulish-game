using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public StatBar healthBar;
    public MetricManager metrics;

    private AudioSource _playerAudio;

    private void Start()
    {
        _playerAudio = GetComponent<AudioSource>();
        _entityRender = GetComponent<SpriteRenderer>();

        healthBar.SetMaxValue(maxHealth);
    }

    public void PlaySound(AudioClip soundClip)
    {
        _playerAudio.PlayOneShot(soundClip);
    }

    public void Heal(int healAmount)
    {
        Health = Mathf.Clamp(Health + healAmount, 0, maxHealth);
        healthBar.SetValue(Health);
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
        healthBar.SetValue(Health);
    }

    public override void Die()
    {
        metrics.TrackPlayerDeath();
        Debug.Log("You died!");

        GameManager.Instance.GameOver(true);
        base.Die();
    }
}
