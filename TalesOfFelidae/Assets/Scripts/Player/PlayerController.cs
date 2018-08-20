using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    CharacterController moveComponent;
    Vector3 speed;

	// Use this for initialization
	void Start () {
        moveComponent = GetComponent<CharacterController>();
        speed = new Vector3(0,0);
    }
	
	// Update is called once per frame
	void Update () {

        speed = new Vector3(8 * Input.GetAxisRaw("Horizontal"), 0, 8 * Input.GetAxisRaw("Vertical"));
        if(speed != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(speed), 0.15f);
        moveComponent.SimpleMove(speed);

    }
}
