using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoap
{
    /*Interface will world data, for the agent.
     * It will be used in planning
     * 
     * Agents must implement interface
     * This will give feeback to the agent
     */

    /*
     * Start by getting the starting state of the agent and the world
     * will know what action are needed for actions to run
     */
    HashSet<KeyValuePair<string, object>> GetWorldState();

    /*
     * Getting the a new goal so the planner can figure actions needed
     */
    HashSet<KeyValuePair<string, object>> createGoalState();
    /*
     * Gets a new goal so the planner can figure out, and the actions to do it
     */


    bool move(ActionG nextAction);
    /*
     * Moves agent towards target to complete next action 
     */

    void actionsCompleted();
    /*
     * Actions finished and reached goal
     */

    void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<ActionG> actions);
    /*
    * We found the plan, will queue the actions for the agent 
    */

    void planHalt(ActionG haltAction);
    /*
     * action can the plan to halt
     * return
     */
    void planFailed(HashSet<KeyValuePair<string, object>> FailedGoal);
    /*
     * No actions can be found to get to the goal
     */
}
