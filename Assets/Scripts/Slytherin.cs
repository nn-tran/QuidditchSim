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
    public float weight;
    public float aggression;
    public float exhaustion;
    public float current_exhaustion = 0f;
    public float timer = 0f;
    public GameObject[] friends;
    // Start is called before the first frame update
    void Start()
    {
        friends = GameObject.FindGameObjectsWithTag("Slytherin");
        transform.Translate(new Vector3(Random.Range(-10f,10f), Random.Range(-10f,10f), Random.Range(-10f,10f)));
        maxspeed = UniformToNormal(16f,2f);
        weight = UniformToNormal(85f,17f);
        aggression = UniformToNormal(30f, 7f);
        exhaustion = UniformToNormal(50f,15f);
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //normalizer using Box-Mueller transform
    float UniformToNormal(float mean, float deviation){
        return (float)(mean+deviation*(System.Math.Sqrt(-2f*System.Math.Log(Random.Range(0f,1f))) * System.Math.Cos(Random.Range(0f,1f)*2f*System.Math.PI)));
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
            Vector3 avoidForce = transform.position - friend.transform.position;
            if (avoidForce.magnitude != 0) rb.AddForce(avoidForce.normalized);
        }
        
        //moving away from wall is approx. equivalent to moving to center
        Vector3 toCenter = new Vector3(0, 100, 0) - transform.position;
        rb.AddForce(toCenter.normalized * (float) System.Math.Sqrt(toCenter.magnitude));
        
        current_exhaustion += 0.1f;
        if (current_exhaustion > exhaustion){
            rb.velocity = new Vector3(0,0,0);
            timer+= Time.deltaTime;
            if (timer > 3f){
                timer = 0f;
                current_exhaustion = 0f;
            }
        }
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + rb.velocity.normalized*5f);

    }
}
