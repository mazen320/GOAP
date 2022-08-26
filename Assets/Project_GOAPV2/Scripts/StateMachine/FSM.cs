using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    /*We are using stacks, which is an Abstract Data Type
     * It's "Last in First Out"
     * 
     * States should push other states onto the stack 
     * and pop themselves off.
     */
    private Stack<FState> statesStack = new Stack<FState>();

    /*
     * Delegate is a container for a function
     * it can be passed around, and be used like a variable
     * delegates can have values assigned to them
     * while variables contain data, deleates contain functions
     */
    public delegate void FState(FSM fsm, GameObject gameObject);

    public void Update(GameObject gameObject)
    {
        /*
         * Getting the top item without removing
         */
        if (statesStack.Peek() != null)
        {
            statesStack.Peek().Invoke(this, gameObject);
        }
    }

   public void pushState(FState state)
    {
        /*
         * Add item to the top
         */
        statesStack.Push(state);
    }

    public void popState()
    {
        /*
         * Remove the top item
         */
        statesStack.Pop();
    }
}
