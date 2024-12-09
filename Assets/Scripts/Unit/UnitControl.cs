using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UnitControl : MonoBehaviour
{
    [field: SerializeField] public UnitCore UnitCore { get; protected set; }
    [field: SerializeField] public EUnitClass UnitClass { get; protected set; }

    [SerializeField] private UnitBaseStats _unitBaseStats;

    public UnitIngameStats UnitStats { get; protected set; }
    public Vector3 MoveDir {  get; protected set; }
    public bool IsRunning {  get; protected set; }
    public bool IsAttacking { get; protected set; }

    public EState State {  get; set; }

    protected AState CurState => _stateMachine.State;
    protected StateMachine _stateMachine;
    protected Quaternion _rortateDir = Quaternion.identity;

    public virtual void SetUp()
    {
        UnitStats = new(_unitBaseStats);
        _stateMachine = new();
        var stateList = GetComponentsInChildren<AState>();

        foreach(var state in stateList)
        {
            state.Init(this);
        }
    }

    public void SetState(AState state, bool doReset = false)
    {
        _stateMachine.SetState(state, doReset);
    }
}

public enum EUnitClass
{
    Player_Barbarian,
    Player_Knight,
    Player_Mage,
    Player_Rouge,
}

public class UnitIngameStats
{
    public float MaxHealth { get; private set; }
    public float MaxMoveSpeed { get; private set; }
    public float RunSpeedMultiple { get; private set; }
    public float MaxRotateSpeed { get; private set; }
    public float MaxDamage { get; private set; }

    public UnitIngameStats(UnitBaseStats unitBaseStats)
    {
        MaxHealth = unitBaseStats.BaseHealth;
        MaxMoveSpeed = unitBaseStats.BaseMoveSpeed;
        RunSpeedMultiple = unitBaseStats.BaseRunSpeedMultiple;
        MaxRotateSpeed = unitBaseStats.BaseRotateSpeed;
        MaxDamage = unitBaseStats.BaseDamage;
    }

    public void ChangeStats(float deltaHealth = 0, float deltaMoveSpeed = 0, float deltaRunSpeed = 0, float deltaRotateSpeed = 0, float deltaDamage = 0)
    {
        MaxHealth = Mathf.Clamp(MaxHealth + deltaHealth, 0, MaxHealth + deltaHealth);
        MaxMoveSpeed = Mathf.Clamp(MaxMoveSpeed + deltaMoveSpeed, 0, MaxMoveSpeed + deltaMoveSpeed);
        RunSpeedMultiple = Mathf.Clamp(RunSpeedMultiple + deltaRunSpeed, 0, RunSpeedMultiple + deltaRunSpeed);
        MaxRotateSpeed = Mathf.Clamp(MaxRotateSpeed + deltaRotateSpeed, 0, MaxRotateSpeed + deltaRotateSpeed);
        MaxDamage = Mathf.Clamp(MaxDamage + deltaDamage, 0, MaxDamage + deltaDamage);
    }
}

[CreateAssetMenu(menuName = "Unit Class")]
public class UnitBaseStats : ScriptableObject
{
    [field: SerializeField] public float BaseHealth { get; private set; }
    [field: SerializeField] public float BaseMoveSpeed { get; private set; }
    [field: SerializeField] public float BaseRunSpeedMultiple { get; private set; }
    [field: SerializeField] public float BaseRotateSpeed { get; private set; }
    [field: SerializeField] public float BaseDamage { get; private set; }
}

[System.Serializable]
public class UnitCore
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
}

public enum EState
{
    Idle,
    Move,
    Walk,
    Run,
    Dodge,
    Attack,
    Block,
    UseItem,
    Loot,
}

public class StateGroup { }
