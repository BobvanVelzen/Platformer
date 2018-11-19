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
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        player = GetComponent<Player>();
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

        if (vi.GetButtonDown(InButton.ACTION))
        {
            if (possessing != null)
            {
                ActivatePossessing();
            }
            else Possess();
        }
        if (vi.GetButtonDown(InButton.JUMP))
        {
            if (possessing != null)
            {
                Unpossess();
            }
        }

        if (possessing)
            return;

        ////////////////////////////////////////////////////////

        float h = vi.GetAxisRaw(InAxis.HORIZONTAL) * speed;
        float v = vi.GetAxisRaw(InAxis.VERTICAL) * speed;

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
                    player.ChangeOpacity(0f);
                    break;
                }
            }
        }
    }

    private void Unpossess()
    {
        if (possessing != null && CanUnpossess)
        {
            possessing.Unpossess();
            possessing = null;
            player.ChangeOpacity(0.5f);
        }
    }

    private void ActivatePossessing()
    {
        if (possessing != null)
        {
            possessing.Activate();
        }
    }
}
