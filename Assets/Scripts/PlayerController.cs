using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : PhysicsObject {

    [Tooltip("Allows the player to latch onto walls.")]
    public bool canWallLatch = true;
    private bool wallLatched = false;
    [Tooltip("Time before the player can wall latch again.")]
    public float wallLatchCooldown = 0.3f;
    private float wallLatchCooldownTimer = 0f;

    [Tooltip("Allows the player to cancel his jump forcing him down.")]
    public bool canCancelJump = true;
    [Tooltip("The horizontal move speed of the player.")]
    public float speed = 8f;
    [Tooltip("The vertical force upwards when the player jump.")]
    public float jumpVelocity = 14f;
    [Tooltip("The vertical force downwards when the player cancels his jump.")]
    public float jumpCancelVelocityModifier = 0.3f;
    private bool wasWallLatched;

    public VirtualInput vi;

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
        applyPhysics = !wallLatched;

        if (wallLatchCooldownTimer > 0 && !wallLatched)
        {
            wallLatchCooldownTimer -= Time.deltaTime;
        }

        // Play latch sound if just latched
        if (wallLatched)
        {
            if (!wasWallLatched)
            {
                audioSource.PlayOneShot(latchSound);
            }
        } else if (wallLatchCooldownTimer > 0)
        {
            wallLatchCooldownTimer -= Time.deltaTime;
        }
        wasWallLatched = wallLatched;


        // Compute Velocity
        Vector2 move = Vector2.zero;

        move.x = vi.GetAxisRaw(Axis.HORIZONTAL) * speed;

        if (vi.GetButtonDown(Button.JUMP) && (grounded || wallLatched))
        {
            velocity.y = jumpVelocity;

            wallLatchCooldownTimer = wallLatchCooldown;
            wallLatched = false;
            animator.SetBool("wallLatched", wallLatched);

            // Play jump sound
            audioSource.PlayOneShot(jumpSound);
        }
        else if (vi.GetButtonUp(Button.JUMP) && canCancelJump)
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
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / speed);

        targetVelocity = move;
    }

    protected override void OnHitWall()
    {
        // Latches onto wall
        if (canWallLatch && wallLatchCooldownTimer <= 0)
        {
            wallLatched = true;
            animator.SetBool("wallLatched", wallLatched);
            wallLatchCooldownTimer = wallLatchCooldown;
        }
    }
}
