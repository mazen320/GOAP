using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FState
{
    void Update(FSM fsm, GameObject gameObject);
}
