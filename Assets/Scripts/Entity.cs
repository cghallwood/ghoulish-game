using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int Health { get; protected set; }

    public int maxHealth = 100;
    public ParticleSystem deathEffect;

    protected SpriteRenderer _entityRender;

    private bool _isFlashing;

    private void Awake()
    {
        _entityRender = GetComponent<SpriteRenderer>();

        Health = maxHealth;
        _isFlashing = false;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        Health = Mathf.Clamp(Health - damageAmount, 0, maxHealth);

        if (!_isFlashing)
            StartCoroutine(DamageFlash());

        if (Health <= 0)
            Die();
    }

    public virtual void Die()
    {
        GameObject deathEffectObject = Instantiate(deathEffect.gameObject, transform.position, deathEffect.transform.rotation);
        Destroy(deathEffectObject, 2f);
        Destroy(gameObject);
    }

    // Entity sprites flash when they take damage
    private IEnumerator DamageFlash()
    {
        Color origColor = _entityRender.color;

        _isFlashing = true;
        _entityRender.color = Color.grey;

        yield return new WaitForSeconds(0.35f);

        _isFlashing = false;
        _entityRender.color = origColor;
    }
}
