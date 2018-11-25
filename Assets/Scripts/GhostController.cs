using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

    public float speed = 5f;
    public bool CanUnpossess = true;
    public Possessable possessing;

    // TODO: Get these bounds from outside
    public float boundX = 20f;
    public float boundY = 20f;

    public VirtualInput vi;
    private Player player;

    private ContactFilter2D contactFilter;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        player = GetComponent<Player>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    
    void Update () {

        if (vi.GetButtonDown(Button.ACTION))
        {
            if (possessing != null)
            {
                ActivatePossessing();
            }
            else Possess();
        }
        if (vi.GetButtonDown(Button.JUMP))
        {
            if (possessing != null)
            {
                Unpossess();
            }
        }

        if (possessing)
            return;

        ////////////////////////////////////////////////////////
        // Move the player

        float h = vi.GetAxisRaw(Axis.HORIZONTAL) * speed;
        float v = vi.GetAxisRaw(Axis.VERTICAL) * speed;

        Vector2 move = new Vector2(h, v) * Time.deltaTime;

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if ((rb.position.x + move.x) > boundX || (rb.position.x + move.x) < -boundX)
        {
            move.x = 0;
        }
        if ((rb.position.y + move.y) > boundY || (rb.position.y + move.y) < -boundY)
        {
            move.y = 0;
        }

        rb.position += move;
	}

    private void Possess()
    {
        Collider2D[] possessables = new Collider2D[16];
        int count = rb.OverlapCollider(contactFilter, possessables);

        for (int i = 0; i < count; i++) {
            Possessable p = possessables[i].GetComponent<Possessable>();
            if (p != null)
            {
                if (p.Possess(player))
                {
                    transform.position = p.transform.position;

                    possessing = p;

                    col.enabled = false;
                    spriteRenderer.enabled = false;

                    break;
                }
            }
        }
    }

    private void Unpossess()
    {
        if (possessing != null && CanUnpossess)
        {
            if (possessing.Unpossess())
            {
                col.enabled = true;
                spriteRenderer.enabled = true;

                possessing = null;
            }
        }
    }

    private void ActivatePossessing()
    {
        if (possessing != null)
        {
            possessing.Interact();
        }
    }
}
