using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;      //gonna use this for sorting

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r) //string for key value, i for value, r for remove. string going to be name of the goal, and the value(i) is going to be how important it is
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
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    bool invoked = false;

    public void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    void LateUpdate()   //making it late cause of it getting actions from the planner then we run this.
    {
        if(currentAction != null && currentAction.running)
        {
            //navmesh code
            //in this case, less than a distance of 1, then we're going to run the end of our action. basically completion
            if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                if(!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        //pretty much the planner
        if(planner == null || actionQueue == null)
        {
            planner = new GPlanner();   //if the planner is null we create a new planner

            /*That's going to take our 'goals' array and order it and stick it into 'sortedGoals'.
             * we're going to sort through all of our Goals and put them in order, from most
             * important to least important, so that we can then loop through them one at a time until we find one that 
             * we can do.
             */
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;  //sorted dictionary
            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, null);

                if(actionQueue != null) //if its not null we must follow the plan
                {
                    currentGoal = sg.Key;   //we're going to set the 'currentGoal' that we're working on to equal the sub-goal:
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            //so if the Goal that we've just been working on is one of those removable ones
            //and then we're going to set 'planner = null' which is going to trigger getting another Planner

            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            // this will take the action that's sitting at the top of our queue OFF
            // the queue and put it into currentAction.
            currentAction = actionQueue.Dequeue();
            if(currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);    //starting point for setting the destination
                }
            }
            else
            {
                actionQueue = null; //and that will force us to get a new plan
            }
        }
    }
}
