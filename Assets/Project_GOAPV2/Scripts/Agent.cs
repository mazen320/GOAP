using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : GeneralAgent
{
    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("eat", true));
        return goal;
    }
}
