using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Analytics;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.AI;

public enum FSMStates { PLANING, MOVING, PERFORMING }
[RequireComponent(typeof(NavMeshAgent))]
public class GAgent : MonoBehaviour
{

    public Dictionary<WorldState,int> goals = new Dictionary<WorldState,int>();

    public List<GAction> avalibleActions = new List<GAction>();
    public Dictionary<string,int> agentBelief = new Dictionary<string,int>();


    public bool hasAxe=false;
    public bool hasWood=false;


    public FSMStates stateMachineState = FSMStates.PLANING;
    public Queue<GAction> planActions= null;
    GAction _currentAction= null;
    NavMeshAgent _navAgent;
    IEnumerator _performCoroutine = null;

    // Start is called before the first frame update
    void Awake()
    {
        GAction[] gActions = GetComponents<GAction>();
        foreach (GAction action in gActions)
        {
            avalibleActions.Add(action);
        }
       
        _navAgent = GetComponent<NavMeshAgent>();
        //GameObject gameObject = GameObject.FindGameObjectWithTag("Axe");
        //_navAgent.SetDestination(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch(stateMachineState)
        {
            case FSMStates.PLANING:
                Planing();
                break;
                case FSMStates.MOVING:
                Move();

                break;
                case FSMStates.PERFORMING:
                if(_performCoroutine ==null)
                {
                    _performCoroutine = Perform ();
                    StartCoroutine(_performCoroutine);
                }
                

                break;
        }
        

        foreach(KeyValuePair<string,int> state in GWorld.Instance.GetState())
        {
            print(state.Key+": "+state.Value);
        }
        foreach(KeyValuePair<string,int> belif in agentBelief)
        {
            print(belif.Key+": "+belif.Value);
        }
    }

    private void Move()
    {
         if(_navAgent != null)
        {
            _navAgent.SetDestination(_currentAction.target.position);

            if(_navAgent.remainingDistance<2 && _navAgent.hasPath && !_navAgent.pathPending)
            {
              
            stateMachineState = FSMStates.PERFORMING;
                return;
            } 
            stateMachineState = FSMStates.MOVING;
                return;
            
        }
    }

    private IEnumerator Perform()
    {
        if(_currentAction.isDone)
        {
            if(planActions.Count > 0)
            {
                _currentAction = planActions.Dequeue();
                _currentAction.agentBeliefInAction = agentBelief;
            }
            else
            {
                planActions = null;
                stateMachineState = FSMStates.PLANING;
                                yield break;
            }
          

            
        }

        if (_currentAction.RequireInRange)
        {
            if (_currentAction.InRange)
            {
                if (_currentAction.Preperform())
                {
                    float time = Time.time;
                   while (time + _currentAction.duration > Time.time)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    if (_currentAction.Postperform())
                    {
                        _currentAction.isDone = true;
                    }
                }
                else
                {
                    _currentAction = planActions.Dequeue();
                }

            }
            else
            {
                stateMachineState = FSMStates.MOVING;
            }
        }
        else
        {

            if (_currentAction.Preperform())
            {
                float time = Time.time;

                if (time + _currentAction.duration > Time.time)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (_currentAction.Postperform())
                {
                    _currentAction.isDone = true;
                }
            }
        }
        _performCoroutine = null;
    }

    private void Planing()
    {
       var orderedGoals = from g in goals orderby g.Value select g;

        foreach(var goal in orderedGoals)
        { 
            if(planActions==null )
        {
                planActions = GPlanner.Plan(avalibleActions, goal.Key, agentBelief);
                if (planActions == null) continue;
                _currentAction= planActions.Dequeue();
                stateMachineState = FSMStates.PERFORMING;
                _performCoroutine= null;
                break;

        }

        }
       
         
    }
}
