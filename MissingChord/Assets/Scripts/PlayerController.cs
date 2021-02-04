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

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement.x * Time.fixedDeltaTime * speed, 0.0f, 0.0f);

        //if (Physics2D.OverlapCircle(groundSensor.position, 0.1f, layers).Distance == )

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundSensor.position, 0.1f, playerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject)
            {
                return;
            }

            if (jump)
            {
                rb.AddForce(new Vector2(0.0f, jumpHeight));
                jump = false;
            }
        }
    }
}
