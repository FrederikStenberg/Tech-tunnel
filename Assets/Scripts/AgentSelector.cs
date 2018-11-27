using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSelector : MonoBehaviour {

    float timeBeforeNextState;
    float timeBeforeNextInteraction;
    int selectState;
    int selectInteraction;
    int currentState = 1;
    int[] firstStateAccess = new int[3] { 1, 2, 3 };
    int[] secondStateAccess = new int[1] { 4 };
    int[] thirdStateAccess = new int[2] { 5, 6 };
    int[] fourthStateAccess = new int[2] { 7, 8 };
    int[] firstStateInter = new int[2] { 1, 2 };
    int[] secondStateInter = new int[1] { 1 };
    int[] thirdStateInter = new int[2] { 3, 4 };
    int[] fourthStateInter = new int[1] { 5 };
    int doInteraction = 0;

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
        doInteraction = Random.Range(0, 2);

        if(doInteraction == 0)
        {
            if (currentState == 1)
            {
                selectState = firstStateAccess[Random.Range(0, firstStateAccess.Length)];
            }
            else if (currentState == 2)
            {
                selectState = secondStateAccess[Random.Range(0, secondStateAccess.Length)];
            }
            else if (currentState == 3)
            {
                selectState = thirdStateAccess[Random.Range(0, thirdStateAccess.Length)];
            }
            else if (currentState == 4)
            {
                selectState = fourthStateAccess[Random.Range(0, fourthStateAccess.Length)];
            }
            timeBeforeNextState = Random.Range(5.0f, 25.0f);
            yield return new WaitForSeconds(timeBeforeNextState);
            switch (selectState)
            {
                case 1:
                    animator.SetTrigger("State1To2");
                    currentState = 2;
                    break;
                case 2:
                    animator.SetTrigger("State1To3");
                    currentState = 3;
                    break;
                case 3:
                    animator.SetTrigger("State1To4");
                    currentState = 4;
                    break;
                case 4:
                    animator.SetTrigger("State2To1");
                    currentState = 1;
                    break;
                case 5:
                    animator.SetTrigger("State3To1");
                    currentState = 1;
                    break;
                case 6:
                    animator.SetTrigger("State3To4");
                    currentState = 4;
                    break;
                case 7:
                    animator.SetTrigger("State4To1");
                    currentState = 1;
                    break;
                case 8:
                    animator.SetTrigger("State4To3");
                    currentState = 3;
                    break;
            }
        } else if(doInteraction == 1)
        {
            if (currentState == 1)
            {
                selectInteraction = firstStateInter[Random.Range(0, firstStateInter.Length)];
            }
            else if (currentState == 2)
            {
                selectInteraction = secondStateInter[Random.Range(0, secondStateInter.Length)];
            }
            else if (currentState == 3)
            {
                selectInteraction = thirdStateInter[Random.Range(0, thirdStateInter.Length)];
            }
            else if (currentState == 4)
            {
                selectInteraction = fourthStateInter[Random.Range(0, fourthStateInter.Length)];
            }

            timeBeforeNextInteraction = Random.Range(2.0f, 8.0f);
            yield return new WaitForSeconds(timeBeforeNextInteraction);
            switch (selectInteraction)
            {
                case 1:
                    animator.SetTrigger("Wave");
                    break;
                case 2:
                    animator.SetTrigger("Hat");
                    break;
                case 3:
                    animator.SetTrigger("Drill");
                    break;
                case 4:
                    animator.SetTrigger("Sleep");
                    break;
                case 5:
                    animator.SetTrigger("Pet");
                    break;
            }
        }
    }
}
