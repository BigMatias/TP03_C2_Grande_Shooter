using UnityEngine;
using UnityEngine.AI;

public class StateIdle : StateBase
{
    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent1)
    {
        base.Initialize(animator1, fsmManager1, agent1);    
        stateType = StateType.Idle;
    }

    public override void OnEnter()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
        animator.SetInteger(State, (int)StateType.Idle);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
