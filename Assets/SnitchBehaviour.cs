using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://answers.unity.com/questions/1369351/unity-2d-random-movement.html



public class SnitchBehaviour : MonoBehaviour
{
    public float accelerationTime = 1f;
    public float maxSpeed = 1f;
    private Vector3 movement;
    private float timeLeft;
    public Rigidbody rb;
    public float speed = 1;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0){
            movement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            //Debug.Log(movement);
            timeLeft += accelerationTime;
        }
    }
 
    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }
}

 
 