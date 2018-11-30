using UnityEngine;
using System;
using System.Collections;
using cakeslice;


public class MoveTo : MonoBehaviour
{
    // Public inspector variables
    public CameraController cam; // Camera script on main camera
    public AudioClip[] locationClips; // Audio clips for different destinations
    public GameObject ui, map;

    public Vector3 defaultRot, targetOffset = new Vector3(0, 5, 0);
    public float speed;

    // Temporary public inspector variables, should be Private/hidden
    public string agentState = "Idle"; // Remove = "Idle"; when done with testing animations

    // Public variables hidden in the inspector - Accessed in other scripts
    [HideInInspector] // Vector3
    public Vector3 lookAtPos;

    [HideInInspector] // Boolean
    public bool moving = false, arrived = false, checkingDistance = false;
    
    [HideInInspector] // String
    public string clickedLocation = "default";

    // Private variables
    Animator anim;
    Vector3 defaultPos, targetPos;
    Quaternion defaultQuart;
    string[] randomAnimArray = { "Waving", "ToSleep" }; // Idle animation state names in Agentcontroller.
    string[] talkingAnimArray = { "GoToShort", "GoToMedium", "GoToLong" }; // Talking animation state names in Agentcontroller.
    float currentDistance, angle, step;
    bool randomAnimNumAssigned = false, defaultCheck = true;


    void Start()
    {
        agentState = "Idle";
        anim = GetComponentInChildren<Animator>();
        defaultPos = transform.position;
        defaultRot = transform.eulerAngles;
        defaultQuart = transform.rotation;
    }

