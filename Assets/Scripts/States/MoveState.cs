using UnityEngine;

public class MoveState : AState
{
    [field: SerializeField] public WalkState WalkState {  get; private set; }
    [field: SerializeField] public RunState RunState { get; private set; }

    public override void Init(UnitControl unit)
    {
        base.Init(unit);

        WalkState.SetParent(this);
        RunState.SetParent(this);
    }

    public override void EnterState()
    {
        base.EnterState();

        SetState(WalkState);
        Unit.State = EState.Walk;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        CanChangeState = WalkState.CanChangeState && RunState.CanChangeState;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Unit is PlayerControl player)
        {
            if(Mathf.Approximately(Unit.MoveDir.magnitude, 0))
            {
                player.SetState(player.IdleState);
                player.State = EState.Idle;
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
