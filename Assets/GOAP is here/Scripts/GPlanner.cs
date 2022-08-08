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
}
