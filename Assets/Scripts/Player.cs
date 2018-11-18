using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int maxHealth = 1;
    public int health;
    public bool isDead = false;

    private PlayerController aliveController;
    private GhostController deadController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // Use this for initialization
    void Awake () {
        aliveController = GetComponent<PlayerController>();
        deadController = GetComponent<GhostController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Damage(1);

        if (Input.GetKeyDown(KeyCode.R))
            Revive();
    }

    // Update is called once per frame
    void FixedUpdate () {
		if (!isDead && health <= 0)
        {
            ChangeOpacity(0.5f);
            aliveController.enabled = false;
            deadController.enabled = true;
            isDead = true;
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
            health = maxHealth;
            animator.SetBool("dead", isDead);
        }
    }

    public void Damage(int amount)
    {
        if (!isDead)
        {
            health -= amount;

            audioSource.PlayOneShot(hurtSound);
        }
    }

    private void ChangeOpacity(float opacity)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = Mathf.Clamp(opacity, 0, 1);
        spriteRenderer.color = newColor;
    }
}
