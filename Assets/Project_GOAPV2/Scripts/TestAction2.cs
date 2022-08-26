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
        addPrerequisites("hasEnoughMoney", true);
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
        return true;
    }

    /*
     * Should always succed as its just a check.
     * */
    public override bool perform(GameObject agent)
    {
        AgentInventory inventory = agent.GetComponent<AgentInventory>();
        inventory.hasMoney = true;
        return true;
    }

}

