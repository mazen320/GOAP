using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentG : MonoBehaviour
{
    //We are going to use the state machines for this

    FSM fsm;

    FSM.FState idle;
    FSM.FState moveTo;
    FSM.FState runAction;

    HashSet<ActionG> availableActions;
    Queue<ActionG> currentActions;

    IGoap data; //implementing class, that gives our world data.

    Planner planner;



    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM();
        availableActions = new HashSet<ActionG> ();
        currentActions = new Queue<ActionG> ();
        planner = new Planner ();
        findData();
        createIdleState();
        createMoveToState();
        createRunActionState();
        loadActions();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update(this.gameObject);
    }

    public void addAction(ActionG action)
    {
        availableActions.Add (action);
    }
    public void removeAction(ActionG action)
    {
        availableActions.Remove(action);
    }
    private bool hasPlan()
    {
        return currentActions.Count > 0;
    }
    public ActionG GetAction(Type action)
    {
        foreach (ActionG actionG in availableActions)
        {
            if (actionG.GetType().Equals(action))
            {
                return actionG;
            }
        }
        return null;
    }

    void createIdleState()
    {
        //the lambda operator => separates the input parameters on the left side from the lambda body on the right side
        idle = (fsm, gameObj) =>
        {
            //the goap planning
            //we wanna get the world state, and goal we want to plan
            HashSet<KeyValuePair<string, object>> worldState = data.GetWorldState();
            HashSet<KeyValuePair<string, object>> goal = data.createGoalState();

            //we plan here
            Queue<ActionG> plan = planner.plan(gameObject, availableActions, worldState, goal);

            if (plan != null)
            {
                currentActions = plan;
                data.planFound(goal, plan);

                fsm.popState(); //we are moving this to run action state
                fsm.pushState(runAction);
            }
            else
            {
                //we couldnt get a plan
                Debug.Log("Failed Plan");
                data.planFailed(goal);
                fsm.popState(); //we will go back to idle state
                fsm.pushState(idle);
            }
        };
    }
    void createMoveToState()
    {
        moveTo = (fsm, gameObj) => { 
            //we move to game object

            ActionG action = currentActions.Peek();
            if (action.inRangeCheck() && action.target == null)
            {
                Debug.Log("Error: Add a target");
                fsm.popState(); // here we move
                fsm.popState(); // then it perform
                fsm.pushState(idle);
                return;
            }

            // here this will let the agent move itself
            if (data.move(action))
            {
                fsm.popState();
            }

        };
    }
    private void createRunActionState()
    {

        runAction = (fsm, gameObj) => {
            // perform the action

            if (!hasPlan())
            {
                // no actions to perform
                Debug.Log("Done ");
                fsm.popState();
                fsm.pushState(idle);
                data.actionsCompleted();
                return;
            }

            ActionG action = currentActions.Peek();
            if (action.Achieved())
            {
                // the action is done. Remove it so we can perform the next one
                currentActions.Dequeue();
            }

            if (hasPlan())
            {
                // perform the next action
                action = currentActions.Peek();
                bool inRange = action.inRangeCheck() ? action.isInRange() : true;

                if (inRange)
                {
                    // if we are in range, so perform the action
                    bool success = action.perform(gameObj);

                    if (!success)
                    {
                        // then if action fail, we plan again
                        fsm.popState();
                        fsm.pushState(idle);
                        data.planHalt(action);
                    }
                }
                else
                {
                    //we have to get there first
                    // push moveTo state
                    fsm.pushState(moveTo);
                }

            }
            else
            {
                // no actions available, go back to plan state
                fsm.popState();
                fsm.pushState(idle);
                data.actionsCompleted();
            }

        };
    }


    private void findData()
    {
        foreach (Component component in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(component.GetType()))
            {
                data = (IGoap)component;
                return;
            }
        }
    }
    private void loadActions()
    {
        ActionG[] actions = gameObject.GetComponents<ActionG>();
        foreach(ActionG action in actions)
        {
            availableActions.Add(action);
        }
        Debug.Log("Found actions");
    }

}
