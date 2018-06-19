using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISceneController : MonoBehaviour, IUserAction {

    public GameObject player;

    private bool gameOver;
    private int enemyNum;
    private Factory factory;
    private Director director;

    void Awake()
    {
        director = Director.GetInstance();
        director.CurrentScenceController = this;
        gameOver = false;
        enemyNum = 6;
        factory = Singleton<Factory>.Instance;
        player = factory.getPlayer();
    }

	// Use this for initialization
	void Start () {
		for(int i=0;i<enemyNum;i++)
        {
            factory.getTank();
        }
        //Player.destroyEvent += setGameOver;
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
        Player.destroyEvent += setGameOver;
    }

    public Vector3 getPlayerPosition()
    {
        return player.transform.position;
    }

    public void setGameOver()
    {
        gameOver = true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void MoveForward()
    {
        player.GetComponent<Rigidbody>().velocity = player.transform.forward * 10;
    }

    public void MoveBackward()
    {
        player.GetComponent<Rigidbody>().velocity = player.transform.forward * -10;
    }

    public void  Turn(float offsetX)
    {
        float x = player.transform.localEulerAngles.y + offsetX * 5;
        float y = player.transform.localEulerAngles.x;
        player.transform.localEulerAngles = new Vector3(y, x, 0);
    }

    public void Shoot()
    {
        GameObject bullet = factory.getBullet(tankType.PLAYER);
        bullet.transform.position = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z) + player.transform.forward * 1.5f;
        bullet.transform.forward = player.transform.forward;
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(bullet.transform.forward * 20, ForceMode.Impulse);
    }
}
