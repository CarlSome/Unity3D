using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class TankController : NetworkBehaviour {

	[Header("Movement Variables")]
	[SerializeField] float movementSeppd = 5.0f;
	[SerializeField] float turnSpeed = 45.0f;

	[Header("Camera Position Variables")]
	[SerializeField] float cameraDistance = 16f;
	[SerializeField] float cameraHeight = 16f;

	Rigidbody localRigiBody;  
	Transform mainCamera;
	Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			Destroy (this);
			return;
		}


		localRigiBody = GetComponent<Rigidbody> ();
		cameraOffset = new Vector3 (0f, cameraHeight, -cameraDistance);
		mainCamera = Camera.main.transform;
		MoveCamera ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float turnDis = CrossPlatformInputManager.GetAxis ("Horizontal");
		float moveDis = CrossPlatformInputManager.GetAxis ("Vertical");

		Vector3 translation = transform.position + transform.forward * movementSeppd * moveDis * Time.deltaTime;
		localRigiBody.MovePosition (translation);

		Quaternion rotation = Quaternion.Euler (turnSpeed * new Vector3 (0, turnDis, 0) * Time.deltaTime);
		localRigiBody.MoveRotation (localRigiBody.rotation * rotation);

		MoveCamera ();
	}

	void MoveCamera() {
		mainCamera.position = transform.position;
		mainCamera.rotation = transform.rotation;
		mainCamera.Translate (cameraOffset);
		mainCamera.LookAt (transform);
	}
}
