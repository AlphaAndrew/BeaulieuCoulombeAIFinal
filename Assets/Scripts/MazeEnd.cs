using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnd : MonoBehaviour
{
    private GameObject[] enemies;
    public GameObject winScreen;
    public GameObject loseScreen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemies = GameObject.FindGameObjectsWithTag("EnemyMouse");
            foreach(GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            winScreen.SetActive(true);
        }
        if (other.gameObject.CompareTag("EnemyMouse"))
        {
            //Turn off movement
            loseScreen.SetActive(true);
        }
    }
}
