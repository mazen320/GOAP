using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction1 : ActionG
{
    private bool bought = false;

    private float startTime = 0f;
    private float workDuration = 5f;

    public TestAction1()
    {
        addPrerequisites("freeMoney", true); //must have enough money
        addEffect("hasMoney", true);
    }

    public override void reset()
    {
        bought = false;
        startTime = 0;
    }

    public override bool Achieved()
    {
        return bought;
        Debug.Log("Achieved");
    }

    public override bool inRangeCheck()
    {
        return false;
    }


    public override bool PrePerform(GameObject agent)
    {
        
        target = GameObject.FindGameObjectWithTag("Food");
        return true;
    }

    /*
     * Should always succed as its just a check.
     * */
    public override bool perform(GameObject agent)
    {
        AgentInventory inventory = agent.GetComponent<AgentInventory>();
        inventory.hasMoney = true;
        Destroy(target);
        bought = true;
        return true;
    }

}

