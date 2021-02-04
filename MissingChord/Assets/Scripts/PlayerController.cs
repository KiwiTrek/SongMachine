using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Transform groundSensor;
    [SerializeField] private LayerMask layers;
    public float speed;
    public float jumpHeight;
    bool jump = false;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {

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

        //if(Physics2D.OverlapCircle(groundSensor.position,0.1f,layers).Distance == )

        if(jump)
        {
            rb.AddForce(new Vector2(0.0f, jumpHeight));
            jump = false;
        }
    }
}
