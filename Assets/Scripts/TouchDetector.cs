using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    NPCTankController npcScript;
    PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        npcScript = GetComponentInParent<NPCTankController>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        if (npcScript.aggroType.ToString() != "Touch")
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
                if (aspect.aspectName == Aspect.aspect.Player)
                {
                    //playerScript.isTouching = true;
                    npcScript.SetTransition(Transition.SawPlayer);
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
                if (aspect.aspectName == Aspect.aspect.Player)
                {

                   // playerScript.isTouching = false;
                }

            }
        }
    }
}
