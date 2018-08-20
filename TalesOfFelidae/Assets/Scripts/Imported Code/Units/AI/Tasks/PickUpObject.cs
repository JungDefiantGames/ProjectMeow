using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[CreateAssetMenu(menuName = "Tasks/PickUpObjectTask")]
public class PickUpObject : BaseTask {

    public override void ExecuteFixedUpdate(UnitPawn unit)
    {
        PlayerPawn pc = unit as PlayerPawn;

        if (pc.interactable != null)
        {
            if (unit.CheckClearPathToTarget(pc.interactable.transform, 2f))
            {
                unit.navMeshAgent.isStopped = true;
                unit.taskKeys.isWalking = false;
                pc.interactable.OnInteract(pc);
            }
            else if (!unit.CheckClearPathToTarget(pc.interactable.transform, 2f))
            {
                unit.navMeshAgent.isStopped = false;
                unit.taskKeys.isWalking = true;
                unit.currentDestination = pc.interactable.transform.position;
                unit.navMeshAgent.destination = unit.currentDestination;
            }
        }

        if (unit.taskKeys.isWalking)
        {
            if (unit.navMeshAgent.isStopped) unit.navMeshAgent.isStopped = false;
            unit.navMeshAgent.velocity = unit.transform.forward.normalized * 14f * Time.fixedDeltaTime;
            Vector3 moveVector = (unit.navMeshAgent.nextPosition - unit.transform.position) * 12f; //* (unit.unitStats.Speed);
            moveVector.y = 0f;
            unit.transform.position += moveVector * Time.deltaTime;
            unit.Rotate(unit.navMeshAgent.nextPosition);
        }
    }

    public override bool TestKeys(UnitPawn unit)
    {
        if (unit.taskKeys.isPickingUpObject) return true;

        return false;
    }
}*/
