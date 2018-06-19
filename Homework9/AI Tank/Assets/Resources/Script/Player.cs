using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank {

    public delegate void destroy();
    public static event destroy destroyEvent;

	// Use this for initialization
	void Start () {
        setHealth(400f);
	}
	
	// Update is called once per frame
	void Update () {
		if(getHealth() <= 0)
        {
            gameObject.SetActive(false);
            if(destroyEvent != null)
            {
                destroyEvent();
            }
        }
	}
}
