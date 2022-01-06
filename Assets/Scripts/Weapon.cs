using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public AudioClip attackSound;
    public LayerMask attackLayer;
    public int damage;
    public float attackForce;
    public float attackRadius;
    public float attackRate;
    public float attackDelay;

    // Deal damage to all enemies within a certain radius
    public void Attack(Transform attackPoint)
    {
        Collider2D[] entitiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, attackLayer);

        foreach (Collider2D entity in entitiesToDamage)
        {
            entity.GetComponent<Entity>().TakeDamage(damage);
            
            if (entity.CompareTag("Enemy"))
                entity.GetComponent<EnemyAI>().KnockBack(attackForce);
        }
    }
}
