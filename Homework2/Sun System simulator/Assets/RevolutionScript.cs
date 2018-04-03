using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionScript : MonoBehaviour {

    public Transform Center;
    public float Speed;
    public float X;
    public float Y;
    public float Z;
    private Vector3 Axis;

    // Use this for initialization
    void Start () {
        Axis = new Vector3(X, Y, Z);
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(Center.position, Axis, Speed * Time.deltaTime);
	}
}
