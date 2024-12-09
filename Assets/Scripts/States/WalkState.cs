using UnityEngine;

public class WalkState : AState
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _parentState.SetState(player.MoveState.RunState);
                player.State = EState.Run;
            }
        }
    }
}
