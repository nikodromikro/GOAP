using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

class Node
{
    public GAction action;
  public  int cost;
    public Node parent;
    public Dictionary<string, int> worldStates;

    public Node(GAction action, int cost, Node parent, Dictionary<string, int> worldState, Dictionary<string, int> beliefState)
    {
        this.action = action;
        this.cost = cost;
        this.parent = parent;
        worldStates = new Dictionary<string, int>(worldState);
        if(beliefState != null )
        foreach(KeyValuePair<string, int> kvp in beliefState)
        {
            
            if(!worldStates.ContainsKey(kvp.Key))
            {
                worldStates.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
public class GPlanner  
{
    public static Queue<GAction> Plan(List<GAction> avalibleActions, WorldState goal,Dictionary<string,int> agentBelief)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction action in avalibleActions) action.Reset(); 
        foreach (GAction action in avalibleActions)
        {
            if(action.CheckProceduralCondiiton())
            {
                usableActions.Add(action);
                
            }
        }

        Queue<GAction> planActions = new Queue<GAction>();
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, null, GWorld.Instance.GetState(), agentBelief);


        bool successed = BuildGraph( usableActions, goal, start, leaves);
        
        if(!successed)
        {
            Debug.Log("Plan not found");
            return null;
        }

        Node cheapest=null;

        foreach(Node node in leaves)
        {
            if(cheapest == null)
            {
                cheapest = node;
            }
            else
            {
                if(node.cost<cheapest.cost)
                {
                    cheapest = node;
                }
            }
        }


        List<GAction> unorderedActions = new List<GAction>();
        Node n = cheapest;
        while (n.parent != null)
        {
            unorderedActions.Add(n.action);
            n = n.parent;

        }

         unorderedActions.Reverse();

        foreach(GAction action in unorderedActions)
        {
            planActions.Enqueue(action);
        }

        return planActions;

    }

    private static bool BuildGraph(List<GAction> usableActions, WorldState goal, Node start, List<Node> leaves)
    {
         bool foundOne = false;

        foreach(GAction action in usableActions)
        {
           
            if(InState(action.precondition,start.worldStates))
            {
                Dictionary<string,int> AffectedState = ApplyEffect(action.postConditions,start.worldStates);

                Node affectedNode = new Node(action, action.cost + start.cost, start, AffectedState, null);

                Dictionary<string, int> goalDic = new Dictionary<string, int>();
                goalDic.Add(goal.key, goal.value);

                if (InState(goalDic,AffectedState ))
                {
                    leaves.Add(affectedNode);
                    foundOne = true;
                    return foundOne;
                }
                else
                {
                    List<GAction> subSetAction =ExcludeActionFromSet(usableActions, action);
                    bool found = BuildGraph(subSetAction, goal, affectedNode, leaves);
                    if (found) { foundOne = true; } 
                }

            }
        }
        return foundOne;
    }

    private static List<GAction> ExcludeActionFromSet(List<GAction> usableActions, GAction action)
    {
       
        List<GAction> subSetedSet = new List<GAction>();
        foreach(GAction act in usableActions)
        {
            if(! act.Equals(action))
            {
                subSetedSet.Add(act);
            }
        }
        return subSetedSet;
    }

    private static Dictionary<string, int> ApplyEffect(Dictionary<string, int> postConditions, Dictionary<string, int> worldStates)
    {
        Dictionary<string, int> modifiedState = new Dictionary<string, int>(worldStates);

        foreach(KeyValuePair<string, int> kvp in postConditions)
        {
            if(modifiedState.ContainsKey(kvp.Key))
            {
                modifiedState.Remove(kvp.Key);
                modifiedState[kvp.Key] = kvp.Value;
            }
            else
            {
                modifiedState.Add(kvp.Key, kvp.Value);
            }
        }
        return modifiedState;

    }

    static bool InState(Dictionary<string,int> subSet,Dictionary<string,int> motherSet)
    {
        foreach(KeyValuePair<string,int> kvp in subSet)
        {
            bool match= false;
            foreach (KeyValuePair<string, int> m in motherSet)
            {
             if( kvp.Equals(m))
             {
                match = true;
                    break;
              }
            }
            if(!match)
            {
                return false;
            }
           
        }
        return true;
    }


}
