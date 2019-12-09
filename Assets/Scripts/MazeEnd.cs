using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnd : MonoBehaviour
{
    private GameObject[] enemies;
    public GameObject winScreen;
    public GameObject loseScreen;
    private PlayerScript playerScr;
    private void Start()
    {
        playerScr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerScr.canMove = false;
            enemies = GameObject.FindGameObjectsWithTag("EnemyMouse");
            foreach(GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            winScreen.SetActive(true);
        }
        if (other.gameObject.CompareTag("EnemyMouse"))
        {
            playerScr.canMove = false;
            loseScreen.SetActive(true);
        }
    }
}
