using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : PhysicsObject {

    public bool canCancelJump = true;
    public float speed = 8f;
    public float jumpVelocity = 14f;
    public float jumpCancelVelocityModifier = 0.3f;
    private bool wasWallLatched;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip latchSound;

    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void ComputeVelocity()
    {
        // Play latch sound if just latched
        if (!wasWallLatched && wallLatched)
        {
            audioSource.PlayOneShot(latchSound);
        }
        wasWallLatched = wallLatched;

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump") && (grounded || wallLatched))
        {
            wallLatchCooldownTimer = wallLatchCooldown;
            wallLatched = false;
            velocity.y = jumpVelocity;

            // Play jump sound
            audioSource.PlayOneShot(jumpSound);
        }
        else if (Input.GetButtonUp("Jump") && canCancelJump)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * jumpCancelVelocityModifier;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite && !wallLatched)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetBool("wallLatched", wallLatched);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / speed);

        targetVelocity = move;
    }
}
