using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 offset;

    public Transform target;
    

    void Start()
    {
        offset = new Vector3(0, 20f, -20f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = target.position + offset;
	}
}
