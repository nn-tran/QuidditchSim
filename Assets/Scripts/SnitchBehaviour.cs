using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//random movement code based on https://answers.unity.com/questions/1369351/unity-2d-random-movement.html



public class SnitchBehaviour : MonoBehaviour
{
    private float accelerationTime = 1f;
    private Vector3 movement;
    private float timeLeft;
    private Rigidbody rb;
    private LineRenderer lr;
    private float speed = 10f;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0){
            movement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
        
    }
 
    void FixedUpdate()
    {
        rb.AddForce(movement * speed);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + rb.velocity.normalized*5f);
    }
}

 
 