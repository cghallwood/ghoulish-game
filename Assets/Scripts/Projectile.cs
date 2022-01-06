using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public ParticleSystem impactEffect;
    public AudioClip impactSound;

    private Rigidbody2D _projectileRb;

    private void Start()
    {
        _projectileRb = GetComponent<Rigidbody2D>();

        _projectileRb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            entity.TakeDamage(damage);
            Impact();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Impact();
        }
    }

    public void Impact()
    {
        AudioSource.PlayClipAtPoint(impactSound, transform.position, 1.0f);
        GameObject impactObject = Instantiate(impactEffect.gameObject, transform.position, Quaternion.identity);
        Destroy(impactObject, 2f);
        Destroy(gameObject);
    }
}
