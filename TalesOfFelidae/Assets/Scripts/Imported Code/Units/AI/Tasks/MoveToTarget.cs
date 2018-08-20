using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[CreateAssetMenu (menuName = "Tasks/MoveToTarget")]
public class MoveToTarget : BaseTask {

    // Use this for initialization
    public override void ExecuteFixedUpdate(UnitPawn unit)
    {
        if (unit.navMeshAgent.isStopped) unit.navMeshAgent.isStopped = false;
        unit.navMeshAgent.velocity = unit.transform.forward.normalized * 14f * Time.fixedDeltaTime;
        Vector3 moveVector = (unit.navMeshAgent.nextPosition - unit.transform.position) * 12f; //* (unit.unitStats.Speed);
        moveVector.y = 0f;
        unit.transform.position += moveVector * Time.deltaTime;
        unit.Rotate(unit.navMeshAgent.nextPosition);
    }

    public override bool TestKeys(UnitPawn unit)
    {
        //if (unit.taskKeys.isAutoAttacking) return false;
        if (!unit.navMeshAgent.pathPending &&
            unit.navMeshAgent.remainingDistance <= unit.navMeshAgent.stoppingDistance) unit.taskKeys.isWalking = false;
        else if (unit.taskKeys.isWalking && !unit.taskKeys.isAutoAttacking) return true;

        return false;
    }

}*/
