using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    IUserAction action;

	// Use this for initialization
	void Start () {
        action = Director.GetInstance().CurrentScenceController as IUserAction;
	}
	
	// Update is called once per frame
	void Update () {
		if(!action.IsGameOver())
        {
            if(Input.GetKey(KeyCode.W))
            {
                action.MoveForward();
            }

            if(Input.GetKey(KeyCode.S))
            {
                action.MoveBackward();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                action.Shoot();
            }

            float offsetX = Input.GetAxis("Horizontal");
            action.Turn(offsetX);
        }
	}
}
