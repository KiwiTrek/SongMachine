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

        if (Input.GetButtonDown("Jump") && jumpAvailable)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement.x * Time.fixedDeltaTime * speed, 0.0f, 0.0f);

        //bool wasGrounded = isGrounded;
        //jump = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundSensor.position, 0.1f, playerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                jumpAvailable = true;
                Debug.Log("Collision!");
                //if (wasGrounded)
                //{
                //    rb.AddForce(new Vector2(0.0f, jumpHeight));
                //    //jump = false;
                //}
            }
            else
            {
                jumpAvailable = false;
            }
        }

        if(jump)
        {
            jumpAvailable = false;
            jump = false;
            rb.AddForce(new Vector2(0.0f, jumpHeight));
        }
    }
}
