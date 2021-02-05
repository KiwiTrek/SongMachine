using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform groundSensor = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform interactRange = null;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    bool jump = false;
    bool jumpAvailable = true;
    public bool climbAvailable = false;
    bool isClimbing = false;
    bool isScanning = false;
    //Vector2 spawnPos;
    Interactables lastIdScanned;
    Vector2 movement;

    enum Interactables
    {
        PLANT1,
        PLANT2,
        PLANT3,
        PLANT4,
        PLANT5,
        DWAYNE1,
        DWAYNE2,
        DWAYNE3,
        DWAYNE4,
        DWAYNE5
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //spawnPos = new Vector2(465, 821);                                     DO NOT ERASE (initial spawnpos)
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
                        if(colliders[i].gameObject == GameObject.Find("Dwayne_1"))
                        {
                            SetSpawnPoint(Interactables.DWAYNE1);
                        }
                        else if(colliders[i].gameObject == GameObject.Find("Dwayne_2"))
                        {
                            SetSpawnPoint(Interactables.DWAYNE2);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Dwayne_3"))
                        {
                            SetSpawnPoint(Interactables.DWAYNE3);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Dwayne_4"))
                        {
                            SetSpawnPoint(Interactables.DWAYNE4);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Dwayne_5"))
                        {
                            SetSpawnPoint(Interactables.DWAYNE5);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Plant_1"))
                        {
                            SetSpawnPoint(Interactables.PLANT1);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Plant_2"))
                        {
                            SetSpawnPoint(Interactables.PLANT2);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Plant_3"))
                        {
                            SetSpawnPoint(Interactables.PLANT3);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Plant_4"))
                        {
                            SetSpawnPoint(Interactables.PLANT4);
                        }
                        else if (colliders[i].gameObject == GameObject.Find("Plant_5"))
                        {
                            SetSpawnPoint(Interactables.PLANT5);
                        }
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
        switch(lastIdScanned)
        {
            case Interactables.DWAYNE1:
                break;
        }
    }

    void SetSpawnPoint(Interactables id)
    {
        lastIdScanned = id;
    }

    public void ResetPlayer()
    {
        sr.flipX = false;
        Vector2 spawnPos = new Vector2(0,0);
        switch (lastIdScanned)
        {
            case Interactables.DWAYNE1:
                spawnPos = new Vector2(487, -816);
                break;
            case Interactables.PLANT1:
                spawnPos = new Vector2(554, -807);
                break;
            case Interactables.DWAYNE2:
                spawnPos = new Vector2(704, -828);
                break;
            case Interactables.PLANT2:
                spawnPos = new Vector2(658, -798);
                sr.flipX = true;
                break;
            case Interactables.DWAYNE3:
                spawnPos = new Vector2(680, -755);
                break;
            case Interactables.PLANT3:
                spawnPos = new Vector2(669, -784);
                sr.flipX = true;
                break;
            case Interactables.DWAYNE4:
                spawnPos = new Vector2(619, -791);
                sr.flipX = true;
                break;
            case Interactables.PLANT4:
                spawnPos = new Vector2(727, -786);
                break;
            case Interactables.DWAYNE5:
                spawnPos = new Vector2(672, -815);
                sr.flipX = true;
                break;
            case Interactables.PLANT5:
                spawnPos = new Vector2(554, -748);
                sr.flipX = true;
                break;
            default:
                break;
        }
        transform.position = new Vector3(spawnPos.x, spawnPos.y, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            Debug.Log("Dialog triggered");
            if (other.gameObject.name=="S1")
            {
                Debug.Log("Sensor 1 triggered");
            }
            Destroy(other.gameObject);
        }
    }
}