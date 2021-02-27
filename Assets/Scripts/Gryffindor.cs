using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//speed limit from
//https://answers.unity.com/questions/9985/limiting-rigidbody-velocity.html
//misc code from https://docs.unity3d.com

public class Gryffindor : MonoBehaviour
{
    public GameObject snitch;
    public Rigidbody rb;
    public LineRenderer lr;
    public float maxspeed;
    public float weight;
    public float aggression;
    public float exhaustion;
    public float current_exhaustion = 0f;
    public bool unconscious = false;
    
    public float timer = 0f;
    public GameObject[] friends;
    // Start is called before the first frame update
    void Start()
    {
        friends = GameObject.FindGameObjectsWithTag("Slytherin");
        transform.Translate(new Vector3(Random.Range(-10f,10f), Random.Range(-10f,10f), Random.Range(-10f,10f)));
        maxspeed = UniformToNormal(18f,2f);
        weight = UniformToNormal(55f,12f);
        aggression = UniformToNormal(22f, 3f);
        exhaustion = UniformToNormal(65f,13f);
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //collision handler
    IEnumerator OnCollisionEnter(Collision collision){
        
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Wall")
        {
            unconscious = true;
            rb.useGravity = true;
            yield return null;
        }
        if (collision.gameObject.tag == "Floor")
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            transform.position = new Vector3(-25f + Random.Range(-10f, 10f),25f + Random.Range(-10f, 10f),-25f + Random.Range(-10f, 10f));
            rb.useGravity = false;
            unconscious = false;
            yield return null;
        }
        
        if (collision.gameObject.tag == "Gryffindor")
        {
            //unique trait: encouraging
            collision.rigidbody.velocity *= 1.2f;
            if (Random.value < 0.05){
                CollisionDecider(collision.gameObject);
            }
            yield return null;
        }
        
        if (collision.gameObject.tag == "Snitch")
        {
            Scores.score_gryffindor++;
            if (Scores.last_score == 0) Scores.score_slytherin++;
            else Scores.last_score = 0;
            yield return null;
        }
    }
    
    //code decides who wins in a collision
    //opposing-team collision handled only by Slytherin for consistency
    //same-team collision is separate
    int CollisionDecider(GameObject opponent){
        float value1 = (float) aggression * (Random.value * (1.2f - 0.8f) + 0.8f) *  (1f -(current_exhaustion / exhaustion));
        
        float value2 = 0f;

        //the 2 branches are identical, except for the type of the opponent
        if (opponent.GetComponent(typeof(Gryffindor)) != null)
        {
            Gryffindor op = (Gryffindor) opponent.GetComponent(typeof(Gryffindor));
            value2 = (float) op.aggression * (Random.value * (1.2f - 0.8f) + 0.8f) *  (1f -(op.current_exhaustion / op.exhaustion));
        
            if (value1 < value2)
            {
                unconscious = true;
                rb.useGravity = true;
                return 0;
            } else {
                op.unconscious = true;
                op.rb.useGravity = true;
                return 1;
            }
        } else {
            Slytherin op = (Slytherin) opponent.GetComponent(typeof(Slytherin));
            value2 = (float) op.aggression * (Random.value * (1.2f - 0.8f) + 0.8f) *  (1f -(op.current_exhaustion / op.exhaustion));
            if (value1 < value2)
            {
                unconscious = true;
                rb.useGravity = true;
                return 0;
            } else {
                op.unconscious = true;
                op.rb.useGravity = true;
                return 1;
            }
        }
        
        
    }
    
    //normalizer using Box-Mueller transform
    float UniformToNormal(float mean, float deviation){
        return (float)(mean+deviation*(System.Math.Sqrt(-2f*System.Math.Log(Random.Range(0f,1f))) * System.Math.Cos(Random.Range(0f,1f)*2f*System.Math.PI)));
    }
    
    void FixedUpdate()
    {
        //don't add force on knockout (momentum is maintained)
        if (unconscious) {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + rb.velocity.normalized*5f);
            return;
        }
        
        //this code handles the speed limit
        rb.AddForce(snitch.transform.position-transform.position);
        float speed = rb.velocity.magnitude;  // test current object speed
        if (speed > maxspeed)
        {
            float brakeSpeed = speed - maxspeed;  // calculate the speed decrease
     
            Vector3 normalisedVelocity = rb.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value
            rb.AddForce(-brakeVelocity);  // apply opposing brake force
        }
        
        //trying to avoid friendlies
        foreach (var friend in friends)
        {
            Vector3 avoidForce = transform.position - friend.transform.position;
            if (avoidForce.magnitude != 0) rb.AddForce(avoidForce.normalized);
        }
        
        //moving away from wall is approx. equivalent to moving to center
        Vector3 toCenter = new Vector3(0, 50, 0) - transform.position;
        rb.AddForce(toCenter.normalized * (float) System.Math.Sqrt(toCenter.magnitude));
        
        //when exhausted, stop for 3 seconds
        current_exhaustion += 0.1f;
        if (current_exhaustion > exhaustion){
            rb.velocity = new Vector3(0,0,0);
            timer+= Time.deltaTime;
            if (timer > 3f){
                timer = 0f;
                current_exhaustion = 0f;
            }
        }
        
        //direction 'arrow'
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + rb.velocity.normalized*5f);

    }
}