    void Update()
    {
        // Update debuggers

        // --------------------------------------------------- //


        if (moving)
        {
            Move();
        }
        else
        {
            cam.CamToMap();
            if (defaultCheck)
            {
                ClickOnLocation();
            }
        }

        // When return button is clicked, but agent is not at default position
        if (!defaultCheck)
        {
            MoveToDefault();
        }

        // Random animations when idle  
        if (!randomAnimNumAssigned && !moving && defaultCheck)
        {
            sec_toRandomAnim = UnityEngine.Random.Range(5.0f, 10.0f);
            StartCoroutine(animEvents());
            randomAnimNumAssigned = true;
        }

        // Animations when talking
        if (GetComponentInChildren<AudioSource>().isPlaying)
        {
            // Reset triggers to avoid same animation activation
            anim.ResetTrigger(talkingAnimArray[0]); anim.ResetTrigger(talkingAnimArray[1]); anim.ResetTrigger(talkingAnimArray[2]);

            int randTalk = UnityEngine.Random.Range(0, 3);
            if ((randTalk == 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.TalkingShort")) || 
                (randTalk == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.TalkingMedium")) || 
                (randTalk == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.TalkingLong"))) // Check that we are not playing the same animation twice in a row
            {
                if (randTalk == talkingAnimArray.Length-1) // Avoid going out of bounds in the array
                    randTalk -= 1; 
                else
                    randTalk += 1;
            }

            anim.SetTrigger(talkingAnimArray[randTalk]);
        }
        else if (arrived)
        {
            anim.Play("Idle");
            anim.ResetTrigger(talkingAnimArray[0]); anim.ResetTrigger(talkingAnimArray[1]); anim.ResetTrigger(talkingAnimArray[2]);
            ReturnToDefaultState(); // Agent is done giving information so we return automatically
        }
    }

    //ClickOnLocation - retrieves information about the hit object, and sets the position of the object as the agent's destination
    void ClickOnLocation()
    {
        //Click Event
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Clickable")
                {
                    cam.gameObject.GetComponent<OutlineEffect>().enabled = false; // Hide outlines on clickable objects

                    clickedLocation = hit.collider.gameObject.transform.name;

                    // Make sure to include the prefabs LookAtPos and MoveToMe as the first and second child in the object with
                    // the clickable tag respectively - and include a collider on the clickable object (parent).
                    lookAtPos = hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position;
                    targetPos = hit.collider.gameObject.transform.GetChild(1).gameObject.transform.position;
                    Debug.Log("Raycast hit clickable object.");

                    moving = true;
                    checkingDistance = true;
                    arrived = false;

                    anim.Play("ToFlying");
                }
            }
        }
    }

    //Move function - moves towards clicked object
    void Move()
    {
        if (!arrived) // False when agent position =/= target position
        {
            Vector3 targetDir = targetPos - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            angle = Vector3.Angle(transform.forward, targetDir);
            step = speed / 10;

            // Move agent rotation a step closer to the target.
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, step * Time.deltaTime, 0.0f));

            if (angle < 5f) // Angle determines whether the agent is rotated towards the clicked location, to avoid travelling backwards
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if (checkingDistance)
                {
                    CheckDistToTarget();
                }
                if (transform.position == targetPos)
                {
                    anim.SetTrigger("Arrived");
                    arrived = true; // Agent arrived at destination

                    //Enable desired sound-clip depending on location
                    switch (clickedLocation)
                    {
                        case "SvanekeHavn":
                            GetComponentInChildren<AudioSource>().clip = locationClips[0];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "Rokkesten":
                            GetComponentInChildren<AudioSource>().clip = locationClips[1];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "Hammershus":
                            GetComponentInChildren<AudioSource>().clip = locationClips[2];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "Menhirs":
                            GetComponentInChildren<AudioSource>().clip = locationClips[3];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "Smokehouse":
                            GetComponentInChildren<AudioSource>().clip = locationClips[4];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "EchoValley":
                            GetComponentInChildren<AudioSource>().clip = locationClips[5];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                        case "Airport":
                            GetComponentInChildren<AudioSource>().clip = locationClips[6];
                            GetComponentInChildren<AudioSource>().Play();
                            break;
                    }
                }
            }
        }
        else
        {
            transform.LookAt(cam.transform);
            cam.LookAtAgent();
            ui.SetActive(true);
        }
    }

    void MoveToDefault()
    {
        // Function variable initialization
        Vector3 targetDir = defaultPos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        angle = Vector3.Angle(transform.forward, targetDir);
        step = speed / 10 * Time.deltaTime;

        // If the agent's positions = target position, then stop movement and rotate to camera
        if (transform.position == targetPos)
        {
            transform.eulerAngles = defaultRot;
            cam.gameObject.GetComponent<OutlineEffect>().enabled = true;
            anim.ResetTrigger(talkingAnimArray[0]); anim.ResetTrigger(talkingAnimArray[1]); anim.ResetTrigger(talkingAnimArray[2]);
            defaultCheck = true;

        }
        else // If the agent needs to move to the target position (which is the default position)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f));
            if (angle < 5f)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.Flying") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.FromFlying"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                }

                //Checks distance to ensure Agent doesn't get stuck in FlySpin animation when at defaultPos
                if (checkingDistance)
                {
                    CheckDistToTarget();
                }
            }
        }

    }

    public void CheckDistToTarget()
    {
        currentDistance = Vector3.Distance(transform.position, targetPos);
        if (currentDistance < 5)
        {
            anim.SetTrigger("FromFlying");
            checkingDistance = false;
        }
    }

    //Enable properties that allows for returning to the default state
    public void ReturnToDefaultState()
    {
        ui.SetActive(false);
        GetComponentInChildren<AudioSource>().Stop();
        targetPos = defaultPos;
        moving = false;
        arrived = false;
        anim.Play("ToFlying");
        checkingDistance = true;
        defaultCheck = false; // Should be false when targetPos =/= defaultPos
    }
    

    //Assigning second floats, initialize and tweak through inspector
    float sec_toRandomAnim;

    public void randomAnimationsTable(string animationName)
    {
        if (randomAnimNumAssigned)
        {
            if (animationName == "Random" || animationName == "random" || animationName == "RANDOM")
            {
                int r = UnityEngine.Random.Range(0, 3);
                anim.Play(randomAnimArray[r]);
            }
            else
                anim.Play(animationName);
        }
    }

    IEnumerator animEvents()
    {
        yield return new WaitForSeconds(sec_toRandomAnim);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.Idle") && !GetComponentInChildren<AudioSource>().isPlaying)
        {
            randomAnimationsTable("Random");
            randomAnimNumAssigned = false;
        }
        yield break;
    }
}
