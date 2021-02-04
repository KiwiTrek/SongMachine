using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform groundSensor = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    bool jump = false;
    bool jumpAvailable = true;
    public bool climbAvailable = false;
    bool isClimbing = false;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(0.0f, 0.0f);
        movement.x = Input.GetAxisRaw("Horizontal");

        if(climbAvailable)
        {
            movement.y = Input.GetAxisRaw("Vertical");
            if(movement.y != 0.0f)
            {
                isClimbing = true;
                rb.gravityScale = 0.0f;
                //Physics2D.gravity = new Vector2(0.0f, 0.0f);
            }
        }
        else
        {
            rb.gravityScale = 5.0f;
        }

        if (Input.GetButtonDown("Jump") && jumpAvailable)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isClimbing)
        {
            transform.position += new Vector3(movement.x * Time.fixedDeltaTime * speed, 0.0f, 0.0f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundSensor.position, 0.01f, playerMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    jumpAvailable = true;
                }
                else
                {
                    jumpAvailable = false;
                }
            }
        }
        else
        {
            jumpAvailable = true;
            transform.position += new Vector3(movement.x * Time.fixedDeltaTime * speed, movement.y * Time.fixedDeltaTime * speed, 0.0f);
        }

        if (jump)
        {
            jumpAvailable = false;
            jump = false;
            rb.AddForce(new Vector2(0.0f, jumpHeight));
        }
    }
}
