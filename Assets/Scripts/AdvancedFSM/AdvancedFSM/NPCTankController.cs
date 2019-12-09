using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class NPCTankController : AdvancedFSM
{
    public GameObject Bullet;
    public float health;
    public Text tankHealthText;

 
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
    public int fastFireProb = 20;
    public int slowFireProb = 80;
    public bool fastttt = false;

    public int FieldOfView;
    public int ViewDistance;
    public Vector3 rayDirection;
    public Aspect.aspect aspectName = Aspect.aspect.Enemy;

    public ArrayList fireRate = new ArrayList();

    public Quaternion targetRotation;

    public int chaseProb = 80;
    public enum AggroType
    {
        LineOfSight,
        Sound,
        Touch,
        Investigate,
        None
    }
    public AggroType aggroType;

    public List<GameObject> canHearObjects;
    public bool cantHearDebug = false;
    public float detectionRate = 0.25f;
    public float detectionTimer = 0.0f;

    protected override void Initialize()
    {
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
        turret = gameObject.transform.GetChild(0).gameObject;
        bulletSpawnPoint = turret.transform.GetChild(0).transform;

        //Start Doing the Finite State Machine
        ConstructFSM();
    }


    //Update each frame
    protected override void FSMUpdate()
    {

        
        tankHealthText.text = "Tanks Remaining: " + ListCount();
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
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        patrol.AddTransition(Transition.HeardSomething, FSMStateID.Investigating);

        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        InvestigateState investigate = new InvestigateState(waypoints);
        investigate.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        investigate.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        investigate.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        

        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

       

        //Add states to the fsmstates list
        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);
        AddFSMState(investigate);
    }
    public IEnumerator DelayTransition(float time, Transition transitionEvent)
    {

            Debug.Log("incoroutine");
            yield return new WaitForSeconds(time);
            SetTransition(transitionEvent);
      
        
        StopCoroutine(DelayTransition(time, transitionEvent));
        yield return null;
    }
    void OnDrawGizmos()
    {

        if (!Application.isEditor || playerTransform == null)
            return;

        Debug.DrawLine(transform.position, transform.position, Color.blue);

        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        //Approximate perspective visualization
        Vector3 leftRayPoint = Quaternion.Euler(0, FieldOfView * 0.5f, 0) * frontRayPoint;

        Vector3 rightRayPoint = Quaternion.Euler(0, -FieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
        Debug.DrawLine(transform.position, leftRayPoint, Color.red);
        Debug.DrawLine(transform.position, rightRayPoint, Color.red);
    }
    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 20;

            if (health <= 0)
            {
                ListCount();
                //if (EnemyTanks[counter] == gameObject && ListCount() > 1)
                //{
                //    if (counter < ListCount())
                //    {
                //        Debug.Log("CounterUp");
                //        foreach (GameObject tank in EnemyTanks)
                //        {
                //            if (tank != null)
                //            {
                //                if (tank.GetComponent<NPCTankController>().counter < ListCount() - 1)
                //                {
                //                    tank.GetComponent<NPCTankController>().counter++;
                //                }
                //                else
                //                {
                //                    tank.GetComponent<NPCTankController>().counter = 0;
                //                }
                //            }
                //        }
                //    }
                //}
                Debug.Log("Switch to Dead State");
                SetTransition(Transition.NoHealth);
                Explode();
            }
        }
 
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
    protected void Explode()
    {
        foreach (GameObject tank in EnemyTanks)
        {
            if (tank != null)
            {
                tank.GetComponent<NPCTankController>().chaseProb -= 20;
            }
        }
        if (ListCount() == 1)
        {
            playerTransform.gameObject.GetComponent<PlayerScript>().WinGame();
        }
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            rigidbody.AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            rigidbody.velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
      
        Destroy(gameObject, 1.5f);
     
    }



    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }
}
