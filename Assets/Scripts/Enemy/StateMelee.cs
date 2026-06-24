using UnityEngine;
using UnityEngine.AI;

public class StateMelee : StateBase
{
    private float attackCdAux;

    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent1)
    {
        base.Initialize(animator1, fsmManager1, agent1);
        stateType = StateType.Punch;
    }

    public override void OnEnter()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        attackCdAux = 0f;
        animator.SetInteger(State, (int)StateType.Punch);
    }

    public override void OnUpdate()
    {
        attackCdAux -= Time.deltaTime;
        if (attackCdAux <= 0f)
        {
            fsmManager.Punch();
            attackCdAux = fsmManager.EnemyData.meleeCd;
        }
    }

    public override void OnExit()
    {
        agent.isStopped = false;
    }
}