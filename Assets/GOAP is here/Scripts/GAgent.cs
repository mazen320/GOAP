using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;      //gonna use this for sorting

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r) //string for key value, i for value, r for remove
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction>actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GPlanner planner;
    Queue<GAction> Actionqueue;
    public GAction currentAction;
    SubGoal currentGoal;
    
    
    void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    void LateUpdate()   //making it late cause of it getting actions from the planner then we run this.
    {
        
    }
}
