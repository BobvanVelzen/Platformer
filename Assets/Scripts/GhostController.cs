using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

    public float speed = 5f;

    // TODO: Get these bounds from outside
    public float boundX = 20f;
    public float boundY = 20f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        float h = Input.GetAxisRaw("Horizontal") * speed;
        float v = Input.GetAxisRaw("Vertical") * speed;

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
}
