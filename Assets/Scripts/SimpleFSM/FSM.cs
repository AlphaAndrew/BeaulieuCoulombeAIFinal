using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM : MonoBehaviour 
{
    //Player Transform
    protected Transform playerTransform;
    //Next destination position of the NPC Tank
    protected Vector3 destPos;
    //List of points for patrolling

    //Bullet shooting rate
    protected float shootRate;
    protected float elapsedTime;

    //Tank Turret
    public GameObject turret { get; set; }
    public Transform bulletSpawnPoint { get; set; }

    public List<GameObject> EnemyTanks;
    public int counter = 0;

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    
	// Use this for initialization
	void Start () 
    {
        GameObject[] Tanks = GameObject.FindGameObjectsWithTag("EnemyTank");
       // GameObject[] Tanks2 = GameObject.FindGameObjectsWithTag("FastTank");
        EnemyTanks = new List<GameObject>(Tanks);
        //for (int i = 0; i < Tanks.Length; i++){
        //    EnemyTanks.Add(Tanks2[i]);
        //}
        Initialize(); 
	}
	
	// Update is called once per frame
	void Update () 
    {
        FSMUpdate();
	}

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }    

}
