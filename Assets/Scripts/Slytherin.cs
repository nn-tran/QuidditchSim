using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//speed limit from
//https://answers.unity.com/questions/9985/limiting-rigidbody-velocity.html
//

public class Slytherin : MonoBehaviour
{
    public GameObject snitch;
    public Rigidbody rb;
    public LineRenderer lr;
    public float maxspeed;
    private GameObject[] friends;
    // Start is called before the first frame update
    void Start()
    {
        friends = GameObject.FindGameObjectsWithTag("Slytherin");
        //normalize maxspeed using Box-Mueller transform
        maxspeed = UniformToNormal(16f,2f);
        Debug.Log(maxspeed);
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    float UniformToNormal(float mean, float deviation){
        return (float)(mean+2*deviation*(System.Math.Sqrt(-2f*System.Math.Log(Random.Range(0f,1f))) * System.Math.Cos(Random.Range(0f,1f)*2f*System.Math.PI)) - 2*deviation);
    }
    
    void FixedUpdate()
    {
        rb.AddForce(snitch.transform.position-transform.position);
        float speed = rb.velocity.magnitude;  // test current object speed
        if (speed > maxspeed)
        {
            float brakeSpeed = speed - maxspeed;  // calculate the speed decrease
     
            Vector3 normalisedVelocity = rb.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value
            rb.AddForce(-brakeVelocity);  // apply opposing brake force
        }
        foreach (var friend in friends)
        {
            Vector3 translate = transform.position - friend.transform.position;
            float dist = translate.magnitude;
            if (dist != 0) rb.AddForce(translate.normalized);
        }
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + Vector3.Normalize(rb.velocity)*5f);
    }
}
