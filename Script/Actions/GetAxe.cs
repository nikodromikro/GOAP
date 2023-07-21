using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAxe : GAction
{
    public override bool CheckProceduralCondiiton()
    {
        GameObject targetGameObj = GameObject.FindGameObjectWithTag("Axe");
        if(targetGameObj == null) return false;

        target = targetGameObj.transform;
        return true;
    }

    public override bool Postperform()
    {
        GWorld.Instance.SetState("hasAxe", 1);
        isDone = true;
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
