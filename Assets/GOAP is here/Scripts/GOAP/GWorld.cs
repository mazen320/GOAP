using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{ 
    /*this will help when we start using queues, we are using sealed so dont get any conflicts when we are trying to access any particular part of the class
     * which tbh should be accessed one thing at a time
     */

    private static readonly GWorld instance = new GWorld();
    /*Making this a singleton so we can access it everywhere
     * and we ONLY want ONE 1 version of the game world environment states
     * we dont want to have copies of it with different values
     */
    private static WorldStates world;    //dictionary holding all the states

    static GWorld() //our constructor
    {
        world = new WorldStates();
    }

    private GWorld()
    {

    }
    public static GWorld Instance
    {
        get { return instance; }
    }
    public WorldStates GetWorld()    // so we can return the status of the world
    {
        return world;
    }
}
