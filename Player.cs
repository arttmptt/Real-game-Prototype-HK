using UnityEngine;

public class Player : Entity
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayAnimation(string name, bool forcePlay = false)
    {
        if (!IsDead || forcePlay)
            _spriteAnimator.Play(name);
    }
}