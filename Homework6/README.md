# Homework6 不智能的巡逻兵

---

## 游戏规则
使用WSAD或方向键上下左右移动player，进入巡逻兵的追捕后逃脱可积累一分，若与巡逻兵碰撞则游戏结束

---

## 设计要求

- 游戏设计要求
 - 创建一个地图和若干巡逻兵(使用动画)；
 - 每个巡逻兵走一个3~5个边的凸多边型，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算；
 - 巡逻兵碰撞到障碍物，则会自动选下一个点为目标；
 - 巡逻兵在设定范围内感知到玩家，会自动追击玩家；
 - 失去玩家目标后，继续巡逻；
 - 计分：玩家每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束；

- 程序设计要求：
  - 必须使用订阅与发布模式传消息
   - Subject：OnLostGoal
   - Publisher: EventHandler
   - Subscriber: FirstSceneController
  - 工厂模式生产巡逻兵

---

## 游戏效果

![游戏截图](https://github.com/CarlSome/Unity3D/blob/master/Homework6/%E6%88%AA%E5%9B%BE/%E6%B8%B8%E6%88%8F%E6%88%AA%E5%9B%BE.png?raw=true)

视频链接：https://pan.baidu.com/s/1zUb6sUbaek0sqvt6lf12RA

---

## 游戏实现

本游戏的实现主要参考了Unity 3D官方教程的与师兄的代码。具体实现如下：

巡逻兵：

    巡逻兵使用了官方教程的模型与动画。在此基础上添加一个Sphere Collider以检测玩家是否进入范围。

![Patrol预制](https://raw.githubusercontent.com/CarlSome/Unity3D/master/Homework6/%E6%88%AA%E5%9B%BE/Patrol%E9%A2%84%E5%88%B6.png)

巡逻兵相关代码：

 - Patrol.cs

 ```
 //保存巡逻兵的相关数据
 public class Patrol : MonoBehaviour {

    public GameObject player; //追踪的玩家
    public bool follow = false; //是否在追踪状态
    public Vector3 start_postion; //巡逻起点
}
 ```

 - Factory.cs

 ```
 //生产巡逻兵的工厂
 public class Factory : MonoBehaviour {

    private List<GameObject> usingPatrolList; //正在使用的巡逻兵的列表
    private GameObject patorl;  //巡逻兵对象
    private Vector3[] patorlPosition; //巡逻兵产生的位置

    private void Awake()
    {
        usingPatrolList = new List<GameObject>();
        patorl = null;
        patorlPosition = new Vector3[7];

        patorlPosition[0] = new Vector3(20, 0, 10);
        patorlPosition[1] = new Vector3(0, 0, 10);
        patorlPosition[2] = new Vector3(-10, 0, 5);
        patorlPosition[3] = new Vector3(-20, 0, 0);
        patorlPosition[4] = new Vector3(30, 0, 0);
        patorlPosition[5] = new Vector3(20, 0, -10);
        patorlPosition[6] = new Vector3(6, 0, -20);
    }

    //从预制中实例化巡逻兵并放进正在使用的队列中并返回
    public List<GameObject> GetPatrol()
    {
        for(int i=0;i<7;i++)
        {
            patorl = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"));
            patorl.transform.position = patorlPosition[i];
            patorl.GetComponent<Patrol>().start_postion = patorlPosition[i];
            usingPatrolList.Add(patorl);
        }

        return usingPatrolList;
    }
}
 ```

 - PatrolActionManager.cs

 ```
 //管理巡逻兵的动作
 public class PatrolActionManager : SSActionManager {

    private GoPatrolAction goPatrol;  //巡逻动作

    public void GoPatrol(GameObject patrol)
    {
        goPatrol = GoPatrolAction.GetSSAction(patrol.transform.position);
        this.Act(patrol, goPatrol, this);
    }

    public void DestroyAllAction()
    {
        DestroyAll();
    }
}
 ```

 - GoPatrolAction.cs


 ```
 //控制巡逻兵的巡逻动作

public class GoPatrolAction : SSAction {

	private enum Direction { //移动方向
        NORTH,
        WEST,
        SOUTH,
        EAST
    };
    private Patrol patrol;
    private float x;  //巡逻兵位置的x轴
    private float y;  //巡逻兵位置的y轴（实际对应Unity3D的z轴，但为方便理解，在代码中记录为y）
    private float length; //移动距离
    private float speed = 1.2f; //巡逻速度
    private bool arrived = true;  //是否到达目标位置
    private Direction direction = Direction.EAST; //初始方向

    public override void Start() {
        patrol = this.gameObject.GetComponent<Patrol>();
        this.gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
    }

    public override void Update()
    {
        //如果玩家不在范围内，进行巡逻；否则，开始追踪玩家
        if(patrol.follow == false)
        {
            Gopatrol();
        }
        else
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, 0, this.gameObject);
        }
    }

    private GoPatrolAction()
    {

    }

    //巡逻动作
    private void Gopatrol()
    {
        //移动目标设置
        if(arrived)
        {
            if(direction == Direction.EAST)
            {
                x -= length;
            }
            else if(direction == Direction.WEST)
            {
                x += length;
            }
            else if(direction == Direction.SOUTH)
            {
                y -= length;
            }
            else
            {
                y += length;
            }
            arrived = false;
        }

        //面朝移动
        this.transform.LookAt(new Vector3(x, 0, y));
        float distance = Vector3.Distance(transform.position, new Vector3(x, 0, y));

        //到达目标位置后，设置下一个运动方向
        if(distance > 0.1)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(x, 0, y), speed * Time.deltaTime);
        }
        else
        {
            direction = direction + 1;
            if (direction > Direction.EAST)
            {
                direction = Direction.NORTH;
            }
            arrived = true;
        }

    }

    public static GoPatrolAction GetSSAction(Vector3 destination)
    {
        GoPatrolAction action = CreateInstance<GoPatrolAction>();
        action.x = destination.x;
        action.y = destination.z;
        action.length = Random.Range(4, 4);

        return action;
    }
}

 ```

 - PatrolFollow.cs


 ```
//控制巡逻兵的跟踪行为
public class PatrolFollow : SSAction {

    private float speed = 5f; //跟踪速度
    private GameObject player;  //跟踪对象
    private Patrol patrol;

    public override void Start()
    {
        patrol = this.gameObject.GetComponent<Patrol>();
    }

    public override void Update()
    {
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0) {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }

        if (transform.position.y != 0) {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        Follow();

        if (patrol.follow == false)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, 1, this.gameObject);
        }
    }

    private PatrolFollow() { }

    private void Follow()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        this.transform.LookAt(player.transform.position);
    }

    public static PatrolFollow GetSSAction(GameObject player)
    {
        PatrolFollow action = CreateInstance<PatrolFollow>();
        action.player = player;
        return action;
    }
}

 ```

玩家：

玩家同样使用了官方教程的模型与动画。因为资源已为模型添加了Capsule Collider，我们添加其他东西。

![Player预制](https://github.com/CarlSome/Unity3D/blob/master/Homework6/%E6%88%AA%E5%9B%BE/Player%E9%A2%84%E5%88%B6.png?raw=true)

玩家相关代码：

- PlayerController.cs


```
//玩家控制部分我基本复用了官方教程的代码，只是在控制玩家转向部分作了修改。原版是射击游戏，所以玩家是通过鼠标来转向的。但在这个游戏中我们并不需要这样做。另外我们死亡后不能移动角色，因此在原代码上我加了这个移动条件的限制
namespace CompleteProject
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.

        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float camRayLength = 100f;          // The length of the ray from the camera into the scene.

        FirstSceneController firstSceneController;
        bool GameOver = false;

        void Awake()
        {
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask("Floor");

            // Set up references.
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();

            firstSceneController = Director.GetInstance().CurrentScenceController as FirstSceneController;
        }


        void FixedUpdate()
        {
            GameOver = firstSceneController.GetGameOver();
            if(!GameOver)
            {
                // Store the input axes.
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");

                // Move the player around the scene.
                Move(h, v);

                // Turn the player to face the mouse cursor.
                Turning(h, v);

                // Animate the player.
                Animating(h, v);
            }

        }


        void Move(float h, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition(transform.position + movement);
        }


        void Turning(float h, float v)
        {

            if(!(h == 0 && v == 0))
            {
                Quaternion newRotatation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                Quaternion oldRotatation = playerRigidbody.rotation;
                if (newRotatation != oldRotatation)
                {
                    playerRigidbody.MoveRotation(newRotatation);
                }
            }

        }


        void Animating(float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool("IsWalking", walking);
        }
    }
}
```


订阅与发布模式:

订阅者：

- FirstSceneController.cs


```
public class FirstSceneController : MonoBehaviour, ISceneController, IUserAction
{

    public GameObject player;
    public Factory factory;
    public ScoreController scoreController;
    public PatrolActionManager patrolActionManager;
    public float playerSpeed = 5f;
    private List<GameObject> patrols;
    private bool gameOver = false;


    // Use this for initialization
    void Start()
    {
        Director director = Director.GetInstance();
        director.CurrentScenceController = this;
        factory = Singleton<Factory>.Instance;
        patrolActionManager = gameObject.AddComponent<PatrolActionManager>() as PatrolActionManager;
        LoadResource();
        scoreController = Singleton<ScoreController>.Instance;

    }

    public void LoadResource()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Environment"));
        Instantiate(Resources.Load<GameObject>("Prefabs/Floor"));
        Instantiate(Resources.Load<GameObject>("Prefabs/Lights"));
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        patrols = factory.GetPatrol();

        for (int i = 0; i < patrols.Count; i++)
        {
            patrolActionManager.GoPatrol(patrols[i]);
        }
    }

    public int GetScore()
    {
        return scoreController.GetScore();
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    //订阅部分代码
    private void Score()
    {
        scoreController.Score();
    }

    private void GameOver()
    {
        gameOver = true;
        patrolActionManager.DestroyAllAction();
    }

    private void OnEnable()
    {
        EventHandler.GameOverChange += GameOver;
        EventHandler.ScoreChange += Score;
    }

    private void OnDisable()
    {
        EventHandler.GameOverChange -= GameOver;
        EventHandler.ScoreChange -= Score;
    }
}
```

发布者：

 - EventHandler.cs


 ```
 //发布事件的类
 public class EventHandler : MonoBehaviour {

    public delegate void GameOverEvent();
    public static event GameOverEvent GameOverChange;

    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;

    public void PlayerEscape()
    {
        if(ScoreChange != null)
        {
            ScoreChange();
        }
    }

    public void PlayerCaught()
    {
        if(GameOverChange != null)
        {
            GameOverChange();
        }
    }
}
 ```
