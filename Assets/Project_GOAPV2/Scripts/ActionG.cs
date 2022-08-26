using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionG : MonoBehaviour
{
    HashSet<KeyValuePair<string, object>> prerequisites;
    HashSet<KeyValuePair<string, object>> afterEffects;

    public GameObject target;   //target can be null sometimes, depends on the action
    bool isInRange = false;

    /*cost is basically the weight of the action
     *Changing this can affect the way it plans things
     */
    public float ActionCost = 1f;

    public bool actionFinished = false;


    public ActionG()
    {
        prerequisites = new HashSet<KeyValuePair<string, object>>();
        afterEffects = new HashSet<KeyValuePair<string, object>>();
    }

    public void initReset()
    {
        reset();
        isInRange = false;
        target = null;
    }

    public abstract bool Achieved();
    /*
     * Checking if the action is done
     */
    public abstract bool succeeded(GameObject agent);
    /*
     * Will return true if it succeeded the action
     * if not the action queue will clear out
     */
    public abstract bool PrePerform(GameObject agent);
    /*
     * This will be checking the prerequisites, if the action can run.
     * this wont be used by all actions
     */
    public abstract bool inRangeCheck();
    /*
     * Checks if is in range, if not it will get to it.
     */
    public abstract void reset();
    /*
     * Resets vairables that need to be reset, so the planning happens again
     */

    public void setIsInRange(bool isInRange)
    {
        /*
         * Passing the bool outcome to this void
         */
        this.isInRange = isInRange;
    }

    public void addPrerequisites(string key, object value)
    {
        prerequisites.Add(new KeyValuePair<string, object>(key, value));
    }

    public void removePrerequisites(string key)
    {
        //will set to default prerequisites
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> keyValuePair in prerequisites)
        {
            if (keyValuePair.Equals(key))
            {
                remove = keyValuePair;
            }
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            prerequisites.Remove(remove);
        }
    }
    public void addEffect(string key, object value)
    {
        afterEffects.Add(new KeyValuePair<string, object>());
    }
    public void removeEffects(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> keyValuePair in afterEffects)
        {
            if (keyValuePair.Equals(key))
            {
                remove = keyValuePair;
            }
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            afterEffects.Remove(remove);
        }
    }

    public HashSet<KeyValuePair<string, object>> Prerequisites
    {
        get { return prerequisites; }
    }
    public HashSet<KeyValuePair<string, object>> Aftereffects
    {
        get { return afterEffects; }
    }
}
