using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

    public GameMode mode
    {
        get;
        set;
    }

	public IActionManager actionManager
    {
        get;
        set;
    }
    
    public ScoreController scoreController
    {
        get;
        set;
    }
    
    public Queue<GameObject> UFOQueue = new Queue<GameObject>();

    private int round;
    private int UFONumber;
    private float interval;
    private GameState gameState;


    void Awake()
    {
        Director director = Director.GetInstance();
        director.currentSceneController = this;
        this.gameObject.AddComponent<ScoreController>();
        scoreController = Singleton<ScoreController>.Instance;
        this.gameObject.AddComponent<UFOFactory>();
        this.gameObject.AddComponent<UFOScriptFactory>();

        UFONumber = 10;
        round = -1;
        interval = 1;
        gameState = GameState.START;
        mode = GameMode.EMPTY;

        director.currentSceneController.LoadResource();
    }

    public void LoadResource()
    {
        GameObject GreenPlane = GameObject.Instantiate(Resources.Load("Prefabs/Terrian", typeof(GameObject)), new Vector3(-286,-6,0), Quaternion.identity, null) as GameObject;
    }

    private void Update()
    {
        //Debug.Log("Interval = " + interval);
        if(mode != GameMode.EMPTY && actionManager != null)
        {
            if (actionManager.getUFONumber() == 0)
            {
                if (gameState == GameState.PLAYING)
                {
                    gameState = GameState.ROUND_FINISH;
                }

                if (gameState == GameState.ROUND_START)
                {
                    //修改难度需要调节这里
                    //round = (round + 1) % totalRound;
                    round = (round + 1) % 3;
                    NextRound();
                    actionManager.setUFONumber(10);
                    gameState = GameState.PLAYING;
                }
            }

            if (interval > 2)
            {
                //Debug.Log("发射");
                Launch();
                interval = 0;
            }
            else
            {
                interval += Time.deltaTime;
            }
        }
        else if(mode == GameMode.EMPTY)
        {
            UFONumber = 10;
            round = -1;
            interval = 0;
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
        Vector3 position = new Vector3(-UFO.GetComponent<UFOObject>().GetUFODirection().x * 4, seed, 0);
        return position;
    }

    private void NextRound()
    {
        UFOFactory factory = Singleton<UFOFactory>.Instance;
        for(int i=0;i<UFONumber;i++)
        {
            //难度修改器可能要修改一下这样
            UFOQueue.Enqueue(factory.GetUFO(round, mode));
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

    public void SetMode(GameMode newMode)
    {
        if(mode == GameMode.KINEMATIC)
        {
            this.gameObject.AddComponent<CCActionManager>();
        }
        else if(mode == GameMode.PHYSIC) {
            this.gameObject.AddComponent<PhysicActionManager>();
        }

        mode = newMode;
    }

    public GameMode GetMode()
    {
        return mode;
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
}
