using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SardineCollider : MonoBehaviour
{
    NPCTankController npcScript;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        npcScript = GetComponentInParent<NPCTankController>();
      

    }
    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if(other.gameObject.tag == "Sardine")
            {
                this.gameObject.GetComponentInParent<NPCTankController>().SetTransition(Transition.SmelledSardine);
                npcScript.GetComponent<NPCTankController>().targetSardine = other.gameObject;
                npcScript.isEatingSardines = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}


