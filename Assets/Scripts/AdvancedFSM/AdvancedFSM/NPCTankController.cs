using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
public class NPCTankController : AdvancedFSM
{
    public float health;

    public NavMeshAgent agent;
    public bool isDead = false;
    // We overwrite the deprecated built-in `rigidbody` variable.
    new public Rigidbody rigidbody;

    //Tanks rotation speeds(allowing for different tankControllers to have different rotation speeds
    public float curTurretRotSpeed;
    public float curRotSpeed;
    public float curSpeed;
    public int findPointCounter = 0;
    //Waypoints
    public GameObject[] pointList;
    public bool isEatingSardines = false;

    public Vector3 rayDirection;


    public Aspect.aspect aspectName = Aspect.aspect.Enemy;

    public Quaternion targetRotation;

    public GameObject targetSardine;
    public float pathingBuffer = 0;

    public enum AggroType
    {
        LineOfSight,
        Sound,
        Investigate,
        None
    }
    public AggroType aggroType;

    public List<GameObject> canHearObjects;
    public bool cantHearDebug = false;
    public float detectionRate = 0.25f;
    public float detectionTimer = 0.0f;

    public Animator walkAnim;

    protected override void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        canHearObjects = new List<GameObject>();
        //setting rot variables
        curTurretRotSpeed = 0.5f;
        curRotSpeed = 1.0f;
        //move speed
        curSpeed = 100.0f;

        
        //health
        health = 100;
        //referencing the heal areas location
        elapsedTime = 0.0f;

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        //Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

        //Get the turret of the tank


        //Start Doing the Finite State Machine
        ConstructFSM();
    }


    //Update each frame
    protected override void FSMUpdate()
    {

        
        //Check for health
        elapsedTime += Time.deltaTime;
    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(playerTransform, transform);
        CurrentState.Act(playerTransform, transform);
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    private void ConstructFSM()
    {
        //Get the list of points
        

        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;
        foreach(GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }
        //Add transitions
        PatrolState patrol = new PatrolState(waypoints);
        
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        patrol.AddTransition(Transition.SmelledSardine, FSMStateID.Investigating);


        InvestigateState investigate = new InvestigateState(waypoints);
        investigate.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        investigate.AddTransition(Transition.NoHealth, FSMStateID.Dead);


        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

       

        //Add states to the fsmstates list
        AddFSMState(patrol);
        AddFSMState(dead);
        AddFSMState(investigate);
    }
   
    
    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
       
 
    }


    public float getHealth()
    {
        return health;
    }
    public void setHealth(float hp)
    {
        health = hp;
    }
    public void incrementHealth(float healthValue)
    {
        health += healthValue;
    }
    public int listCounter=0;
    public int ListCount()
    {
        listCounter = 0;
        foreach(GameObject tank in EnemyTanks)
        {
            if(tank!= null)
            {
                listCounter++;
            }
        }
        return listCounter;
    }
   
}
