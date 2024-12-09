using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : UnitControl
{
    [field: SerializeField] public IdleState IdleState { get; private set; }
    [field: SerializeField] public MoveState MoveState { get; private set; }
    [field: SerializeField] public DodgeState DodgeState { get; private set; }
    [field: SerializeField] public AttackState LightAttackState { get; private set; }
    [field: SerializeField] public AttackState HeavyAttackState { get; private set; }
    [field: SerializeField] public BlockState BlockState { get; private set; }

    [SerializeField] protected InputAction _inputAction;

    private void Awake()
    {
        SetUp();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CurState.UpdateState();

        GetInput();
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (State == EState.Walk || State == EState.Run)
        { 
            UpdateBodyRotation(); 
        }

        CurState.FixedUpdateState();

        OnStateUpdating();
    }

    public override void SetUp()
    {
        base.SetUp();

        _inputAction.Enable();
        SetState(IdleState);
    }

    private void GetInput()
    {
        var vector3Input = _inputAction.ReadValue<Vector3>();

        if (State != EState.Dodge)
        { 
            MoveDir = new Vector3(vector3Input.x, 0, vector3Input.z); 
        }
    }

    private void HandleInput()
    {
        CurState.HandleInput();
    }

    private void UpdateBodyRotation()
    {
        var lookDir = Vector3.RotateTowards(transform.forward, MoveDir.normalized, UnitStats.MaxRotateSpeed, 0f);
        _rortateDir = Quaternion.LookRotation(lookDir);
        UnitCore.Rigidbody.MoveRotation(_rortateDir);
    }

    private void OnStateUpdating()
    {
        switch (State)
        {
            case EState.Idle:
                OnIdling();
                break;

            case EState.Walk:
                OnWalking();
                break;

            case EState.Run:
                OnRunning();
                break;

            case EState.Dodge:
                OnDodging();
                break;

            case EState.Attack:
                OnAttacking();
                break;

            case EState.Block:
                OnBlocking();
                break;

            default:
                break;
        }
    }

    private void OnIdling()
    {
        //UnitCore.Rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnWalking()
    {
        UnitCore.Rigidbody.MovePosition(UnitCore.Rigidbody.position + UnitStats.MaxMoveSpeed * Time.fixedDeltaTime * MoveDir.normalized);
    }

    private void OnRunning()
    {
        UnitCore.Rigidbody.MovePosition(UnitCore.Rigidbody.position + UnitStats.MaxMoveSpeed * UnitStats.RunSpeedMultiple * Time.fixedDeltaTime * MoveDir.normalized);
    }

    private void OnDodging()
    {
        UnitCore.Rigidbody.MovePosition(UnitCore.Rigidbody.position + UnitStats.MaxMoveSpeed * .75f * Time.fixedDeltaTime * transform.forward);
    }

    private void OnAttacking()
    {
        if(!LightAttackState.CanChangeState || !HeavyAttackState.CanChangeState)
        { 
            UnitCore.Rigidbody.angularVelocity = Vector3.zero; 
        }
    }

    private void OnBlocking()
    {
        if (!BlockState.CanChangeState)
        {
            UnitCore.Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
