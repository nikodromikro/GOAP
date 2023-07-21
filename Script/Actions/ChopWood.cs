using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopWood : GAction
{
    public override bool CheckProceduralCondiiton()
    {
        GameObject tree = GameObject.FindGameObjectWithTag("Tree");
        if (tree != null)
        {
            target = tree.transform;
            return true;
        }
        return false;
    }

    public override bool Postperform()
    {
        GWorld.Instance.SetState("hasWood", 1);
        return true;
    }

    public override bool Preperform()
    {
         return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
