using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState
{
    public string key;  //doing this so we can edit these items in the inspector
    public int value;   
}

public class WorldStates
{
    public Dictionary<string, int> states; // this is going to hold all the states of the world
    
    public WorldStates()
    {
        states = new Dictionary<string, int>(); //constructor for the states
    }

    public bool HasState(string key)
    { return states.ContainsKey(key); }

    /* We are using Dictionary because it is very easy to use for what we are creating
     * and it is very easy to manipulate and add stuff to
     * So we can look for things without writing a lot more code.
     */
    void AddState(string key, int value)   //to add a state if there isnt one
        { states.Add(key, value); }

    public void ModifyState(string key, int value)
    {
        /*What I want to do is increase it's value
         * 
         */
        if(states.ContainsKey(key))
        {
            states[key] += value;
            if(states[key] <= 0)
            {
                RemoveState(key);
            }  
            else
            {
                states.Add(key, value); //so if it doesn't exist, we are adding it, rather than incrementing it
            }
        }
    }
    public void RemoveState(string key)
    {
        if(states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    /*This will set a set state, instead of modifying it. and set its value directly
     */
    public void SetState(string key, int value)
    {
        if(states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }
    /*This will return all of the states/dictonary
     * Planner will be using this when its trying to get a hold of all the states in the world & the belief.
     */
    public Dictionary<string, int> GetStates()
    {
        return states;
    }
}
