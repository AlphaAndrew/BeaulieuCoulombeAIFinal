using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveToMouse : MonoBehaviour
{
    Camera mainCam;

    private NavMeshAgent agent;
    private Rigidbody rigidbody;
    private Vector3 velocity;

    public float moveSpeed;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
         if (Input.GetMouseButtonDown(0))
         {
             RaycastHit hit;

             if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
             {
                 agent.destination = hit.point;
             }
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

