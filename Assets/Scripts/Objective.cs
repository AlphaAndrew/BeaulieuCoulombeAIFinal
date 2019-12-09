using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public string objMessage1;
    public Text objText1;
    public string objMessage2;
    public Text objText2;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        objText1.text = objMessage1;
        objText2.text = objMessage2;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            objText1.enabled = false;
            objText2.enabled = false;
        }
    }
}
