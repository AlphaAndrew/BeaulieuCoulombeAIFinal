using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvestigateState : FSMState
{
    private float investigateTime = 0;

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
        float distToSardine = Vector3.Distance(npc.position, npcScript.targetSardine.transform.position);

       investigateTime += Time.deltaTime;
       
        if(distToSardine <= 2.0f)
        {
            npcScript.agent.isStopped = true;
        }
        if(investigateTime >= 5.0f)
        {
            npcScript.SetTransition(Transition.LostPlayer);
            investigateTime = 0;
            npcScript.findPointCounter = 0;
            npcScript.agent.isStopped = false;
        }
    }


    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();

        npcScript.agent.SetDestination(npcScript.targetSardine.transform.position);
        
    }
}


