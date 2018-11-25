using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Possessable {

    [Tooltip("Decides how long the spikes are activated.")]
    public float Duration = .5f;
    protected float timeActive;

    public Sprite spriteOff;
    public Sprite spriteOn;
    public AudioClip activateSound;

    protected override void OnFixedUpdate()
    {
        if (timeActive < Duration)
        {
            timeActive += Time.fixedDeltaTime;
            if (timeActive > Duration)
                Deactivate();
        }
    }

    protected override void OnPlayerCollide(Player player)
    {
        if (Active)
            player.Damage();
    }

    protected override void Activate()
    {
        base.Activate();
        if (Active)
        {
            timeActive = 0f;

            spriteRenderer.sprite = spriteOn;
            audioSource.PlayOneShot(activateSound);

            foreach (Player player in collidingPlayers)
            {
                player.Damage();
            }
        }
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        spriteRenderer.sprite = spriteOff;
    }

    protected override bool CanActivate()
    {
        return base.CanActivate() && CooldownTimer <= 0;
    }
}
