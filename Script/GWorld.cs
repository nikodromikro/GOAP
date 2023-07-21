using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWorld : MonoBehaviour
{
    static GWorld instance;
    Dictionary<string,int> wolrdStates = new Dictionary<string,int>();











    public static GWorld Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GWorld>();
                if(instance == null)
                {
                    GameObject gameObject = new GameObject("GWorld");
                    instance = gameObject.AddComponent<GWorld>();
                }
            }
            return instance;
        }
    }


    public Dictionary<string,int> GetState() { return wolrdStates; }

    public void SetState(string key , int value)
    {
        if(wolrdStates.ContainsKey(key))
        {
            wolrdStates[key] = value;
        }
        else
        {
            wolrdStates.Add(key, value); 
        }
    }


    public void ModifyState(string key , int value)
    {
        if (wolrdStates.ContainsKey(key))
        {
            wolrdStates[key] += value;
        }
        else
        {
            wolrdStates.Add(key, value);
        }
    }



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }











}
