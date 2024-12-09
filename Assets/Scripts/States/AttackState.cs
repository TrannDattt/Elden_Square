using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AState
{
    [SerializeField] private List<AnimationClip> _attackClips;

    private int _clipIndex;
    private bool _canChangeClip;

    public override void Init(UnitControl unit)
    {
        base.Init(unit);

        _clipIndex = 0;
    }

    public override void EnterState()
    {
        _clip = _attackClips[_clipIndex];

        base.EnterState();

        _canChangeClip = false;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (TimePlayed > ClipLength)
        {
            _canChangeClip = true;
        }

        if (TimePlayed > ClipLength + .3f)
        {
            CanChangeState = true;
        }
    }

    private void ChoseClipIndex()
    {
        _clipIndex = (_clipIndex + 1) % _attackClips.Count;
    }

    public override void ExitState()
    {
        base.ExitState();

        _clipIndex = 0;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (CanChangeState)
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
        else if (_canChangeClip)
        {
            if (Unit is PlayerControl player)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ChoseClipIndex();
                    player.SetState(player.LightAttackState, true);
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    ChoseClipIndex();
                    player.SetState(player.HeavyAttackState, true);
                }
            }
        }
    }
}
