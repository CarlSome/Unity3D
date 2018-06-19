using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Tank {

    public delegate void recycle(GameObject Tank);
    public static event recycle recycleEvent;

    private Vector3 target;
    private bool gameOver;
    private ISceneController currentScenceController;
    private Factory factory;

	// Use this for initialization
	void Start () {
        currentScenceController = Director.GetInstance().CurrentScenceController;
        factory = Singleton<Factory>.Instance;
        setHealth(100f);
        StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void Update () {
        gameOver = currentScenceController.IsGameOver();
        if (gameOver == false)
        {
            target = currentScenceController.getPlayerPosition();
            if(getHealth() <= 0 && recycleEvent != null)
            {
                Debug.Log("Destroy");
                recycleEvent(gameObject);
            }
            else
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.SetDestination(target);
            }
        }
        else
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
	}

    IEnumerator Shoot()
    {
        while(gameOver == false)
        {
            for(float i=2; i>0;i-= Time.deltaTime)
            {
                yield return 0;
            }
            if(Vector3.Distance(transform.position, target) <= 15)
            {
                GameObject bullet = factory.getBullet(tankType.ENEMY);
                bullet.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z) + transform.forward * 1.5f;
                bullet.transform.forward = transform.forward;
                Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
                rigidbody.AddForce(bullet.transform.forward * 20, ForceMode.Impulse);
            }
        }
    }
}
