using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour
{
    NPCTankController npcScript;
    PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        npcScript = GetComponentInParent<NPCTankController>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        if (npcScript.aggroType.ToString() != "Sound" && npcScript.aggroType.ToString() != "Investigate")
        {
            this.gameObject.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerScript != null)
        {
            Aspect aspect = other.GetComponent<Aspect>();
            if (aspect != null)
            {
                if (aspect.aspectName == Aspect.aspect.Player || aspect.aspectName == Aspect.aspect.Enemy)
                { //add to the list 
                    //GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = true;
                    npcScript.canHearObjects.Add(other.gameObject);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (playerScript != null)
        {
            Aspect aspect = other.GetComponent<Aspect>();
            if (aspect != null)
            {
                if (aspect.aspectName == Aspect.aspect.Player )
                { //add to the list 
                   // GameObject.FindWithTag("Player").GetComponent<PlayerScript>().isHeard = false;
                    npcScript.canHearObjects.Remove(other.gameObject);
                    //npcScript.SetTransition(Transition.LostPlayer);
                }else if(aspect.aspectName == Aspect.aspect.Enemy)
                {
                    npcScript.canHearObjects.Remove(other.gameObject);
                }

            }
        }
    }
}
