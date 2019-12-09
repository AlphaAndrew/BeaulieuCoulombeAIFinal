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


    //Start
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = mouseStates.Run;
        agent.destination = waypoints[0].transform.position;

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
    }

    // Update is called once per frame
    void Update()
    {
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
}
