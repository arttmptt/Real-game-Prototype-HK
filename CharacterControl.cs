using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public bool HLD; // testing HLD-like character control
    public float MovementSpeed;
    public float JumpingForce;
    public float JumpingLengthTime;
    public LayerMask GroundMask;

    bool _isGrounded;
    bool _isJumping;
    float _jumpingStartTime;
    float _playersMovementDirection;

    Player _player;
    Rigidbody2D _rigidBody;
    Collider2D _collider;
    PlayerControls _inputAction;

    void Start()
    {
        _player = GetComponent<Player>();
        _rigidBody = GetComponentInChildren<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();

        _inputAction = new PlayerControls();
        _inputAction.Enable();

        _inputAction.Ground.Walk.performed += moving =>
        {
            _playersMovementDirection = moving.ReadValue<float>();
        };
        _inputAction.Ground.Jump.performed += jumping => { StartJumping(); };
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapBox(transform.position, new Vector2(_collider.bounds.size.x * 0.82f, 0.125f), 0, GroundMask);

        // Walking
        bool movingToWall = false;
        List<ContactPoint2D> contacts = new();
        _collider.GetContacts(contacts);
        foreach (var contact in contacts)
        {
            if (!_isGrounded)
            {
                if ((contact.point.x < transform.position.x && _playersMovementDirection < 0)
                    || (contact.point.x > transform.position.x && _playersMovementDirection > 0))
                    movingToWall = true;
                if (_isJumping && contact.point.y > transform.position.y + _collider.bounds.size.y - 0.01f)
                    _isJumping = false;
            }
        }
        if (HLD)
            _rigidBody.velocity = _inputAction.Ground.WalkHLD.ReadValue<Vector2>() * MovementSpeed;
        else
            _rigidBody.velocity = new Vector2(!movingToWall ? (_playersMovementDirection * MovementSpeed) : 0, _rigidBody.velocity.y);

        // Jumping
        if (_isJumping && _inputAction.Ground.Jump.IsPressed())
        {
            if (Time.time - _jumpingStartTime < JumpingLengthTime)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, JumpingForce);
            }
            else _isJumping = false;
        }
        else _isJumping = false;

        // Animation
        if (_rigidBody.velocity.x != 0)
        {
            transform.localScale = new Vector2(_rigidBody.velocity.x > 0 ? 1 : -1, 1);
            _player.PlayAnimation("Walk");
        }
        else
        {
            if (_isJumping)
                _player.PlayAnimation("Walk");
            else
                _player.PlayAnimation("Idle");
        }
    }

    void StartJumping()
    {
        if (_isGrounded && !HLD)
        {
            _isJumping = true;
            _jumpingStartTime = Time.time;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, JumpingForce);
        }
    }
}