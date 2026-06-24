using UnityEngine;
using UnityEngine.AI;

public class StateWalk : StateBase
{
    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent)
    {
        base.Initialize(animator1, fsmManager1, agent);
        agent.speed = fsmManager.EnemyData.speed;
        stateType = StateType.Walking;
    }

    public override void OnEnter()
    {
        agent.isStopped = false;
        animator.SetInteger(State, (int)StateType.Walking);
    }

    public override void OnUpdate()
    {
        if (!agent.isOnNavMesh) return;
        agent.SetDestination(fsmManager.playerTargetTransform.position);
    }

    public override void OnExit()
    {

    }
    
}
