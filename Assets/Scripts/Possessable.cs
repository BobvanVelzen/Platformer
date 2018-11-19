using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour {

    public float duration = .5f;
    public float cooldown = 1f;
    private float cooldownTimer;
    private float timeActive;
    private bool active = false;

    public Player ghost;
    public Sprite spriteOff;
    public Sprite spriteOn;

    private Rigidbody2D rb;
    private ContactFilter2D playerFilter; 
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public AudioClip activateSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFilter = new ContactFilter2D { layerMask = 8 };
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (timeActive < duration)
        {
            timeActive += Time.fixedDeltaTime;
            if (timeActive > duration)
                Deactivate();
        }

        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do nothing when not possessed
        if (ghost == null || !active)
            return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && !player.isDead)
            player.Damage();
    }

    public bool Possess(Player player)
    {
        if (ghost == null)
        {
            Color newColor = player.Color;
            newColor.a = 1f;
            spriteRenderer.color = newColor;            

            ghost = player;

            return true;
        }

        return false;
    }

    public void Unpossess()
    {
        ghost = null;
        spriteRenderer.color = Color.white;
    }

    public void Activate()
    {
        if (CanActivate())
        {
            spriteRenderer.sprite = spriteOn;
            cooldownTimer = cooldown;
            timeActive = 0f;
            active = true;

            audioSource.PlayOneShot(activateSound);

            Collider2D[] hits = new Collider2D[16];
            int count = rb.OverlapCollider(playerFilter, hits);

            for (int i = 0; i < count; i++)
            {
                Player p = hits[i].GetComponent<Player>();
                if (p != null)
                {
                    p.Damage();
                }
            }
        }
    }

    private void Deactivate()
    {
        spriteRenderer.sprite = spriteOff;
        active = false;
    }

    private bool CanActivate()
    {
        return ghost != null && cooldownTimer <= 0;
    }
}
