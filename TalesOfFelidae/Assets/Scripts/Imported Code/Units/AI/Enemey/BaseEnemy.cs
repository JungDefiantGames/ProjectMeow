using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class BaseEnemy : UnitPawn {

    public EnemyController enemyOwner;
    public Transform weaponBone;

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        taskKeys = ScriptableObject.CreateInstance<TaskKeysTable>();
        animController = GetComponent<Animator>();

        walkingHash = Animator.StringToHash("IsWalking");
        attacking1HHash = Animator.StringToHash("IsAttacking1H");
        attacking2HHash = Animator.StringToHash("IsAttacking2H");
        usingPowerHash = Animator.StringToHash("IsUsingPower");
        idleStateHash = Animator.StringToHash("Base Layer.Idle");
        attackStateHash = Animator.StringToHash("Base Layer.Walk");
        walkStateHash = Animator.StringToHash("Base Layer.Attack");
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        currentTask = TaskSequenceList[0];
        enemyOwner = owner as EnemyController;
        // currentTask.ExecuteUpdate(this);
    }

    public void OnSelectEnemy(UnitPawn newTarget)
    {
        navMeshAgent.isStopped = true;
        taskKeys.isAutoAttacking = true;
        taskKeys.isWalking = false;
        currentTarget = newTarget;
    }

    public void OnSelectDestination(Vector3 newDestination)
    {
        navMeshAgent.isStopped = false;
        taskKeys.isAutoAttacking = false;
        taskKeys.isWalking = true;
        currentDestination = newDestination;
        //currentTarget = null;
        navMeshAgent.destination = currentDestination;
    }

    public void PerformAttack()
    {
        Rotate(currentTarget.transform.position);
        enemyOwner.inventoryHandler.weaponParticle.EmitBulletParticle();
        enemyOwner.inventoryHandler.attackTimer = enemyOwner.inventoryHandler.equippedWeapon.attackRate;
    }
}*/
