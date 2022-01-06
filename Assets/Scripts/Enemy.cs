using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int damage = 1;
    [Range(0, 1)] public float lootDropChance;
    public GameObject lootPrefab;

    private float _attackTime = 0;
    private float _attackDelay = 0.5f;
    private bool _isAttacking = false;

    private void Start()
    {
        _entityRender = GetComponent<SpriteRenderer>();

        // Add health buff
        Health += Progression.EnemyBuff;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        _attackTime += Time.deltaTime;

        if (player != null)
        {
            // Deal damage to player if not already attacking
            if (!_isAttacking)
            {
                player.TakeDamage(damage);
                _attackTime = 0;
                _isAttacking = true;
            }
            
            // Deal damage to player after every delay
            if (_isAttacking && _attackTime >= _attackDelay)
            {
                player.TakeDamage(damage);
                _attackTime = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isAttacking = false;
    }

    public override void Die()
    {
        if (Random.value <= lootDropChance)
            Instantiate(lootPrefab, transform.position, Quaternion.identity);

        EnemySpawner.Instance.RemoveEnemy();
        base.Die();
    }
}