using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //sorting like dictionary

public class Node
{
    /*the Planner, which will link all of our Actions together in a plan.
    But rather than link the Actions to other Actions, we're actually just going to create a set of nodes
    that are linked to one another and then they will just hold on (or point back) to a particular Action.
    */
    public Node parent;
    public float cost;  //determining which plan is the cheapest
    public Dictionary<string, int> state;
    public GAction action;  //node pointing to
    public Node(Node parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates); //doing this because we dont actually want all states, but we want a copy
        this.action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GAction> usableActions = new List<GAction>();

        foreach (GAction a in actions)
        {
            //So this will siphon out any actions that CANT be run.
            if (a.isAchieveable())
            {
                usableActions.Add(a);
            }
        }
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("No plan");
            return null; // so return no plan. simply.
        }

        Node cheapest = null;

        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
                if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }
        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;   //essentially like a linked list, which represent our graph
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }
        Debug.Log("The plan is :");
        foreach (GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }
        return queue;

    }
    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> useableActions, Dictionary<string, int> goal)
    {
        /*currentState is going to be filled-up with the states of the world.
        Now the idea with currentState is, as we go through all of our Actions and build up our branch 
        is that we keep track of everything that's being satisfied and all the conditions that are changing as we're going through.
        */
        bool foundPath = false;
        foreach (GAction action in useableActions)
        {
            if (action.isAchieveableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);

                foreach (KeyValuePair<string, int> eff in action.effects)
                {
                    /*So now currentState is going to have not only the state of the world but we're also adding to it the effects
                     * of the current action and then as we move on, the effects from the next action will be added and the next.
                     */
                    if (!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    /*THIS is going to take out of our usableActions, the Action that we've already created a node in our graph for,
                     * so that as we go along the branches, this list of usableActions becomes smaller and smaller, 
                     * which means you can't create a circular path anywhere. So you're not going to end up with some kind 
                     * of endless circle that you're searching through for creating a plan.
                     */
                    List<GAction> subset = ActionSubset(useableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
            return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if(!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
}
