using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StateDie : StateBase
{
    public override void Initialize(Animator animator1, FsmManager fsmManager1, NavMeshAgent agent1)
    {
        base.Initialize(animator1, fsmManager1, agent1);
        stateType = StateType.Die;
    }

    public override void OnEnter()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetInteger(State, (int)StateType.Die);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }


    
}