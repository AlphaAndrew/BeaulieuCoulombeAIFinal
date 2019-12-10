﻿using System.Collections;
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
           // npcScript.findPointCounter = 0;
            npcScript.isEatingSardines = false;
            npcScript.SetTransition(Transition.LostPlayer);
        }
        else
        {
            distToSardine = Vector3.Distance(npc.position, npcScript.targetSardine.transform.position);

            investigateTime += Time.deltaTime;

            if (distToSardine <= 3.5f)
            {
                npcScript.agent.isStopped = true;
                npcScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            }
            if (investigateTime >= 4f)
            {
                investigateTime = 0;
             //   npcScript.findPointCounter = 0;
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

        if (npcScript.targetSardine == null)
        {
            npcScript.agent.isStopped = false;
            investigateTime = 0;
            //npcScript.findPointCounter = 0;
            npcScript.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            npcScript.isEatingSardines = false;
            npcScript.SetTransition(Transition.LostPlayer);
        }
        else
        {
            distToSardine = Vector3.Distance(npc.position, npcScript.targetSardine.transform.position);

            npcScript.agent.SetDestination(npcScript.targetSardine.transform.position);
        }
        
    }
}


