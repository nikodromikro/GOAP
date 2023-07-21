using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodCutter : GAgent
{





    // Start is called before the first frame update
    void Start()
    {
         
        agentBelief.Add("hasAxe", hasAxe ? 1 : 0);
        agentBelief.Add("hasWood", hasWood ? 1 : 0);


        WorldState chopWoods = new WorldState() { key = "dropOffWood", value = 1 };
        goals.Add(chopWoods, 2);
        
    }

    
}
