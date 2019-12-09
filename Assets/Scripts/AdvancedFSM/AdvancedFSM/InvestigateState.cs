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
            RaycastHit hit;
            npcScript.rayDirection = player.position - npc.position;             
            // Detect if player is within the field of view
            if (Physics.Raycast(npc.position, npcScript.rayDirection, out hit, npcScript.ViewDistance))
            {
            //get aspect on the hit 
            Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    if (aspect.aspectName == Aspect.aspect.Player)
                    {
                        Debug.Log("Enemy Seen");
                        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isSeen = true;

                        npcScript.SetTransition(Transition.SawPlayer);
                        investigateTime = 0;
                    }
                else if(aspect.aspectName == Aspect.aspect.Wall)
                    {
                        Debug.Log("Wall seen");
                        int randNum = Random.Range(0, 100);
                        if(randNum < npcScript.chaseProb)
                        {
                            npcScript.SetTransition(Transition.SawPlayer);
                            investigateTime = 0;
                        }
                        else
                        {
                            npcScript.SetTransition(Transition.LostPlayer);
                            GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = false;
                            investigateTime = 0;
                        }
                    }
                }
            }
            if(dist >= 1000.0f)
            {
                npcScript.SetTransition(Transition.LostPlayer);
                GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = false;
                investigateTime = 0;
            }
            npcScript.detectionTimer = 0;
        }
     }


    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();
        investigateTime += Time.deltaTime;
        destPos = player.position;
        if (investigateTime <= 2.0f)
        {
            npcScript.targetRotation = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.Slerp(npc.rotation, npcScript.targetRotation, Time.deltaTime * npcScript.curRotSpeed);
        }
    }
}


