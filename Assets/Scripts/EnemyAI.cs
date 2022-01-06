using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;
    public float pauseTime;
    public float repelRange;
    public float repelAmount;

    [Header("Shooting")]
    public bool isShooter;
    public float fireRate;
    public float shootTime;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private static List<Rigidbody2D> _EnemyRbs;

    private Rigidbody2D _enemyRb;
    private Animator _enemyAnim;
    private bool _isMoving;
    private bool _isAttacking;
    private bool _isStunned;
    private float _attackTime;

    private void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAnim = GetComponent<Animator>();

        _isMoving = true;
        _attackTime = 0;
        _isAttacking = false;
        _isStunned = false;

        if (_EnemyRbs == null)
            _EnemyRbs = new List<Rigidbody2D>();

        _EnemyRbs.Add(_enemyRb);

        StartCoroutine(PauseEnemy());
    }

    private void Update()
    {
        if (isShooter)
        {
            _attackTime += Time.deltaTime;
            if (_attackTime >= fireRate)
            {
                StartCoroutine(Shoot());
                _attackTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(_enemyRb.position, CameraMovement.Position);

        // Destory enemy if camera is far enough away
        if (distance > 3)
        {
            EnemySpawner.EnemyCount--;
            Destroy(gameObject);
        }
        
        // Move enemy
        if (_isMoving && !GameManager.isGameOver)
            MoveEnemy();
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            _isMoving = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isAttacking && !_isStunned)
            _isMoving = true;
    }

    private void OnDestroy()
    {
        _EnemyRbs.Remove(_enemyRb);
    }

    public void KnockBack(float force)
    {
        StartCoroutine(StunEnemy());
        Vector2 hitDir = (_enemyRb.position - PlayerController.Position).normalized;
        _enemyRb.AddForce(hitDir * force, ForceMode2D.Impulse);
    }

    private IEnumerator StunEnemy()
    {
        _isMoving = false;
        _isStunned = true;
        yield return new WaitForSeconds(.5f);
        _isStunned = false;

        if (!_isAttacking)
            _isMoving = true;
    }

    private IEnumerator Shoot()
    {
        _isMoving = false;
        _isAttacking = true;
        _enemyAnim.SetTrigger("Shoot");

        yield return new WaitForSeconds(shootTime);

        _isMoving = true;
        _isAttacking = false;

        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Destroy(projectileObject, 10f);
    }

    // Prevent collisions and movement from enemy while spawning
    private IEnumerator PauseEnemy()
    {
        _enemyRb.simulated = false;
        yield return new WaitForSeconds(pauseTime);
        _enemyRb.simulated = true;
    }

    private void MoveEnemy()
    {
        Vector2 direction = (PlayerController.Position - _enemyRb.position).normalized;
        Vector2 repelForce = Vector2.zero;

        // Determine amount of repel force to prevent enemy clustering
        foreach (Rigidbody2D rb in _EnemyRbs)
        {
            if (rb == _enemyRb)
                continue;

            if (Vector2.Distance(rb.position, _enemyRb.position) <= repelRange)
            {
                Vector2 repelDir = (_enemyRb.position - rb.position).normalized;
                repelForce += repelDir;
            }
        }

        // Set a new position based on direction to player and repel force
        Vector2 newPos = _enemyRb.position + direction * Time.fixedDeltaTime * moveSpeed;
        newPos += repelForce * Time.fixedDeltaTime * repelAmount;

        // Set movement animation float param on x-axis to direction.x
        _enemyAnim.SetFloat("X Input", direction.x);

        _enemyRb.MovePosition(newPos);
    }
}
