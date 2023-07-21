using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GAction  :MonoBehaviour

{  
    
    [SerializeField] bool requireInRange;
    public bool performing; 
    public bool inRange;
    public int cost=1;
    public Transform target;
    public float duration = 1f;
    public bool isDone=false;


    [SerializeField] WorldState[] Inspreconditions;
    [SerializeField] WorldState[] Inspostconditions;
  

    public Dictionary<string,int> precondition = new Dictionary<string,int>();
    public Dictionary<string,int> postConditions = new Dictionary<string,int>();
    public Dictionary<string, int> agentBeliefInAction = new Dictionary<string, int>();



    public bool RequireInRange => requireInRange;
    public bool InRange
    {
        get
        {
            if (Vector3.Distance(this.transform.position, target.position) < 1) return true;
            else return false;
        }
    }
    private void Awake()
    {
        foreach(WorldState state in Inspreconditions)
        {
            precondition.Add(state.key, state.value);
        }
        foreach(WorldState state in Inspostconditions)
        {
            postConditions.Add(state.key, state.value);
        }
    }


    public abstract bool CheckProceduralCondiiton( );
    public abstract bool Preperform();
    public abstract bool Postperform();
    public virtual  void Reset()
    {
        isDone = false;
        target = null;
    }
}
