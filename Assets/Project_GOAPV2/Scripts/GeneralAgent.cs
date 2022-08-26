using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 *A general Farmer class so in future there can be multiple farmers
 */
public abstract class GeneralAgent : MonoBehaviour, IGoap
{
    public float moveSpeed = 1;
    public AgentInventory inventory;


    void Start()
    {
        if(inventory == null)
            inventory = gameObject.AddComponent<AgentInventory>();
    }

    /**
	 * data that will go to the actions and system while planning.
	 */
    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasEnoughMoney", (inventory.money >= 5)));
        worldData.Add(new KeyValuePair<string, object>("ownsMoney", (inventory.hasMoney == true)));

        Food[] foods = (Food[])GameObject.FindObjectsOfType(typeof(Food));
        worldData.Add(new KeyValuePair<string, object>("hasFood", (foods.Length > 0)));
        return worldData;
    }

    /**
	 * these we implement in subclasses
	 */
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();


    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        //we are going to make them all succeed
    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<ActionG> actions)
    {
        // we found a plan
        Debug.Log("Plan found");
    }

    public void actionsCompleted()
    {
        // Everything is completed
        Debug.Log("Actions completed");
    }

    public void planHalt(ActionG halt)
    {
        // An action halted out of the plan.
        //reset plan
        Debug.Log("Plan Aborted ");
    }

    public bool move(ActionG nextAction)
    {
        // move towards the NextAction's target
        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

        if (gameObject.transform.position.Equals(nextAction.target.transform.position))
        {
            nextAction.setIsInRange(true);
            return true;
        }
        else
            return false;
    }
}

