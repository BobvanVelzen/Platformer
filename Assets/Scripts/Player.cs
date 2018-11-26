using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int MaxHealth = 1;
    public int health;
    private bool isDead = false;
    public Color Color;

    // TODO: Revert back to private
    public bool ControllerAssigned = false;

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

        AssignVirtualInput(new VirtualInput());
        ControllerAssigned = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        health = MaxHealth;
    }
    
    void FixedUpdate () {
		if (!isDead && health <= 0)
        {
            ChangeOpacity(0.7f);
            aliveController.enabled = false;
            deadController.enabled = true;
            isDead = true;
            gameObject.layer = 9;
            spriteRenderer.sortingOrder = 2;
            animator.SetBool("dead", isDead);
            animator.SetTrigger("deathTrigger");
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

    public void AssignVirtualInput(VirtualInput virtualInput)
    {
        aliveController.vi = virtualInput;
        deadController.vi = virtualInput;

        ControllerAssigned = true;
    }
}
