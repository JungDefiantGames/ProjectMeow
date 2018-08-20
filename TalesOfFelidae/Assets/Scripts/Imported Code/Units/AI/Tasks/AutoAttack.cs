using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[CreateAssetMenu(menuName = "Tasks/AutoAttackTask")]
public class AutoAttack : BaseTask {

    public override void ExecuteFixedUpdate(UnitPawn unit)
    {
        if (unit.currentTarget != null)
        {
            BaseWeapon weapon = unit.owner.inventoryHandler.equippedWeapon;

            if (unit.CheckClearPathToTarget(unit.currentTarget.transform, weapon.attackRange) &&
                unit.owner.inventoryHandler.attackTimer <= 0)
            {
                unit.navMeshAgent.isStopped = true;
                unit.taskKeys.isWalking = false;
            }
            else if (!unit.CheckClearPathToTarget(unit.currentTarget.transform, weapon.attackRange))
            {
                unit.navMeshAgent.isStopped = false;
                unit.taskKeys.isWalking = true;
                unit.currentDestination = unit.currentTarget.transform.position;
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
        if (unit.taskKeys.isAutoAttacking) return true;

        return false;
    }
}*/
