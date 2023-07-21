using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffWoods : GAction
{
    public override bool CheckProceduralCondiiton()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Pile");
        if (go == null) return false;

        target = go.transform;
        return true;
    }

    public override bool Postperform()
    {
        GWorld.Instance.ModifyState("number of woods", 1);
        GWorld.Instance.SetState("hasWood", 0);
       //if(agentBeliefInAction.ContainsKey("hasWood"))
       // {
       //     agentBeliefInAction.Remove("hasWood");
       //     agentBeliefInAction.Add("hasWood", 0);
       // }
        return true;
    }

    public override bool Preperform()
    {
        return true;
    }
}
