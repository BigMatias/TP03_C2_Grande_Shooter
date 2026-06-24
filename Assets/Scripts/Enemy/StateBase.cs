using UnityEngine;
using UnityEngine.AI;

public abstract class StateBase 
{
    protected static readonly int State = Animator.StringToHash("State");

    public StateType stateType = StateType.None;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected FsmManager fsmManager;

    public virtual void Initialize(Animator animator, FsmManager fsmManager, NavMeshAgent agent)
    {
        this.animator = animator;
        this.agent = agent;
        this.fsmManager = fsmManager;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}

