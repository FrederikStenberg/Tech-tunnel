using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSelector : MonoBehaviour {

    float timeBeforeNextEvent;
    int selectEvent;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void startCour()
    {
        StartCoroutine(SelectTrigger());
    }

    public IEnumerator SelectTrigger()
    {
        selectEvent = Random.Range(1, 5);
        timeBeforeNextEvent = Random.Range(5.0f, 25.0f);
        yield return new WaitForSeconds(timeBeforeNextEvent);
        switch(selectEvent)
        {
            case 1:
                animator.SetTrigger("ShootingStar");
                break;
            case 2:
                animator.SetTrigger("Meteor");
                break;
            case 3:
                animator.SetTrigger("Geyser");
                break;
            case 4:
                animator.SetTrigger("Spaceman");
                break;
        }
    }
}
