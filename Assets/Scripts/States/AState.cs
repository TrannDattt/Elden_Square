using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;
using static System.TimeZoneInfo;

public interface IState
{
    public abstract void Init(UnitControl unit);
    public abstract void HandleInput();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void SetState(AState state, bool doReset = false);
}

public abstract class AState : MonoBehaviour, IState
{
    [SerializeField] protected AnimationClip _clip;

    public UnitControl Unit { get; private set; }
    public bool CanChangeState { get; protected set; }

    protected Animator Animator => Unit.UnitCore.Animator;
    protected float TimePlayed => Time.time - _startTime;
    protected float ClipLength => _clip != null ? _clip.length / Animator.speed : 0;
    protected AState CurChildState => _subStateMachine.State;
    protected AState _parentState;
    protected StateMachine _subStateMachine;

    private float _startTime;

    public virtual void Init(UnitControl unit)
    {
        Unit = unit;
        _subStateMachine = new();
        CanChangeState = true;
    }

    public void SetParent(AState parent)
    {
        _parentState = parent;
    }

    public virtual void HandleInput() 
    {
        CurChildState?.HandleInput();
    }

    public virtual void EnterState() 
    {
        CanChangeState = false;
        _startTime = Time.time;
        if(_clip != null)
        {
            Animator.CrossFade(_clip.name, .2f);
            //Animator.Play(_clip.name);
        }
    }

    public virtual void UpdateState() 
    {
        CurChildState?.UpdateState();
    }

    public virtual void FixedUpdateState() 
    { 
        CurChildState?.FixedUpdateState(); Debug.Log(CurChildState + ": " + ClipLength);
    }

    public virtual void ExitState() 
    { 
        SetState(null);
    }

    public virtual void SetState(AState state, bool doReset = false)
    {
        _subStateMachine.SetState(state, doReset);
    }
}

//public class StateBlendTree : AnimatorStateMachine
//{

//}