using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction2 : ActionG
{
    private bool eaten = false;
    private Food targetFood;

    private float startTime = 0f;
    private float workDuration = 5f;

    public TestAction2()
    {
        addEffect("hasMoney", true);
    }

    public override void reset()
    {
        eaten = false;
        targetFood = null;
        startTime = 0;
    }

    public override bool Achieved()
    {
        return eaten;
        Debug.Log("Achieved");
    }

    public override bool inRangeCheck()
    {
        return true;
    }


    public override bool PrePerform(GameObject agent)
    {
        Food[] foods = (Food[])GameObject.FindObjectsOfType(typeof(Food));
        if(foods.Length == 0)
        {
            return false;
        }
        targetFood = foods[0];
        target = targetFood.gameObject;
        //GameObject food = GameObject.FindGameObjectWithTag("Food");

        //target = food;
        return foods[0] != null;
    }

    /*
     * Should always succed as its just a check.
     * */
    public override bool perform(GameObject agent)
    {
        AgentInventory inventory = agent.GetComponent<AgentInventory>();
        inventory.eaten = false;
        inventory.hasFood = false;
        eaten = true;
        return true;
    }

}

