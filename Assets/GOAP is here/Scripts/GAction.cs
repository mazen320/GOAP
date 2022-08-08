using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;   //planner will find the cheapest cost, if they are achieveable
    public GameObject target;   //target is the location of the action thats gonna take place
    public GameObject targetTag;    //so we can pick up gameObjects using tag, if they exist in the hierachy incase there isnt a gameObject target
    public float duration = 0f; //how long will the agent stand in the particular waypoint, virtually performing the action
    public WorldState[] preConditions;
    public WorldState[] afterEffects;   //these 2 are what the planner is going to match on either side of the action
    public NavMeshAgent agent;

    /*When we want to plan them in our it makes a lot more sense for them to be in a dictionary
     * it makes it way easier to work with them
     
     *What we are doing is we are going to use the preconditions and afterEffects in the worldState to get them into the inspector
     *and we are going to put them into these 2 dictionaries.
     */
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;    //for the state of our actual agent itself, it's got an internal set of states

    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (preConditions != null)
        {
            foreach(WorldState worldState in preConditions)
            {
                preconditions.Add(worldState.key, worldState.value);
            }
        }
        if(afterEffects != null)
        {
            foreach (WorldState worldState in afterEffects)
            {
                effects.Add(worldState.key, worldState.value);
            }
        }
    }

    public bool isAchieveable()
    {
        return true;
    }
    public bool isAchieveableGiven(Dictionary<string, int> conditions)
    {
        /*We're looping through our preconditions that we've got stored for this particular action
         * and we're looking for each of these condiutions to make sure it actually does match them
         */
        foreach(KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;    //if we matched all the precondtions
    }

    /*So we are going to inherit from this particular class, and we want to make sure that we force
     * a preperformance and a post performance method that will allow me to put in customized code for each action
     * so that i can do other checks before an action actually starts
     */
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
