using UnityEngine;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    public States currentState;

    private void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        States nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    private void SwitchToNextState(States nextState)
    {
        currentState = nextState;
    }
}
