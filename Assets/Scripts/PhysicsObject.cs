using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour {

    protected const float minMoveDistance = .001f;
    protected const float shellRadius = .01f;
    public float wallLatchCooldown = 0.3f;
    protected float wallLatchCooldownTimer = 0f;
    protected float minGroundNormalY = .65f;
    protected float maxWallNormalY = -.65f;
    public float gravityModifier = 1f;
    public bool canWallLatch = false;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected bool wallLatched = false;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Sets what object to ignore when colliding
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity() { }

    void FixedUpdate()
    {
        // Applies gravity to velocity
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;
        if (wallLatchCooldownTimer > 0)
        {
            wallLatchCooldownTimer -= Time.fixedDeltaTime;
        }
        
        // Calculates where the object will be next update
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }
    
    void Movement(Vector2 move, bool yMovement)
    {
        if (wallLatched)
            return;

        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            // Gets all colliders which the object will hit after moving
            int count = rb.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < count; i++)
            {
                Vector2 currentNormal = hitBuffer[i].normal;
                // Set grounded to true if angle smaller than the allowed angle;
                if (currentNormal.y >= minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                // Latches onto wall
                else if (canWallLatch && wallLatchCooldownTimer <= 0 && currentNormal.y > maxWallNormalY && velocity.y < -0.6f)
                {
                    wallLatched = true;
                    wallLatchCooldownTimer = wallLatchCooldown;
                }

                // Checks if velocity needs to be adjusted to not collide with other colliders
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity -= projection * currentNormal;
                }
                float modifiedDistance = hitBuffer[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }        

        rb.position = rb.position + move.normalized * distance;
    }
}
