using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//random movement code based on https://answers.unity.com/questions/1369351/unity-2d-random-movement.html
//misc code from https://docs.unity3d.com

public class SnitchBehaviour : MonoBehaviour
{
    public float accelerationTime = 1f;
    public Vector3 movement;
    public float timeLeft;
    public Rigidbody rb;
    public LineRenderer lr;
    public float speed = 1f;
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

 
 