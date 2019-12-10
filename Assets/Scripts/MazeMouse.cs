using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeMouse : MonoBehaviour
{
    //Enums
    public enum mouseType
    {
        Quick,
        Thinker,
        Normal,
    }
    public mouseType behaviorType;
    public enum mouseStates
    {
        Run,
        Think,
    }
    public mouseStates currentState;

    //Waypoints
    public GameObject[] waypoints;
    public int waypointCounter;
    private NavMeshAgent agent;
    private float waitTime;
    private bool getWaitTime = false;
    public Animator animator;
    public Vector3 velocity;
    public bool isPathing = false;
    public bool isSqueak = false;
    public float squeakTime;
    private float internalSqueakTime;
    private float baseSpeed;
    //Start
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = mouseStates.Run;
        agent.destination = waypoints[0].transform.position;
        internalSqueakTime = squeakTime;
        if (behaviorType == mouseType.Normal)
        {
            agent.speed = 3.0f;
        }
        if (behaviorType == mouseType.Quick)
        {
            agent.speed = 4.0f;
        }
        if (behaviorType == mouseType.Thinker)
        {
            agent.speed = 2.0f;
        }
        baseSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = agent.velocity;

        isPathing = velocity.normalized.x != 0 ? true : false;
        if (isPathing)
        {
            //set true
            animator.SetBool("isPathing", true);
        }
        else
        {
            animator.SetBool("isPathing", false);
        }

        if (isSqueak)
        {
            if (internalSqueakTime > 0)
            {
                internalSqueakTime -= Time.deltaTime;
                agent.speed = baseSpeed * 2.0f;
            }
            else if (internalSqueakTime <= 0)
            {
                isSqueak = false;
                agent.speed = baseSpeed;
                internalSqueakTime = squeakTime;
            }
        }

        switch (currentState)
        {
            case mouseStates.Run:
                Run();
                break;
            case mouseStates.Think:
                Think();
                break;
        }
    }

    public void Run()
    {
        agent.destination = waypoints[waypointCounter].transform.position;
        if (Vector3.Distance(this.transform.position, agent.destination)< 1.0f)
        {
            if (waypointCounter + 1 == waypoints.Length)
            {
                //Lose
            }
            else
            {
                waypointCounter++;
                currentState = mouseStates.Think;
            }
        }
    }
    public void Think()
    {
        if (!getWaitTime)
        {
            if (behaviorType == mouseType.Normal)
            {
                waitTime = Random.Range(0.2f, 0.7f);
            }
            if (behaviorType == mouseType.Quick)
            {
                waitTime = Random.Range(0.05f, 0.25f);
            }
            if (behaviorType == mouseType.Thinker)
            {
                waitTime = Random.Range(0.6f, 1.2f);
            }
            getWaitTime = true;
        }

        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            getWaitTime = false;
            currentState = mouseStates.Run;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("play");
            isSqueak = true;
        }
    }
}
