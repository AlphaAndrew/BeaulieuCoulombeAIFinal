using UnityEngine;
using System.Collections;

public class AttackState : FSMState
{
    public AttackState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Attacking;
       // curRotSpeed = 1.0f;
       // curSpeed = 100.0f;

        //find next Waypoint position
        FindNextRandPoint();
    }
    public override void Reason(Transform player, Transform npc)
    {
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();

        //Check the distance with the player tank
        if (player != null)
        {
            float dist = Vector3.Distance(npc.position, player.position);
            if (dist >= 220.0f && dist < 300.0f)
            { 

                npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
            }
            //Transition to patrol is the tank become too far
            else if (dist >= 300.0f)
            {
                Debug.Log("Switch to Patrol State");
                npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);

            }

        }
    }

    public override void Act(Transform player, Transform npc)
    {       
        NPCTankController npcScript = npc.gameObject.GetComponent<NPCTankController>();

       
      
        //Set the target position as the player position
        destPos = player.position;


        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * npcScript.curRotSpeed);

        //Go Forward
        if (npcScript.aggroType.ToString() != "Touch") { npc.Translate(Vector3.forward * Time.deltaTime * npcScript.curSpeed); }

        //Always Turn the turret towards the player
        Transform turret = npc.GetComponent<NPCTankController>().turret.transform;
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * npcScript.curTurretRotSpeed);

        //Shoot bullet towards the player
    }
   
}
