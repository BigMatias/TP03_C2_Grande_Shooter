using UnityEngine;
using UnityEngine.AI;

public class StateThrowGranade : StateBase
{
    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent1)
    {
        base.Initialize(animator1, fsmManager1, agent1);
        stateType = StateType.ThrowGranade;
    }

    public override void OnEnter()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetInteger(State, (int)StateType.ThrowGranade);
    }

    public override void OnUpdate()
    {
        fsmManager.ThrowGranade();
    }

    public override void OnExit()
    {
        agent.isStopped = false;
    }
}