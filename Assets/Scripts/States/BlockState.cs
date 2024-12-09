using UnityEngine;

public class BlockState : AState
{
    public override void EnterState()
    {
        base.EnterState();

        CanChangeState = true;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        Animator.Play(_clip.name, 0, TimePlayed < ClipLength ? TimePlayed : 1f);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Unit is PlayerControl player)
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                player.SetState(player.IdleState);
                player.State = EState.Idle;
            }
        }
    }
}
