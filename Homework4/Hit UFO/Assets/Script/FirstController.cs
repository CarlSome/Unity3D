using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

	public CCActionManager actionManager
    {
        get;
        set;
    }
    //public ScoreController scoreController;
    
    public ScoreController scoreController
    {
        get;
        set;
    }
    
    /*
    public UFOFactory factory
    {
        get;
        set;
    }
    */

    //public UFOFactory factory;
    public Queue<GameObject> UFOQueue = new Queue<GameObject>();
    public int totalRound = 3;
    private int round = -1;
    private int UFONumber = 10;
    private float interval = 1;

    private GameState gameState = GameState.START;


    void Awake()
    {
        Director director = Director.GetInstance();
        director.currentSceneController = this;
        this.gameObject.AddComponent<ScoreController>();
        scoreController = Singleton<ScoreController>.Instance;
        this.gameObject.AddComponent<UFOFactory>();
        director.currentSceneController.LoadResource();

        UFONumber = 10;
    }

    public void LoadResource()
    {
        GameObject GreenPlane = GameObject.Instantiate(Resources.Load("Prefabs/Terrian", typeof(GameObject)), new Vector3(-286,-5,0), Quaternion.identity, null) as GameObject;
    }

    private void Update()
    {
        //Debug.Log("Interval = " + interval);
        if(actionManager.UFONumber == 0)
        {
            if(gameState == GameState.PLAYING)
            {
                gameState = GameState.ROUND_FINISH;
            }

            if (gameState == GameState.ROUND_START)
            {
                //修改难度需要调节这里
                //round = (round + 1) % totalRound;
                round = (round + 1) % 3;
                NextRound();
                actionManager.RefillUFO();
                gameState = GameState.PLAYING;
            }
        }

        if(interval > 2)
        {
            Debug.Log("发射");
            Launch();
            interval = 0;
        }
        else
        {
            interval += Time.deltaTime;
        }
    }

    public void Launch()
    {
        if(UFOQueue.Count != 0)
        {
                GameObject UFO = UFOQueue.Dequeue();
                UFO.transform.position = GetRandomPosition(UFO);
                UFO.SetActive(true);
        }
    }

    private Vector3 GetRandomPosition(GameObject UFO)
    {
        float seed = UnityEngine.Random.Range(0f, 4f);
        Vector3 position = new Vector3(-UFO.GetComponent<UFOObject>().GetUFODirection().x * 7, seed, 0);
        return position;
    }

    private void NextRound()
    {
        UFOFactory factory = Singleton<UFOFactory>.Instance;
        for(int i=0;i<UFONumber;i++)
        {
            //难度修改器可能要修改一下这样
            UFOQueue.Enqueue(factory.getUFO(round));
        }

        actionManager.ReadyLaunch(UFOQueue);
    }

    public int GetScore()
    {
        return scoreController.GetScore();
    }

    public void SetGameState(GameState _gameState)
    {
        gameState = _gameState;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void Shoot(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        RaycastHit[] shoots;
        shoots = Physics.RaycastAll(ray);
        for(int i=0;i<shoots.Length;i++)
        {
            RaycastHit shoot = shoots[i];

            if(shoot.collider.gameObject.GetComponent<UFOObject>() != null)
            {
                scoreController.Count(shoot.collider.gameObject);
                shoot.collider.gameObject.transform.position = new Vector3(0, -1000, 0);
            }
        }
    }

    //测试项
    public void setGameState(GameState _gameState)
    {
        gameState = _gameState;
    }

    public GameState getGameState()
    {
        return gameState;
    }
    public void GameOver()
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(700, 300, 400, 400), "GAMEOVER");
    }
}
