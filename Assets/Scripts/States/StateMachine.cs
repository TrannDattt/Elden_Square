using UnityEngine;

public class StateMachine
{
    public AState State {  get; private set; }

    public void SetState(AState newState, bool doReset)
    {
        if (newState != State || doReset)
        {
            if(!doReset)
            { 
                State?.ExitState(); 
            }
            State = newState;
            State?.EnterState();
        }
    }
}
