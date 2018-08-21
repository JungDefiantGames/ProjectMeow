using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlayerPawn : UnitPawn {

    public PlayerController playerOwner;
    public InteractableObject interactable;
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
        playerOwner = owner as PlayerController;
       // currentTask.ExecuteUpdate(this);
    }

    public void OnClickEnemy(UnitPawn newTarget)
    {
        navMeshAgent.isStopped = true;
        taskKeys.isAutoAttacking = true;
        taskKeys.isPickingUpObject = false;
        taskKeys.isWalking = false;
        currentTarget = newTarget;
    }

    public void OnClickGround(Vector3 newDestination)
    {
        navMeshAgent.isStopped = false;
        taskKeys.isAutoAttacking = false;
        taskKeys.isPickingUpObject = false;
        taskKeys.isWalking = true;
        currentDestination = newDestination;
        //currentTarget = null;
        navMeshAgent.destination = currentDestination;
    }

    public void OnClickInteractable(InteractableObject newInteractable)
    {
        navMeshAgent.isStopped = true;
        taskKeys.isAutoAttacking = false;
        taskKeys.isPickingUpObject = true;
        taskKeys.isWalking = false;
        interactable = newInteractable;
    }

    public void PerformAttack()
    {
        Rotate(currentTarget.transform.position);
        playerOwner.inventoryHandler.weaponParticle.EmitBulletParticle();
        playerOwner.inventoryHandler.attackTimer = playerOwner.inventoryHandler.equippedWeapon.attackRate;
    }
}*/
