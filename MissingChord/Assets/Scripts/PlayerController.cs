using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public GameObject backgroundText;
    public GameObject text;

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
    bool isInDialogue = false;
    int dialogueId;
    [SerializeField] private GameObject octavia;
    [SerializeField] private GameObject anne;
    [SerializeField] private GameObject svetlana;
    [SerializeField] private GameObject alex;
    //Vector2 spawnPos;
    Interactables lastIdScanned;
    Sensors lastSensorPassed = Sensors.START;
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

    enum Sensors
    {
        NONE = -1,
        START,
        WATER,
        SPIKES,
        VINES,
        PATHS,
        LAKE,
        DROPDOWN,
        MISSION_DONE,
        MISSION_UPDATE,
        MISSION_END,
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartDialogue();
        //text.text = "";
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
                rb.velocity = new Vector2(movement.x, 0.0f);
                isClimbing = true;
                rb.gravityScale = 0.0f;
            }
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = 5.0f;
        }

        if (Input.GetButtonDown("Jump") && jumpAvailable)
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isInDialogue)
            {
                StartDialogue();
            }
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
            else//When text box finishes
            {
                if (!isInDialogue)
                {
                    isScanning = false;
                    Debug.Log("Dialogue ENDOU!");
                }
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
        if (isInDialogue)
        {
            ContinueDialogue(dialogueId);
        }
        else
        {
            isInDialogue = true;
            Debug.Log("Dialogue STARTO!");

            if (lastSensorPassed == Sensors.NONE)
            {
                switch (lastIdScanned)
                {
                    case Interactables.DWAYNE1:
                        svetlana.SetActive(true);
                        CreateDialogue("Don’t blink now, Octavia, there is what we are here for... Hold on... I’m getting Anne on the line");
                        dialogueId = 21;
                        break;
                    case Interactables.PLANT1:
                        octavia.SetActive(true);
                        CreateDialogue("I just scanned... A green hand-coral-thing?");
                        dialogueId = 31;
                        break;
                    case Interactables.PLANT2:
                        octavia.SetActive(true);
                        CreateDialogue("Found another one, I'm scanning.");
                        dialogueId = 121;
                        break;
                    case Interactables.PLANT3:
                        anne.SetActive(true);
                        CreateDialogue("This one feeds from the soil on the ceiling, how curious!");
                        dialogueId = 191;
                        break;
                    case Interactables.PLANT4:
                        octavia.SetActive(true);
                        CreateDialogue("This one is... Beautiful.");
                        dialogueId = 91;
                        break;
                    case Interactables.PLANT5:
                        octavia.SetActive(true);
                        CreateDialogue("Why does this one look somewhat like our moon?");
                        dialogueId = 81;
                        break;
                    case Interactables.DWAYNE2:
                        octavia.SetActive(true);
                        CreateDialogue("What on earth is that?");
                        dialogueId = 161;
                        break;
                    case Interactables.DWAYNE3:
                        octavia.SetActive(true);
                        CreateDialogue("Great, now they float, just great, sending scan.");
                        dialogueId = 151;
                        break;
                    case Interactables.DWAYNE4:
                        octavia.SetActive(true);
                        CreateDialogue("Found a rainbow quartz or something like that.");
                        dialogueId = 61;
                        break;
                    case Interactables.DWAYNE5:
                        octavia.SetActive(true);
                        CreateDialogue("Well this one looks promising.");
                        dialogueId = 141;
                        break;
                }
            }
            else
            {
                switch(lastSensorPassed)
                {
                    case Sensors.START:
                        svetlana.SetActive(true);
                        CreateDialogue("Do you hear me Ms.Koch ?");
                        dialogueId = 1;
                        break;
                    case Sensors.WATER:
                        svetlana.SetActive(true);
                        CreateDialogue("Hold on Octavia, I don't like the look of that liquid, calling Alex to get a result of the ship scan right now.");
                        dialogueId = 11;
                        break;
                    case Sensors.PATHS:
                        octavia.SetActive(true);
                        CreateDialogue("I see different paths here, what should I do?");
                        dialogueId = 41;
                        break;
                    case Sensors.VINES:
                        octavia.SetActive(true);
                        CreateDialogue("I think I can climb that.");
                        dialogueId = 51;
                        break;
                    case Sensors.SPIKES:
                        alex.SetActive(true);
                        CreateDialogue("Scans indicate high density of toxic vegetation, your suit will not protect you from those, watch out.");
                        dialogueId = 101;
                        break;
                    case Sensors.MISSION_DONE:
                        svetlana.SetActive(true);
                        CreateDialogue("Good job cadet, you can come to the extraction point now, or if you feel like it, you can keep looking for samples.");
                        dialogueId = 131;
                        break;
                    case Sensors.MISSION_UPDATE:
                        octavia.SetActive(true);
                        CreateDialogue("Uhm, either you got the coordinates wrong or I'm lost as hell, where are you?");
                        dialogueId = 171;
                        break;
                    case Sensors.MISSION_END:
                        octavia.SetActive(true);
                        CreateDialogue("Ok, I'm finally here.");
                        dialogueId = 181;
                        break;
                    case Sensors.DROPDOWN:
                        octavia.SetActive(true);
                        CreateDialogue("Uhm, I would like to keep my legs attached to my knees, Cap?");
                        dialogueId = 71;
                        break;
                    case Sensors.LAKE:
                        svetlana.SetActive(true);
                        CreateDialogue("Be careful down there rookie, we don’t want casualties.");
                        dialogueId = 111;
                        break;
                }
            }
        }
    }

    private void ContinueDialogue(int id)
    {
        switch(id)
        {
            case 0:
                octavia.SetActive(false);
                svetlana.SetActive(false);
                alex.SetActive(false);
                anne.SetActive(false);
                DestroyDialogue();
                break;
            case 1:
                svetlana.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Loud and clear.");
                dialogueId = 2;
                break;
            case 2:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("I am your mission commander Svetlana Tereshkova. Remember: use A and D to move around and SPACEBAR to Jump.");
                dialogueId = 0;
                break;
            case 11:
                svetlana.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("Hi I'm Alex Nyberg, I'm responsible for your mission IT, you see that.. uhm.. water? Whatever it is, don’t. Touch it. It's sending huge radiation readings.");
                dialogueId = 12;
                break;
            case 12:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Just another day on the job huh?");
                dialogueId = 0;
                break;
            case 21:
                svetlana.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("Hi!, I'm Anne Meir, do you see that mineral over there? Use E to use your scanner and send a clear reading to the ship for analisis.");
                dialogueId = 22;
                break;
            case 22:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Why don’t I just take it?");
                dialogueId = 23;
                break;
            case 23:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("We can’t trust the minerals on every planet, remember what we told you that happened to the scout on Ceturion V?");
                dialogueId = 24;
                break;
            case 24:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("…, Scanning now.");
                dialogueId = 25;
                break;
            case 25:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("This one looks similar to iron, it could have some uses.");
                dialogueId = 26;
                break;
            case 26:
                alex.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("You will need to scan at least 5 samples of this planet’s flora and minerals to consider this mission a success.");
                dialogueId = 0;
                break;
            case 31:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("It's actually a plant, but I like your observation, let’s call it Verdanthan!");
                dialogueId = 32;
                break;
            case 32:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Good with names I see?");
                dialogueId = 33;
                break;
            case 33:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("Hey! At least I try, Alex names them with numbers and codes.");
                dialogueId = 34;
                break;
            case 34:
                anne.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("What was wrong with green-B205?");
                dialogueId = 0;
                break;
            case 41:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Do as you like, as long as you can find samples.");
                dialogueId = 42;
                break;
            case 42:
                svetlana.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Alright.");
                dialogueId = 43;
                break;
            case 43:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Just remember, your extraction point is straight through the middle path.");
                dialogueId = 0;
                break;
            case 51:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("They seem resistant enough but I don’t think you should do tha-");
                dialogueId = 52;
                break;
            case 52:
                alex.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("Yay! Go swing around samples to bring me more samples!");
                dialogueId = 0;
                break;
            case 61:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("It's so pure!! Nice find.");
                dialogueId = 62;
                break;
            case 62:
                anne.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("If you drop down through that hole you will get back to the path below you. If you want to keep exploring that zone with all those vines, don’t go.");
                dialogueId = 0;
                break;
            case 71:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Don’t worry about breaking your knees in the fall, cadet, remember that your suit is equipped to sustain falls from up to 100 meters without damaging its user.");
                dialogueId = 0;
                break;
            case 81:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("I don't know, what I know is that it will kill you the same as most of this planet’s flora if you touch it, so-");
                dialogueId = 82;
                break;
            case 82:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Yeah yeah, but what is it supposed to kill?");
                dialogueId = 83;
                break;
            case 83:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Imprudent cadets probably.");
                dialogueId = 84;
                break;
            case 84:
                svetlana.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("…");
                dialogueId = 0;
                break;
            case 91:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("It keeps bothering me, this is the first planet that we find life. But, there are no animals?");
                dialogueId = 92;
                break;
            case 92:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Maybe it is in the early stages of evolution? Or maybe it is so small that we can’t see it.");
                dialogueId = 93;
                break;
            case 93:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("Doesn’t seem like it, the scans indicate that these plans are at least 50+ years old.");
                dialogueId = 0;
                break;
            case 101:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Copy that, you heard that, Anne? You won't have a sample of that.");
                dialogueId = 102;
                break;
            case 102:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("You can certainly try...");
                dialogueId = 103;
                break;
            case 103:
                anne.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Stop messing with the rookie, c’mon Octavia, keep going.");
                dialogueId = 0;
                break;
            case 111:
                svetlana.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Oh, funny you mention it, I was thinking of taking a bath on that lake.");
                dialogueId = 112;
                break;
            case 112:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Ha. Ha.");
                dialogueId = 0;
                break;
            case 121:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("The lectures indicate high levels of biological toxins, I would suggest not to touch it.");
                dialogueId = 0;
                break;
            case 131:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Sure thing boss.");
                dialogueId = 0;
                break;
            case 141:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("Promisingly lethal you mean, get away as soon as you can, radiation rises to dangerous levels just by being near that thing.");
                dialogueId = 142;
                break;
            case 142:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Wait what?");
                dialogueId = 0;
                break;
            case 151:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("The magnetic field of the planet seems to react with more strength with this mineral.");
                dialogueId = 152;
                break;
            case 152:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("And here I was thinking it was magic.");
                dialogueId = 0;
                break;
            case 161:
                octavia.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("It looks very similar to certain material from a game I know...");
                dialogueId = 162;
                break;
            case 162:
                alex.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Just... just scan it and keep going.");
                dialogueId = 0;
                break;
            case 171:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("The previously designed zone is no longer viable for extraction.");
                dialogueId = 172;
                break;
            case 172:
                svetlana.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("I'm sending you the new coords, do you see that mountain over there? I am afraid you will have to reach the top.");
                dialogueId = 173;
                break;
            case 173:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("You can't land in this open area, but you can on the top of a mountain?");
                dialogueId = 174;
                break;
            case 174:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("If you want us to crash due to high winds we can go to the previous point.");
                dialogueId = 175;
                break;
            case 175:
                svetlana.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Whatever, I needed some fun.");
                dialogueId = 0;
                break;
            case 181:
                octavia.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Took you long enough, c’mon, get in here cadet.");
                dialogueId = 182;
                break;
            case 182:
                svetlana.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("Mission accomplished, continuing to further analyze recovered samples, not bad Ms.Koch.");
                dialogueId = 183;
                break;
            case 183:
                alex.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("How is that for a rookie?");
                dialogueId = 184;
                break;
            case 184:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("You did great!!");
                dialogueId = 185;
                break;
            case 185:
                anne.SetActive(false);
                svetlana.SetActive(true);
                CreateDialogue("Job well done, next time try risking your neck less though.");
                dialogueId = 186;
                break;
            case 186:
                svetlana.SetActive(false);
                alex.SetActive(true);
                CreateDialogue("Isn’t that like part of her job?");
                dialogueId = 0;
                break;
            case 191:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Important question, it won’t hurt me to go under it, right?");
                dialogueId = 192;
                break;
            case 192:
                octavia.SetActive(false);
                anne.SetActive(true);
                CreateDialogue("Seems to be mostly harmless, I believe it won’t hurt you.");
                dialogueId = 193;
                break;
            case 193:
                anne.SetActive(false);
                octavia.SetActive(true);
                CreateDialogue("Roger that.");
                dialogueId = 0;
                break;
        }
    }

    private void CreateDialogue(string m)
    {
        backgroundText.SetActive(true);
        text.SetActive(true);
        text.GetComponent<Text>().text = m;
    }

    private void DestroyDialogue()
    {
        backgroundText.SetActive(false);
        text.GetComponent<Text>().text = "";
        text.SetActive(false);
        lastSensorPassed = Sensors.NONE;
        isInDialogue = false;
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
            case Interactables.PLANT1:
                spawnPos = new Vector2(554, -807);
                break;
            case Interactables.PLANT2:
                spawnPos = new Vector2(672, -815);
                sr.flipX = true;
                break;
            case Interactables.PLANT3:
                spawnPos = new Vector2(658, -798);
                sr.flipX = true;
                break;
            case Interactables.PLANT4:
                spawnPos = new Vector2(669, -784);
                sr.flipX = true;
                break;
            case Interactables.PLANT5:
                spawnPos = new Vector2(554, -748);
                sr.flipX = true;
                break;
            case Interactables.DWAYNE1:
                spawnPos = new Vector2(500, -817);
                break;
            case Interactables.DWAYNE2:
                spawnPos = new Vector2(704, -828);
                break;
            case Interactables.DWAYNE3:
                spawnPos = new Vector2(619, -791);
                sr.flipX = true;
                break;
            case Interactables.DWAYNE4:
                spawnPos = new Vector2(680, -755);
                break;
            case Interactables.DWAYNE5:
                spawnPos = new Vector2(717, -786);
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
            if (dialogueId == 0)
            {
                Destroy(other.gameObject);
            }

            switch(other.gameObject.name)
            {
                case "WaterSensor":
                    lastSensorPassed = Sensors.WATER;
                    break;
                case "PathsSensor":
                    lastSensorPassed = Sensors.PATHS;
                    break;
                case "SpikesSensor":
                    lastSensorPassed = Sensors.SPIKES;
                    break;
                case "LakeSensor":
                    lastSensorPassed = Sensors.LAKE;
                    break;
                case "VinesSensor":
                    lastSensorPassed = Sensors.VINES;
                    break;
                case "Dropdown_1Sensor":
                    lastSensorPassed = Sensors.DROPDOWN;
                    break;
                case "Dropdown_2Sensor":
                    lastSensorPassed = Sensors.DROPDOWN;
                    break;
                case "MissionUpdateSensor":
                    lastSensorPassed = Sensors.MISSION_UPDATE;
                    break;
                case "MissionEndSensor":
                    lastSensorPassed = Sensors.MISSION_END;
                    break;
            }
            //if (other.gameObject.name=="WaterSensor")
            //{
            //    lastSensorPassed = Sensors.WATER;
            //}
            StartDialogue();
        }
    }
}