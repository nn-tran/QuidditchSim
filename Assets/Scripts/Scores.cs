using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public static int score_slytherin = 0;
    public static int score_gryffindor = 0;
    public static int last_score = 0;
    public static bool game_over = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //scoreboard based on https://www.youtube.com/watch?v=fMd3B0T5ow0
    void OnGUI()
    {
        GUI.Box (new Rect (100, 100, 100, 100), 
            ("Slytherin:\n" + score_slytherin.ToString() + "\nGryffindor:\n" 
            + score_gryffindor.ToString()));
        GUI.Box (new Rect (0, 0, 100, 50), "Game Over");
    }
}
