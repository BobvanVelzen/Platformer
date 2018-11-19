using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int maxHealth = 1;
    public int health;
    public bool isDead = false;
    public Color Color
    {
        get { return spriteRenderer.color; }
    }

    private VirtualInput vi;

    private PlayerController aliveController;
    private GhostController deadController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // Use this for initialization
    void Awake () {
        vi = new VirtualInput();

        aliveController = GetComponent<PlayerController>();
        deadController = GetComponent<GhostController>();
        aliveController.vi = vi;
        deadController.vi = vi;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        health = maxHealth;
    }
    
    void FixedUpdate () {
		if (!isDead && health <= 0)
        {
            ChangeOpacity(0.5f);
            aliveController.enabled = false;
            deadController.enabled = true;
            isDead = true;
            gameObject.layer = 9;
            animator.SetBool("dead", isDead);
        }
	}

    // TEMPORARY! Remove this later
    void Revive()
    {
        if (isDead)
        {
            ChangeOpacity(1f);
            aliveController.enabled = true;
            deadController.enabled = false;
            isDead = false;
            gameObject.layer = 8;
            health = maxHealth;
            animator.SetBool("dead", isDead);
        }
    }

    public void Damage(int amount = 1)
    {
        if (!isDead)
        {
            health -= amount;

            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void ChangeOpacity(float opacity)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = Mathf.Clamp(opacity, 0, 1);
        spriteRenderer.color = newColor;
    }
}
