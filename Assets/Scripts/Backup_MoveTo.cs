using UnityEngine;
using System;
using System.Collections;
using cakeslice;


public class BackupMoveTo : MonoBehaviour
{

    //MoveTo based on clicks using Raycast

    public CameraController cam;
    public Vector3 lookAtPos;
    public AudioClip[] locationClips;
    Animator anim;
    //string[] animArray = { "Waving", "ToSleep", "Spin" };
    //bool randomAnimNumAssigned = false;

    string clickedLocation = "default";
    Vector3 defaultPos;
    Vector3 targetPos;
    Quaternion defaultQuart;
    float angle;
    public Vector3 defaultRot, targetOffset = new Vector3(0, 5, 0);

    public float speed;
    float step;

    [HideInInspector]
    public bool moving = false, arrived = false, toDefault = false;
    public string agentState = "Idle";

    bool defaultCheck = false;
    bool dontSpamMoveAnim = false;

    public GameObject ui, map;

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
        if (moving)
        {
            Move();
        }
        else
        {
            cam.CamToMap();
            //Click Event
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Clickable")
                    {
                        clickedLocation = hit.collider.gameObject.transform.name;
                        cam.gameObject.GetComponent<OutlineEffect>().enabled = false;
                        // Make sure to include the prefabs LookAtPos and MoveToMe as the first and second child in the object with
                        // the clickable tag respectively - and include a collider on the clickable object (parent).
                        anim.SetTrigger("Moving");
                        lookAtPos = hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position;
                        targetPos = hit.collider.gameObject.transform.GetChild(1).gameObject.transform.position;
                        moving = true;
                        arrived = false;
                        Debug.Log("Raycast hit clickable object.");
                    }
                }
            }
        }

        if (toDefault)
        {
            ReturnToDefaultState();
        }

        //Animation control
        //if (!randomAnimNumAssigned)
        //{
        //    sec_toRandomAnim = UnityEngine.Random.Range(5.0f, 10.0f);
        //    StartCoroutine(animEvents());
        //    randomAnimNumAssigned = true;
        //}
    }

    void Move()
    {

        if (!arrived)
        {
            Vector3 targetDir = targetPos - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            angle = Vector3.Angle(transform.forward, targetDir);
            step = speed / 10 * Time.deltaTime;

            //// Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f));

            if (angle < 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if (transform.position == targetPos)
                {
                    anim.SetTrigger("Arrived");
                    arrived = true;
                    //Enable desired sound-clip
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
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(0, 0, -1), step, 0.0f));
            transform.LookAt(lookAtPos);
            cam.LookAtAgent();
            ui.SetActive(true);
        }
    }

    public void ReturnToDefaultState()
    {
        if (dontSpamMoveAnim == false)
        {
            GetComponentInChildren<AudioSource>().Stop();
            anim.SetTrigger("FlyUp");
            dontSpamMoveAnim = true;
        }

        //cam.CamToMap();
        //ui.SetActive(false);
        //moving = false;
        //arrived = false;
        //targetPos = defaultPos;

        StartCoroutine(waitForReturnAnimation());
    }

    IEnumerator waitForReturnAnimation()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        cam.CamToMap();
        ui.SetActive(false);
        moving = false;
        arrived = false;
        targetPos = defaultPos;

        if (!defaultCheck)
        {
            //angle = Vector3.Angle(transform.forward, targetPos);
            //step = speed / 10 * Time.deltaTime;
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetPos, step, 0.0f));

            //Vector3 direction = cam.transform.position - transform.position;
            //Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

            Vector3 direction = targetPos - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, -transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);


            if (angle < 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if (transform.position == targetPos)
                {
                    cam.gameObject.GetComponent<OutlineEffect>().enabled = true;
                    anim.SetTrigger("FlyDown");
                    defaultCheck = true;
                }
            }
        }
        else
        {
            float defaultQuartCheck = Quaternion.Angle(transform.rotation, defaultQuart);
            step = speed / 5 * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultQuart, step);
            if (defaultQuartCheck < 5f)
            {
                defaultCheck = false;
                toDefault = false;
                dontSpamMoveAnim = false;
            }
        }
    }

    //public void animationTable(string animationName)
    //{
    //    if (randomAnimNumAssigned)
    //    if (animationName == "Random"|| animationName == "random" || animationName == "RANDOM")
    //    {
    //        int r = UnityEngine.Random.Range(1,3);
    //        Debug.Log("Random animation number: " + r);
    //        //anim.Play(animArray[r]);
    //        anim.SetTrigger(r);
    //    }
    //    else
    //        anim.Play(animationName);
    //}

    //Assigning second floats, initialize and tweak through inspector
    //float sec_toRandomAnim, sec_anim2, sec_anim3, sec_anim4, sec_anim5;

    //IEnumerator animEvents()
    //{
    //    yield return new WaitForSeconds(sec_toRandomAnim);
    //    animationTable("Random");   
    //    randomAnimNumAssigned = false;
    //    Debug.Log("Requested random animation after " + sec_toRandomAnim + " seconds.");
    //    yield break;
    //}
}
