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
        HashSet<KeyValuePair<string, object>> worldState,
        HashSet<KeyValuePair<string, object>> goal)
    {
        //here we reset so we can start fresh
        foreach (ActionG action in availableActions)
        {
            action.initReset();
        }

        //check action can run because of their prerequiests
        HashSet<ActionG> usableActions = new HashSet<ActionG>();
        foreach (ActionG action in availableActions)
            if(action.PrePerform(agent))
            usableActions.Add(action);

        /*Now that we stored all the actions we can run
         * we store it in usableActions
         */

        //build the tree
        List<Node> leaves = new List<Node>();

        //now the graph
        Node start = new Node(null, 0, worldState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            //so we have no plan
            Debug.Log("No plan currently");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)   //we have to set the cheapest leaf to the variable cheapest
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<ActionG> result = new List<ActionG>();
        Node node = cheapest;

        while (node != null)
        {
            if (node.action != null)
            {
                result.Insert(0, node.action);  //we insert the action in the front
            }
            node = node.parent;
        }

        Queue<ActionG> queue = new Queue<ActionG>();
        foreach (ActionG action in result)
        {
            queue.Enqueue(action);
        }
        //now we made the queue of our actions
        return queue;
    }

    //build graph
    //possible paths are stored in leaves list
    //lowest cost will be the sequence we will take
    private bool buildGraph(Node parent, List<Node> leaves, HashSet<ActionG> usableAction, HashSet<KeyValuePair<string, object>> goal)
    {
        bool foundPath = false;

        //going to go through our actions avaiable
        //see if we can use it here
        foreach (ActionG action in usableAction)
        {
            if (stateCheck(action.Prerequisites, parent.state))
            {
                //apply action effect to parent state
                HashSet<KeyValuePair<string, object>> currentState = populateState(parent.state, action.Aftereffects);

                Node node = new Node(parent, parent.cost + action.ActionCost, currentState, action);

                if(stateCheck(goal, currentState))
                {
                    //we found the goal
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    HashSet<ActionG> subset = actionSubset(usableAction, action);
                    bool found = buildGraph(node, leaves, subset, goal);

                    if(found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    /*
     * Now we create a subset of actions, except removeme action
     * It will create a new set
     */
    private HashSet<ActionG> actionSubset(HashSet<ActionG> actions, ActionG removeMe)
    {
        HashSet<ActionG> subset = new HashSet<ActionG>();
        foreach(ActionG action in actions)
        {
            if(!action.Equals(removeMe))
            {
                subset.Add(action);
            }
        }
        return subset;
    }
    /*
     * Check if the items are in 'state', and if they match
     */
    private bool stateCheck(HashSet<KeyValuePair<string, object>> checker, HashSet<KeyValuePair<string, object>> state)
    {
        bool Allmatch = true; //check if they all match
        foreach (KeyValuePair<string, object> check in checker)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(check.Key))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
                Allmatch = false;
        }
        return Allmatch;
    }

    /**
    * Apply the changeState to the currentState
    */
    private HashSet<KeyValuePair<string, object>> populateState(HashSet<KeyValuePair<string, object>> currentState, 
        HashSet<KeyValuePair<string, object>> changeState)
    {
        HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();
        // We make a copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach (KeyValuePair<string, object> change in changeState)
        {
            // we check if the key exists, and if it doesnt we update the value(currentstate)
            bool exists = false;

            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(change))
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                state.RemoveWhere((KeyValuePair<string, object> keyValuePair) => { return keyValuePair.Key.Equals(change.Key); });
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }
            // if it doesn't exist, add it(currentstate)
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return state;
    }
}

