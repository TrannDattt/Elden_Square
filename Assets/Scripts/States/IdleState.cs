using UnityEngine;

public class IdleState : AState
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
            if (Unit.MoveDir.magnitude > 0)
            {
                player.SetState(player.MoveState);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                player.SetState(player.DodgeState);
                player.State = EState.Dodge;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.SetState(player.LightAttackState);
                player.State = EState.Attack;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                player.SetState(player.HeavyAttackState);
                player.State = EState.Attack;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                player.SetState(player.BlockState);
                player.State = EState.Block;
            }
        }
    }
}
