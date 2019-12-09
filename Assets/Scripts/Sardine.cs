﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sardine : MonoBehaviour
{
    private Rigidbody rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, force));
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
