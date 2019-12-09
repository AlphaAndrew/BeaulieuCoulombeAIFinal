using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
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
    public void WinGame()
    {

    }
}
