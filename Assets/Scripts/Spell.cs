using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject projectilePrefab;
    public AudioClip castSound;
    public int manaCost;
    public float fireRate;

    // Shoot projectile from player's firing position
    public void Fire(Transform firePoint)
    {
        GameObject projectileObject =  Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Destroy(projectileObject, 3f);
    }
}
