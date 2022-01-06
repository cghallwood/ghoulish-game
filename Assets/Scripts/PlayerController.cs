using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Vector2 Position { get; private set; }

    public float horizontalSpeed = 0;
    public float verticalSpeed = 0;

    private Rigidbody2D _playerRb;
    private Animator _playerAnim;
    private float _lookDir;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isStunned;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();

        _isStunned = false;

        // Start by looking to the right
        _lookDir = 1;

        Position = _playerRb.position;
    }

    private void Update()
    {
        // Get player input every frame
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        ChangeAnimation();
    }

    private void FixedUpdate()
    {
        // Constrain player movement first to avoid jittery movement
        ConstrainPlayer();

        if (!GameManager.isGameOver)
            MovePlayer();

        Position = _playerRb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
            Progression.Instance.Escape();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!_isStunned)
            {
                horizontalSpeed /= 2;
                verticalSpeed /= 2;
                _isStunned = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_isStunned)
            {
                horizontalSpeed *= 2;
                verticalSpeed *= 2;
                _isStunned = false;
            }
        }
    }

    // Set animator parameters
    private void ChangeAnimation()
    {
        // Store current direction
        Vector2 moveDir = new Vector2(_horizontalInput, _verticalInput);

        // Change look direction from horizontal input
        if (_horizontalInput == -1 || _horizontalInput == 1)
            _lookDir = moveDir.x;

        // Change animations based on look direction and speed.
        _playerAnim.SetFloat("X Input", _lookDir);
        _playerAnim.SetFloat("Speed", moveDir.magnitude);
    }

    private void ConstrainPlayer()
    {
        float leftBound = CameraMovement.Position.x - 1.5f;
        float rightBound = CameraMovement.Position.x + 1.5f;
        float topBound = -0.35f;
        float bottomBound = -0.9f;

        // Check if player position goes out of bounds
        if (_playerRb.position.x <= leftBound)
            _playerRb.position = new Vector2(leftBound, _playerRb.position.y);

        else if (_playerRb.position.x >= rightBound)
            _playerRb.position = new Vector2(rightBound, _playerRb.position.y);


        if (_playerRb.position.y >= topBound)
            _playerRb.position = new Vector2(_playerRb.position.x, topBound);

        else if (_playerRb.position.y <= bottomBound)
            _playerRb.position = new Vector2(_playerRb.position.x, bottomBound);
    }

    // Move position of player
    private void MovePlayer()
    {
        // Get current position of player
        Vector2 currentPos = _playerRb.position;

        // Modify position.
        currentPos.x += horizontalSpeed * _horizontalInput * Time.deltaTime;
        currentPos.y += verticalSpeed * _verticalInput * Time.deltaTime;

        // Move to the new position.
        _playerRb.MovePosition(currentPos);
    }
}
