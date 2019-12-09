using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolState : FSMState
{
    //Initialize & set up bored timer

    public PatrolState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Patrolling;

       // curRotSpeed = 1.0f;
       // curSpeed = 100.0f;
    }

    public override void Reason(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.GetComponent<NPCTankController>();
        npcScript.detectionTimer += Time.deltaTime;

        if (npcScript.detectionTimer >= npcScript.detectionRate)
        {
            if (npcScript.aggroType.ToString() == "LineOfSight")
            {
                //RaycastHit hit;
                //npcScript.rayDirection = player.position - npc.position;
                //if ((Vector3.Angle(npcScript.rayDirection, npc.forward)) < npcScript.FieldOfView)
                //{

                //    // Detect if player is within the field of view
                //    if (Physics.Raycast(npc.position, npcScript.rayDirection, out hit, npcScript.ViewDistance))
                //    {
                //        //get aspect on the hit 
                //        Aspect aspect = hit.collider.GetComponent<Aspect>();
                //        if (aspect != null)
                //        {
                //            //Debug.Log("Hit something");

                //            if(aspect.aspectName == Aspect.aspect.Enemy)
                //            {
                //                Debug.Log("Enemy Seen");
                //                GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isSeen = true;

                //                npcScript.SetTransition(Transition.SawPlayer);
                //            }
                //        }
                //        else
                //        {
                //            if (npcScript.CurrentStateID != FSMStateID.Patrolling)
                //                npcScript.SetTransition(Transition.LostPlayer);
                //        }
                //    }
                //}
            }
            else if (npcScript.aggroType.ToString() == "Sound")
            {
                //if palyer is in list and is moving, else invoke transidtion to patorl after
                if (npcScript.canHearObjects.Contains(player.gameObject))
                {
                    if (npcScript.canHearObjects.Count < 2)
                    {
                        //100% chance
                        npcScript.cantHearDebug = true;
                        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = true;
                        //npcScript.SetTransition(Transition.SawPlayer);
                    }
                    else
                    {
                        //Cuts chance in half depending on the amount of tanks in the list
                       
                        int randNum;
                        randNum = Random.Range(1, 100);
                        Debug.Log("testRand");
                        if (randNum < (100 / (npcScript.canHearObjects.Count*5)))
                        {
                            npcScript.cantHearDebug = true;
                            GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = true;
                          //  npcScript.SetTransition(Transition.SawPlayer);
                        }
                        
                    }
                }
            }else if(npcScript.aggroType.ToString() == "Touch")
            {

            }else if(npcScript.aggroType.ToString() == "Investigate")
            {
                if(npcScript.canHearObjects.Contains(player.gameObject)){
                    Debug.Log("Investigating");
                    npcScript.SetTransition(Transition.SmelledSardine);
                }
            }
            else
            {

            }
            npcScript.detectionTimer = 0;
        }

        if (Vector3.Distance(npc.position, waypoints[waypointCounter].position) <= 3.0f)
        {
            Debug.Log("reached point");
            if (npcScript.pathing == PathingMode.Random)
            {
                FindNextRandPoint();
            }
            else if (npcScript.pathing == PathingMode.Order)
            {
                if (waypointCounter < waypoints.Length - 1)
                {
                    waypointCounter += 1;
                    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
                }
                else
                {
                    waypointCounter = 0;
                    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
                }

            }

        }
    }




    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.GetComponent<NPCTankController>();


        if (npcScript.findPointCounter == 0)
        {
            if (npcScript.pathing == PathingMode.Random)
            {
                FindNextRandPoint();
            }
            else if (npcScript.pathing == PathingMode.Order)
            {
                if (waypointCounter < waypoints.Length - 1)
                {
                    waypointCounter += 1;
                    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
                }
                else
                {
                    waypointCounter = 0;
                    npcScript.agent.SetDestination(waypoints[waypointCounter].position);
                }
            }
            npcScript.findPointCounter++;
        }







        //Rotate to the target point
        //Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            //npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * npcScript.curRotSpeed);
    }
   
}