using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [SerializeField] private Transform groundSensor = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform interactRange = null;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Animator anim;
    bool jump = false;
    bool jumpAvailable = true;
    public bool climbAvailable = false;
    bool isClimbing = false;
    bool isScanning = false;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(0.0f, 0.0f);
        movement.x = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(movement.x));
        if (movement.x < 0.0f)
        {
            sr.flipX = true;
        }
        else if (movement.x > 0.0f)
        {
            sr.flipX = false;
        }

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isScanning)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(interactRange.position, 2.0f, interactMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        Debug.Log("interact");
                        isScanning = true;
                    }
                    else
                    {
                        Debug.Log("can't interact");
                    }
                }
            }
            else //When text box finishes
            {
                isScanning = false;
                Debug.Log("Dialogue ENDOU!");
            }
        }
        
        anim.SetBool("IsJumping", !jumpAvailable);
        anim.SetBool("ScanStart", isScanning);
        anim.SetBool("IsClimbing", isClimbing);
    }

    private void FixedUpdate()
    {
        if (!isClimbing)
        {
            transform.position += new Vector3(movement.x * Time.fixedDeltaTime * speed, 0.0f, 0.0f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundSensor.position, 0.05f, playerMask);
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

    public void StartDialogue()
    {
        Debug.Log("Dialogue STARTO!");
    }
}