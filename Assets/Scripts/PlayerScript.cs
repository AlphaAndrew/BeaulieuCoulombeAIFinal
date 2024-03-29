﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public bool canMove = true;
    public bool hasSardines;
    public GameObject sardine;
    public GameObject spawnPos;
    public float sardineCD;
    private float internalSardineCD;
    public bool isSeen;
    public bool isHeard;
    public GameObject loseScreen;
    public GameObject exclamationObject;
    private NPCTankController agentScript;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        internalSardineCD -= Time.deltaTime;
        if (hasSardines && (internalSardineCD <= 0))
        {
            if (Input.GetKeyDown("space"))
            {
                Instantiate(sardine, spawnPos.transform.position, spawnPos.transform.rotation);
                internalSardineCD = sardineCD;
            }
        }
    }
    public void FoundPlayer()
    {
        canMove = false;
        GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        GetComponent<NavMeshAgent>().isStopped = true;
        loseScreen.SetActive(true);
        exclamationObject.SetActive(true);
    }
    public void WinGame()
    {

    }
}
