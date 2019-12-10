using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveToMouse : MonoBehaviour
{
    Camera mainCam;

    private NavMeshAgent agent;
    private Rigidbody rigidbody;
    public Vector3 velocity;
    public Vector3 m_velocity;
    private PlayerScript playerScr;
    private NPCTankController agentScript;

    public Animator animator;

    public float moveSpeed;
    public bool isPathing = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        playerScr = GetComponent<PlayerScript>();
    }

    void Update()
    {
        velocity = agent.velocity;

        isPathing = velocity.normalized.x != 0 ? true : false;
        if (isPathing)
        {
            //set true
            animator.SetBool("isPathing", true);
        }
        else
        {
            animator.SetBool("isPathing", false);
        }

        if (playerScr.canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    agent.isStopped = false;
                    rigidbody.constraints = RigidbodyConstraints.None;
                    agent.destination = hit.point;
                }
            }
        }
        if (velocity == new Vector3(0, 0, 0))
        {

            if(agentScript != null)
                agentScript.agent.isStopped = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
        }
       
        //Vector3 mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y));
        //transform.LookAt(mousePos + Vector3.up * transform.position.y);
        //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }
    private void FixedUpdate()
    {
        //rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}

