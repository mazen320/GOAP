using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner
{
    /*
     * Start by building the graph, holding things for the action.
     */

    private class Node
    {
        public Node parent;
        public float cost;
        public HashSet<KeyValuePair<string, object>> state;
        public ActionG action;

        public Node(Node parent, float cost, HashSet<KeyValuePair<string, object>> state, ActionG action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = state;
            this.action = action;
        }
    }

    /*
     * We need to plan the sequence of actions now
     */

    public Queue<ActionG> plan(GameObject agent, HashSet<ActionG> availableActions, 
        HashSet<KeyValuePair< string, object>> worldState, 
        HashSet<KeyValuePair<string, object>> goal)
    {
        //here we reset so we can start fresh
        foreach (ActionG action in availableActions)
        {
            action.initReset();
        }

        //check action can run because of their prerequiests
        HashSet<ActionG> usableActions = new HashSet<ActionG>();
        foreach(ActionG action in availableActions)
            usableActions.Add(action);

        /*Now that we stored all the actions we can run
         * we store it in usableActions
         */

        //build the tree
        List<Node> leaves = new List<Node>();

        //now the graph
        Node start = new Node(null, 0, worldState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            //so we have no plan
            Debug.Log("No plan currently");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)   //we have to set the cheapest leaf to the variable cheapest
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<ActionG> result = new List<ActionG>();
        Node node = cheapest;

        while(node != null)
        {
            if(node.action != null)
            {
                result.Insert(0, node.action);  //we insert the action in the front
            }
            node = node.parent;
        }

        Queue<ActionG> queue = new Queue<ActionG>();
        foreach(ActionG action in result)
        {
            queue.Enqueue(action);
        }
        //now we made the queue of our actions
        return queue;
    }
  
}
