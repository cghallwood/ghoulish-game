using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public static int Mana { get; set; }

    public int maxMana;
    public StatBar manaBar;
    public Weapon currentWeapon;
    public Spell currentSpell;
    public Transform weaponPos;
    public Transform attackPos;
    
    private AudioSource _loadoutAudio;
    private Animator _weaponAnim;
    private float _attackTime;
    private bool _isAttacking;

    private void Awake()
    {
        GameObject weapon = Instantiate(currentWeapon.weaponPrefab, transform.position, Quaternion.identity, weaponPos);

        _weaponAnim = weapon.gameObject.GetComponent<Animator>();
        _loadoutAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _attackTime = 0f;
        _isAttacking = false;

        Mana = maxMana;
        manaBar.SetMaxValue(maxMana);
    }

    private void Update()
    {
        if (_isAttacking)
        {
            _attackTime += Time.deltaTime;

            if (_attackTime >= currentWeapon.attackRate)
            {
                _isAttacking = false;
                _attackTime = 0f;
            }
        }

        if (!GameManager.isGameOver)
            CheckInput();
    }

    public void AddMana(int manaAmount)
    {
        Mana = Mathf.Clamp(Mana + manaAmount, 0, maxMana);
        manaBar.SetValue(Mana);
    }

    public void PlaySound(AudioClip soundClip)
    {
        _loadoutAudio.PlayOneShot(soundClip);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isAttacking)
            {
                _weaponAnim.SetTrigger("Attack");
                StartCoroutine(DelayAttack());
                _isAttacking = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            int manaDif = Mana - currentSpell.manaCost;

            // Check that the difference in Mana is less than zero
            if (manaDif < 0)
                return;

            else
                CastSpell();
        }
    }

    private void CastSpell()
    {
        PlaySound(currentSpell.castSound);
        currentSpell.Fire(attackPos);
        Mana -= currentSpell.manaCost;
        manaBar.SetValue(Mana);
    }

    // Delay attack frame to match up with the specific weapon animation 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(currentWeapon.attackDelay);
        PlaySound(currentWeapon.attackSound);
        currentWeapon.Attack(attackPos);
    }

    // Visual aid for attack radius of current weapon
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, currentWeapon.attackRadius);
    }
}
