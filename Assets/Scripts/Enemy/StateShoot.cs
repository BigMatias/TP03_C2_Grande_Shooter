using UnityEngine;
using UnityEngine.AI;

public class StateShoot : StateBase
{
    private LineRenderer lineRenderer;

    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent1)
    {
        base.Initialize(animator1, fsmManager1, agent1);
        stateType = StateType.Shoot;
        lineRenderer = fsmManager.GetComponentInChildren<LineRenderer>();
    }

    public override void OnEnter()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        lineRenderer.enabled = true;
        animator.SetInteger(State, (int)StateType.Shoot);
    }

    public override void OnUpdate()
    {
        lineRenderer.SetPosition(0, fsmManager.gunShootPointTransform.position);
        lineRenderer.SetPosition(1, fsmManager.playerTargetTransform.position);
        
        fsmManager.Shoot();
    }

    public override void OnExit()
    {
        lineRenderer.enabled = false;
    }
}