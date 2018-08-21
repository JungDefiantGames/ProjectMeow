using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(DamageHandler))]
[RequireComponent(typeof(InventoryHandler))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    //Handlers
    //public CameraController cameraPrefab;
    CharacterController moveComponent;
    DamageHandler damageHandler;
    InventoryHandler inventoryHandler;
    Animator animController;

    //Character stats
    public float baseSpeed = 6f;
    TraitTag Agility = TraitTag.None;   //This determines how quickly you can move and act
    TraitTag Strength = TraitTag.None;  //This determines how much you can lift or throw
    TraitTag Insight = TraitTag.None;   //This determines whether you can see in darkness and see illusions
    TraitTag Swimming = TraitTag.None;  //This determines whether you can swim and how quickly
    TraitTag Luck = TraitTag.None;      //This determines the likeliness of finding higher value gems
    TraitTag Fireproof = TraitTag.None; //This determines your reduction against fire attacks
    TraitTag Coldproof = TraitTag.None; //This determines your reduction against cold attacks
    TraitTag Evasion = TraitTag.None;   //This determines your reduction against traps
    TraitTag Counter = TraitTag.None;   //This determines whether enemies take damage when they hit you
    TraitTag Storage = TraitTag.None;   //This determines how much storage your backpack possesses

    // Use this for initialization
    void Awake () {
        moveComponent = GetComponent<CharacterController>();
        damageHandler = GetComponent<DamageHandler>();
        inventoryHandler = GetComponent<InventoryHandler>();
        animController = GetComponent<Animator>();
        //CameraController thisCamera = Instantiate(cameraPrefab);
        //thisCamera.target = transform;
    }
	
	// Update is called once per frame
	void Update () {

        //Main Inputs
        if(Input.GetButtonDown("Primary"))
        {
            //Uses item assigned to Primary Slot; otherwise punches or waves (in town)
        }

        if(Input.GetButtonDown("Secondary"))
        {
            //Uses item assigned to Secondary Slot; otherwise does nothing
            //If held, opens a quick slot menu
        }

        if(Input.GetButtonDown("Interact"))
        {
            //Context-sensitive, interacts with nearest interactable
            //Picks up object/grabs heavy object when held/talks to NPC/uses default item with object; otherwise dashes
        }

        if(Input.GetButtonDown("OpenMenu"))
        {
            //Opens a menu; this does not pause the game
            //Taking damage will cause the menu to exit
        }

        //Movement
        Vector3 movementVector = new Vector3(baseSpeed * Input.GetAxisRaw("Horizontal"), 0, baseSpeed * Input.GetAxisRaw("Vertical"));
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), 0.15f);
            animController.SetBool("Moving", true);
        }
        else animController.SetBool("Moving", false);

        animController.SetFloat("Velocity Z", (Mathf.Abs(baseSpeed * (Input.GetAxisRaw("Horizontal")) + Mathf.Abs(baseSpeed * (Input.GetAxisRaw("Vertical"))) / 2)));
        moveComponent.SimpleMove(movementVector);
    }
}

public enum TraitTag { None, Base, Upgraded }
