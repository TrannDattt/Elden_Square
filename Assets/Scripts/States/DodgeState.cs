using System.Collections;
using UnityEngine;

public class DodgeState : AState
{
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if(TimePlayed > ClipLength)
        {
            CanChangeState = true;
        }
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if(CanChangeState)
        {
            if (Unit is PlayerControl player)
            {    
                if (Unit.MoveDir.magnitude > 0)
                {
                    player.SetState(player.MoveState);
                }
                else
                {
                    player.SetState(player.IdleState);
                    player.State = EState.Idle;
                }
            }
        }
    }
}
