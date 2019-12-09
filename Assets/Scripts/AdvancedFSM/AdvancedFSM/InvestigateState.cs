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

        //find next Waypoint position
        FindNextRandPoint();
    }
    public override void Reason(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();
        float dist = Vector3.Distance(player.position, npc.position);
        npcScript.detectionTimer += Time.deltaTime;
        Debug.Log("reason");


        if (npcScript.detectionTimer >= npcScript.detectionRate && investigateTime >= 1.0f)
        {
           
        }
     }


    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();
       
    }
}


