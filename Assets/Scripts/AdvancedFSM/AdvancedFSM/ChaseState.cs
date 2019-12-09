using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    public ChaseState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;
        //find next Waypoint position
        FindNextRandPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();
        npcScript.detectionTimer += Time.deltaTime;

        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state or retreat
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 200.0f)
        {

            npcScript.SetTransition(Transition.ReachPlayer);
            Debug.Log("Attacking");


        }
        if (npcScript.detectionTimer >= npcScript.detectionRate)
        {
            if (npcScript.aggroType.ToString() == "LineOfSight")
            {

                  RaycastHit hit;
                 npcScript.rayDirection = player.position - npc.position;
                if ((Vector3.Angle(npcScript.rayDirection, npc.forward)) < npcScript.FieldOfView)
                {
                    // Detect if player is within the field of view
                    if (Physics.Raycast(npc.position, npcScript.rayDirection, out hit, npcScript.ViewDistance))
                    {
                        if (hit.collider.tag == "Wall")
                        {
                            Debug.Log("EnemyLost");
                            GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isSeen = false;

                            npcScript.SetTransition(Transition.LostPlayer);
                        }
                          //Go back to patrol if it has become too far
                        if (dist >= 1000.0f)
                        {
                            //recently active, so we set time to bored back to 0
                            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
                            GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isSeen = false;
                        }
                    }

                }
                

            }
            else if (npcScript.aggroType.ToString() == "Sound")
            {
                //if the tank is moving, thus making noise
                if (npcScript.canHearObjects.Contains(player.gameObject))
                {
                    if (npcScript.cantHearDebug == true)
                    {
                        Debug.Log("stopped moving,calling coroutine");
                        npcScript.SetTransition(Transition.LostPlayer);
                        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = false;
                        npcScript.cantHearDebug = false;
                    }
                }
                if (dist >= 1000.0f)
                {
                    //recently active, so we set time to bored back to 0
                    npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
                    GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isSeen = false;
                }
            }
            else if(npcScript.aggroType.ToString() == "Touch")
            {
                if (Vector3.Distance(player.position, npc.position) >= 230.0f)
                {
                    npcScript.SetTransition(Transition.LostPlayer);
                }
            }
            else if (npcScript.aggroType.ToString() == "Investigate")
            {
                if (Vector3.Distance(player.position, npc.position) >= 1000.0f)
                {
                    npcScript.SetTransition(Transition.LostPlayer);
                }
            }
            npcScript.detectionTimer = 0.0f;
        }
    }
    public override void Act(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();
        
        //Change color
        npcScript.gameObject.GetComponent<Renderer>().material.color = Color.yellow;

        destPos = player.position;

        //rotate to target its chasgin
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * npcScript.curRotSpeed);

        //Rotate turret to target
        Transform turret = npc.GetComponent<NPCTankController>().turret.transform;
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * npcScript.curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * npcScript.curSpeed);
    }
    
}
