using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Possessable : MonoBehaviour {

    [Tooltip("Time before a possessable can be activated again.")]
    public float Cooldown = 1f;
    private float cooldownTimer;
    public float CooldownTimer
    {
        get { return cooldownTimer; }
    }
    private bool active = false;
    public bool Active
    {
        get { return active; }
    }

    protected Player ghost;
    protected List<Player> collidingPlayers;

    protected Rigidbody2D rb;
    protected ContactFilter2D playerFilter; 
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;

    void Awake()
    {
        collidingPlayers = new List<Player>();

        rb = GetComponent<Rigidbody2D>();
        playerFilter = new ContactFilter2D { layerMask = 8 };
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
        if (cooldownTimer > 0 && !active)
        {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }

    protected virtual void OnFixedUpdate() {}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            collidingPlayers.Add(player);

            // Do nothing when not possessed
            if (ghost != null && active)
            {
                OnPlayerCollide(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            collidingPlayers.Remove(player);
        }
    }

    /// <summary>
    /// Gets called when a player enters this object's collider
    /// </summary>
    /// <param name="player">Colliding player</param>
    protected virtual void OnPlayerCollide(Player player) { }
    
    public virtual bool Possess(Player player)
    {
        if (ghost == null)
        {
            // Binds player to possessable
            ghost = player;

            // Changes color to player color
            Color newColor = player.Color;
            newColor.a = 1f;
            spriteRenderer.color = newColor;            
            
            return true;
        }

        return false;
    }

    public virtual bool Unpossess()
    {
        if (active)
            return false;

        // Unbinds player to possessable
        ghost = null;

        // Reverts color back to white
        spriteRenderer.color = Color.white;

        return true;
    }

    public virtual void Interact()
    {
        if (!Active)
        {
            Activate();
        }
    }

    protected virtual void Activate()
    {
        if (CanActivate())
        {
            // Resets cooldown timer and sets Active to true
            cooldownTimer = Cooldown;
            active = true;
        }
    }

    protected virtual void Deactivate()
    {
        active = false;
    }

    protected virtual bool CanActivate()
    {
        return ghost != null && !active;
    }
}
