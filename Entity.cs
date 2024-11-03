using System.Collections;
using SpriteAnimations;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int Money;
    public int TeamId;
    public bool IsDead;
    public bool IsInvincible;

    protected bool _facingRight = true;
    protected int _facingRightInt = 1;
    protected int _facingLeftInt = -1;

    protected SpriteAnimator _spriteAnimator;

    protected virtual void Awake()
    {
        _spriteAnimator = GetComponentInChildren<SpriteAnimator>();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }
}