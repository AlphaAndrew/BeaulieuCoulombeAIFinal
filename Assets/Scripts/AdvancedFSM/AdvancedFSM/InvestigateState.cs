using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvestigateState : FSMState
{
    private float investigateTime = 0;
    private float distToSardine = 0;
    public InvestigateState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Investigating;
        // curRotSpeed = 1.0f;
        // curSpeed = 100.0f;
    }
    public override void Reason(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();

        if (npcScript.targetSardine == null)
        {

            npcScript.agent.isStopped = false;
            investigateTime = 0;

            npcScript.findPointCounter = 0;
            Debug.Log("Waypoint count is" + waypointCounter);
            Debug.Log("Waypoing Length is" + waypoints.Length);
            //if (waypointCounter < waypoints.Length - 1)
            //{
            //    Debug.Log("+1");
            //    waypointCounter++;
            //    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
            //}
            //else
            //{
            //    Debug.Log("0");
            //    waypointCounter = 0;
            //    npcScript.agent.SetDestination(waypoints[waypointCounter].position);

            //}
            npcScript.isEatingSardines = false;
            npcScript.SetTransition(Transition.LostPlayer);
        }
        else
        {
            distToSardine = Vector3.Distance(npc.position, npcScript.targetSardine.transform.position);

            investigateTime += Time.deltaTime;

            if (distToSardine <= 3.5f)
            {
                npcScript.agent.velocity = Vector3.zero;
                npcScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                npcScript.agent.isStopped = true;

            }
            if (investigateTime >= 6f)
            {
                investigateTime = 0;
                npcScript.findPointCounter = 0;

                //if (waypointCounter < waypoints.Length - 1)
                //{
                //    waypointCounter += 1;
                //    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
                //}
                //else
                //{
                //    waypointCounter = 0;
                //    npcScript.agent.SetDestination(waypoints[waypointCounter].position);

                //}
                npcScript.agent.isStopped = false;
                npcScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                npcScript.isEatingSardines = false;
                npcScript.SetTransition(Transition.LostPlayer);
            }
        }
    }


    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();

        if (npcScript.targetSardine != null)
        {
           
            distToSardine = Vector3.Distance(npc.position, npcScript.targetSardine.transform.position);

            npcScript.agent.SetDestination(npcScript.targetSardine.transform.position);
        }
        
    }
}


