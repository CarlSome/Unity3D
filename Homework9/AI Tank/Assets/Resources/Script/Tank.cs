using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    public float health;

	public void setHealth(float _health)
    {
        health = _health;
    }

	public float getHealth () {
        return health;
	}
	
}
