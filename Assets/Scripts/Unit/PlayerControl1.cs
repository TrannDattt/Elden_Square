using System.Collections;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl2 : UnitControl
{
    [SerializeField] protected InputAction _inputAction;

    private bool _isWalking;

    private const string _inputX = "InputX";
    private const string _inputZ = "InputZ";
    private const string _stay = "Stay";
    private const string _walk = "Walk";
    private const string _dodge = "Dodge";
    private const string _attack = "Attack";

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
        HandleInput();
    }

    private void FixedUpdate()
    {
        
    }

    public override void SetUp()
    {
        base.SetUp();

        _inputAction.Enable();
    }

    private void HandleInput()
    {
        var animator = UnitCore.Animator;

        var vector3Input = _inputAction.ReadValue<Vector3>();
        var vectorXZ = new Vector3(vector3Input.x, 0, vector3Input.z);
        animator.SetFloat(_inputX, vectorXZ.x);
        animator.SetFloat(_inputZ, vectorXZ.z);

        // Idle & Basic Move
        if (!Mathf.Approximately(vectorXZ.magnitude, 0))
        {
            if(State == EState.Idle)
            {
                State = EState.Move;
            }
        }
        else
        {
            if (State == EState.Move)
            {
                State = EState.Idle;
            }
        }

        if (State == EState.Move)
        {
            MoveDir = vectorXZ;
        }

        if( State == EState.Idle || State == EState.Attack)
        {
            MoveDir = Vector3.zero;
        }

        var _isStaying = Mathf.Approximately(MoveDir.magnitude, 0f);
        animator.SetBool(_stay, _isStaying);

        _isWalking = !Input.GetKey(KeyCode.LeftShift);
        animator.SetBool(_walk, _isWalking);

        // Dodge
        if (Input.GetKeyDown(KeyCode.F) && State != EState.Dodge)
        {
            animator.SetTrigger(_dodge);
            State = EState.Dodge;
            StartCoroutine(ChangeStateToIdle(1f));
        }

        // Attack
        if (Input.GetMouseButtonDown(0) && State != EState.Attack)
        {
            animator.SetTrigger(_attack);
            State = EState.Attack;
            StartCoroutine(ChangeStateToIdle(.8f));
        }

    }

    private IEnumerator ChangeStateToIdle(float time)
    {
        yield return new WaitForSeconds(time);
        State = EState.Idle;
    }

    private void OnAnimatorMove()
    {
        OnBodyMoving();
        OnBodyRotating();
    }

    private void OnBodyRotating()
    {
        var lookDir = Vector3.RotateTowards(transform.forward, MoveDir.normalized, UnitStats.MaxRotateSpeed, 0f);
        _rortateDir = Quaternion.LookRotation(lookDir);
        UnitCore.Rigidbody.MoveRotation(_rortateDir);
    }

    private void OnBodyMoving()
    {
        var moveSpeed = _isWalking ? UnitStats.MaxMoveSpeed : UnitStats.RunSpeedMultiple * UnitStats.MaxMoveSpeed;
        UnitCore.Rigidbody.MovePosition(UnitCore.Rigidbody.position + moveSpeed * Time.fixedDeltaTime * MoveDir.normalized);
    }
}
