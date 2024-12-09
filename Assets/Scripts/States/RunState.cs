using UnityEngine;

public class RunState : AState
{
    public override void EnterState()
    {
        base.EnterState();

        CanChangeState = true;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if(Unit is PlayerControl player)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _parentState.SetState(player.MoveState.WalkState);
                player.State = EState.Walk;
            }
        }
    }
}
