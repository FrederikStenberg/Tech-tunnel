using UnityEngine;

public class CameraController : MonoBehaviour {

    // Public inspector variables
    public MoveTo agentScript;
    public AudioClip[] locationClips; // Audio clips for different destinations

    public Vector3 camOffset, mapOffset;
    public float speed, boundary;

    // Temporary public inspector variables, should be Private/hidden

    // Public variables hidden in the inspector - Accessed in other scripts

    // Private variables
    GameObject map, agent;

    Vector3 camDefaultPos;
    float halfSpeed;

    bool playArrivedAudioOnce = false;


    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        agent = GameObject.FindGameObjectWithTag("Agent");
        camDefaultPos = transform.position;
        halfSpeed = speed / speed;
    }


    void LateUpdate ()
    {
        if(!agentScript.arrived)
        {
            if (!GetComponent<AudioSource>().isPlaying || (!agentScript.arrived && GetComponent<AudioSource>().clip != locationClips[7]))
            {
                GetComponent<AudioSource>().clip = locationClips[7];
                GetComponent<AudioSource>().Play();
            }

            playArrivedAudioOnce = false;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            // LookAt Agent
            Vector3 direction = agent.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, halfSpeed * Time.deltaTime);

            // Move in front of Agent
            Vector3 desiredPosition = agent.transform.position + camOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else if (!playArrivedAudioOnce)
        {
            switch (agentScript.clickedLocation)
            {
                case "SvanekeHavn":
                    GetComponent<AudioSource>().clip = locationClips[0];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Rokkesten":
                    GetComponent<AudioSource>().clip = locationClips[1];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Hammershus":
                    GetComponent<AudioSource>().clip = locationClips[2];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Menhirs":
                    GetComponent<AudioSource>().clip = locationClips[3];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Smokehouse":
                    GetComponent<AudioSource>().clip = locationClips[4];
                    GetComponent<AudioSource>().Play();
                    break;
                case "EchoValley":
                    GetComponent<AudioSource>().clip = locationClips[5];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Airport":
                    GetComponent<AudioSource>().clip = locationClips[6];
                    GetComponent<AudioSource>().Play();
                    break;
                case "Rønne":
                    GetComponent<AudioSource>().clip = locationClips[7];
                    GetComponent<AudioSource>().Play();
                    break;
            }
            playArrivedAudioOnce = true;
        }
    }

    public void LookAtAgent()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        // LookAt Agent
        Vector3 direction = agent.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, halfSpeed * Time.deltaTime);

        // Move in front of Agent
        ///Vector3 desiredPosition = agent.transform.position + camOffset;
        Vector3 desiredPosition = agentScript.lookAtPos;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void CamToMap()
    {
        //Move to default position
        Vector3 desiredPostition = camDefaultPos;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostition, speed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Below if statement prevents the camera from spinning around when looking for the middle map point.
        // It will not only do this when it is within 20 on the x-axis.
        if (transform.position.x <= camDefaultPos.x + boundary && transform.position.x >= camDefaultPos.x - boundary)
        {
            //LookAt Map
            Vector3 direction = map.transform.position + mapOffset - transform.position; // - new Vector3(0 ,transform.position.y, -45)
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, halfSpeed * Time.deltaTime);
        }
        else
        {
            //LookAt Agent
            Vector3 direction = agent.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, halfSpeed * Time.deltaTime);
        }
    }
}
