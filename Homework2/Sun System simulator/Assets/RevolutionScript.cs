using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionScript : MonoBehaviour {

    public Transform Center;    //公转中心，默认为太阳的Position
    public float Speed; //公转线速度
    public float RotateSpeed;//自转角速度
    //围绕旋转轴的x,y,z值
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
        transform.Rotate(Vector3.up * 360 * Time.deltaTime / RotateSpeed);
    }
}
