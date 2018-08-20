using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlayerController : UnitController {
    
    public Camera cam;
    public PlayerPawn pawn;

    private Ray camRay;
    private RaycastHit camRayHit;

    static PlayerController Singleton;

    public static PlayerController GetInstance()
    {
        return Singleton;
    }

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
            //DontDestroyOnLoad(gameObject);

            damageHandler = GetComponent<DamageHandler>();
            inventoryHandler = GetComponent<InventoryHandler>();

            //AssignUnitStats();
        }
    }

    private void Update()
    {
        inventoryHandler.attackTimer -= Time.deltaTime;
        damageHandler.RegenerateHealth();
    }

    private void FixedUpdate()
    {
        camRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && Physics.Raycast(camRay, out camRayHit)) //&& !pawn.taskKeys.isUsingAbility)
        {
            if (camRayHit.transform.tag == "Enemy" && camRayHit.collider.GetComponent<UnitPawn>() != null)
            {
                pawn.OnClickEnemy(camRayHit.collider.GetComponent<UnitPawn>());
            }
            else if (camRayHit.transform.tag == "Interactable" && camRayHit.collider.GetComponent<InteractableObject>() != null)
            {
                pawn.OnClickInteractable(camRayHit.collider.GetComponent<InteractableObject>());
            }
            else if (camRayHit.transform.tag == "Ground")
            {
                pawn.OnClickGround(camRayHit.point);
            }
        }
    }

    /*
    public void AssignUnitStats()
    {
        GameStateManager.GetInstance().currentGameData.pcBody = 10f;
        GameStateManager.GetInstance().currentGameData.pcArms = 10f;
        GameStateManager.GetInstance().currentGameData.pcTech = 10f;
    }
}*/
