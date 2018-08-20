using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class UnitPawn : MonoBehaviour {

    [HideInInspector] public BaseTask currentTask;
    [HideInInspector] public TaskKeysTable taskKeys;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public UnitPawn currentTarget;
    [HideInInspector] public Vector3 currentDestination;

    //Animator variables
    [HideInInspector] public Animator animController;
    [HideInInspector] public int walkingHash;
    [HideInInspector] public int attacking1HHash;
    [HideInInspector] public int attacking2HHash;
    [HideInInspector] public int usingPowerHash;
    [HideInInspector] public int idleStateHash;
    [HideInInspector] public int attackStateHash;
    [HideInInspector] public int walkStateHash;

    public List<BaseTask> TaskSequenceList;
    public UnitController owner;

    void Update()
    {
        currentTask.ExecuteUpdate(this);
    }

    void FixedUpdate()
    {
        currentTask.ExecuteFixedUpdate(this);
        TaskManagement();
        CheckAnimation();
    }

    public void TaskManagement()
    {
        for (int i = 0; i < TaskSequenceList.Count; i++)
        {
            if (TaskSequenceList[i].TestKeys(this))
            {
                if (currentTask != TaskSequenceList[i]) currentTask = TaskSequenceList[i];
                return;
            }
        }
    }

    private void CheckAnimation()
    {
        if (taskKeys.isWalking)
        {
            animController.SetBool(walkingHash, true);
            animController.SetBool(attacking1HHash, false);
            animController.SetBool(attacking2HHash, false);
        }
        else if (!taskKeys.isWalking && taskKeys.isAutoAttacking)
        {
            animController.SetBool(walkingHash, false);
            if(owner.inventoryHandler.equippedWeapon.handsRequired == 1) animController.SetBool(attacking1HHash, true);
            if(owner.inventoryHandler.equippedWeapon.handsRequired == 2) animController.SetBool(attacking2HHash, true);
        }
        else
        {
            animController.SetBool(walkingHash, false);
            animController.SetBool(attacking1HHash, false);
            animController.SetBool(attacking2HHash, false);
        }
    }

    public bool CheckClearPathToTarget(Transform target, float distance)
    {
        Vector3 direction = target.position - transform.position;
        RaycastHit hit;
        bool result = false;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.tag == target.tag)
            {
                result = true;
            }
            else result = false;
        }

        return result;
    }

    public void Rotate(Vector3 nextDestination)
    {
        Vector3 playerToTarget = nextDestination - transform.position;
        playerToTarget.y = 0f;
        Quaternion newRotation = Quaternion.LookRotation(playerToTarget);
        //Quaternion newRotation = Quaternion.LookRotation(unit.transform.forward);
        transform.rotation = newRotation;
    }

    public void TakeDamage(float damageValue)
    {
        owner.damageHandler.TakeDamage(damageValue);
    }
}*/
