# Unity 3D Homework1

标签（空格分隔）： Unity Homework 潘茂林

---

## 1、简答题
 - 解释游戏对象（GameObjects）和资源（Assets）的区别与联系。
&emsp;区别：游戏对象指的是我们在游戏中创建的且可以在设计界面中直接操作的物体，而资源更多指的是从外部引入的3D模型、音频文件、图片等。
&emsp;联系：同一个资源以使用在不同的游戏对象上，作为对象的属性。  

 - 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
&emsp;资源结构：资源的目录组织结构一般以资源的类型为父目录（如：Models、Prefabs、Scenes等），在这些父目录下又可以根据游戏对象划分子目录（如:Cars、Driver、Bush等）或直接存放子文件。
&emsp;对象组织的结构：游戏对象树的层次结构一般以Scene为父目录，不同Scene中有不同的游戏对象。同一个Scene中的子对象一般包含基本的Main Camera、Directional Light以及设计者创建的必要的游戏对象空目录（如：人物、环境、UI界面等），空目录用于存放属于同一类对象的不同实例。

 - 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件
&emsp;基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
```
using UnityEngine;
using System.Collections;
 
public class ExampleClass : MonoBehaviour {
   #验证Awake()触发条件：当script加载完成时触发
    void Awake() {
        Debug.Log("Awake");
    }
     
    #验证Start()触发条件：在script加载完成且开始被使用后、在有update前触发
    void Start() {
        Debug.Log("Start");
    }
    
    #验证Update()触发条件：每一渲染帧触发一次
    void Update() {
        Debug.Log("Update");
    }
    
    #验证FixedUpdate()触发条件：每一固定帧触发一次
    void FixedUpdate() {
        Debug.Log("FixedUpdate");
    }
    
    #验证LateUpdate触发条件：其他Update被触发后每帧触发
    void LateUpdate() {
        Debug.Log("LateUpdate");
    }
}
```
&emsp;&emsp; 常用事件包括 OnGUI() OnDisable() OnEnable()
```
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    #验证OnGUI()触发条件：当需要渲染或处理GUI时触发
    void OnGUI() {
        Debug.Log("OnGUI");
    }
    
    #验证OnDisable()触发条件：当对象被禁用时触发
    void OnDisable {
        Debug.Log("OnDisable");
    }
    
    #验证OnEnable()触发条件：当对象被激活时触发
    void OnEnable() {
        Debug.Log("OnEnable");
    }
}
```
- 查找脚本手册，了解 GameObject，Transform，Component 对象
  - 分别翻译官方对三个对象的描述（Description）
    &emsp;GameObject:游戏场景中所有实体的基类
    &emsp;Transform:对象的位置、旋转角度与大小
    &emsp;Component:所有附加到GameObject的资源的基类
  - 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
    &emsp;对象的属性：table对象有chair1、chair2、chair3和chair4四个实体
    &emsp;Transform：table的位置为(0,0,0),旋转角度为(0,0,0)，大小为(1,1,1)——即长、宽、高均为一个单位大小
    &emsp;部件：table的部件有Cube、Box Collider、Mesh Renderer和Material
  - 用UML图描述三者的关系
    ![此处输入图片的描述][1]
- 整理相关学习资料，编写简单代码验证以下技术的实现：
  - 查找对象
  - 添加子对象
  - 遍历对象树
  - 清除所有子对象

```
#查找对象
var obj = GameObject.Find("ObjectName");    // static GameObject Find (string name)
var obj = GameObject.FindWithTag("TagName");    //static GameObject FindWithTag (string tag) 
var obj = GameObject.FindObjectOfType(TypeName);    //static Object FindObjectsOfType(Type type)
 
#添加子对象
GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane); //public static GameObect CreatePrimitive(PrimitiveTypetype)
GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
...

#遍历对象树
foreach (Transform child in transform) {
    //具体操作
}

#清除所有子对象
foreach (Transform child in transform) {
    Destroy(child.gameObject);
}
```

- 资源预设（Prefabs）与 对象克隆 (clone)
  - 预设（Prefabs）有什么好处？
    &emsp;预设可以理解为对象属性的模板，使用预设方便我们将同一种(或多种)属性应用在不同的游戏对象中
  - 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
    &emsp;预设和对象克隆都能重复使用同种属性，但克隆会将对象完全复制，所得的克隆体与母体完全一致，不能将母体的某些属性提炼出来应用于其他对象上。
  - 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
```
public GameObject Table;

void Start() {
    void Start() {
        GameObject instance = (GameObject)Instantiate(Table, transform.position, transform.rotation);  
    }
}
```

- 尝试解释组合模式（Composite Pattern / 一种设计模式）
&emsp;组合模式：组合模式指将对象组合成树形结构以表示“部分-整体”的层次结构，它可以让用户在使用单个对象或组合对象时保持一致性。

- 使用 BroadcastMessage() 方法向子对象发送消息
给父对象绑定以下脚本：
```
public class Father : MonoBehaviour {
    void message() {
        Debug.Log("I'm your father");
    }
    
    void Start() {
        this.BroadcastMessage("message");
    }
}
```

给子对象绑定以下脚本:
```
public class Son : MonoBehaviour {
    void message() {
        Debug.Log("I'm your father");
    }
}
```
---
## 2、编程实践：小游戏——井字棋
### 需求分析:
- 使用Unity 3D实现井字棋小游戏
- 仅能使用IMGUI构建UI

### 功能分析：
- 能模拟棋盘
- 能自动切换角色(O棋和X棋交替）
- 能判断胜利条件
- 能给出游戏状态显示
- 能重置游戏

### 功能实现
- 模拟棋盘
    用一个3x3的int二维数组来表示一个井字棋盘，其中O棋位置用1表示，X棋位置用-1表示，空白用0表示
- 自动切换角色
    拟用一个int值来表示两位玩家：O棋为1，X棋为-1。至于为什么不用bool值呢？主要是因为用1和-1切换起来相对方便，且使用该值方便直接给棋盘对应位置赋值表示棋子。
- 胜利条件判断
    胜利条件的判断是本游戏的核心。井字棋中共有4种游戏状态：O获胜、X获胜、平局和未结束。其中O获胜又可以分为横向获胜、纵向获胜和对角线获胜，X同理。因为在棋盘中，O棋用1表示，X棋用-1表示，那么我们只需要通过计算每一行、列、对角线的三个空格的和是否为3或-3即可判断O棋或X棋是否获胜。如没有任何一方获胜，则判断棋盘是否已满（在本游戏中，我创建了一个int类型的计步器，每下一枚棋计步器会加一，当计步器为9时，棋盘满—），若棋盘已满，则游戏平局，反之游戏继续。
    以下是胜利条件判断的伪代码
```
    Sum = 横/竖/斜三个的和
    IF(Sum == 3) 
        O胜
    ELSE IF(Sum == -3)
        X胜
    IF(棋盘满）
        平局
    ELSE
        游戏继续
```
- 游戏状态显示
    这个功能的实现非常简单，只需要通过胜利条件判断函数读取游戏状态并作出对应的提示就可以了。根据设计，当O胜，胜利条件判断函数返回1；当X胜，返回-1；当平局，返回0；当游戏未结束，返回666不多赘述。
- 重置游戏
    当游戏结束或游戏中途，玩家可以通过这个按钮来重置游戏。在具体实现上，只需要将棋盘所有位置清零并重置计步器就可以了。

### 游戏演示
- 游戏界面
    ![此处输入图片的描述][2]
- O获胜
![此处输入图片的描述][3]
- X获胜
![此处输入图片的描述][4]
- 平局
![此处输入图片的描述][5]
    
### 完整代码
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
    private int[,] GameBoard = new int[3, 3];   //棋盘
    private int Player = 1; //两位玩家分别用1和-1表示
    private int StepCount = 0;  //计步器，用于判断平局
    private bool Flag = false;  //游戏结束时为true,否则为false

    //开局初始化
    void Start()
    {
        Initialize();
    }

    //游戏主体
    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width) / 2 - 150, 20, 300, 290), "Tic-Tac-Toe");

        int result = GameOver();
        if(result == 1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 100, 200, 200, 50), "O has won! Reset and play again!");
        }
        else if(result == -1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 100, 200, 200, 50), "X has won! Reset and play again!");
        }
        else if(result == 0)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 110, 200, 220, 50), "End in a draw, reset and play again!");
        }

        if(Player == 1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 60, 220, 120, 20), "Current Player: O");
        }
        else
        {
            GUI.Label(new Rect((Screen.width) / 2 - 60, 220, 120, 20), "Current Player: X");
        }

        if(GUI.Button(new Rect((Screen.width) / 2 - 50, 250, 100, 40), "Reset"))
        {
            Initialize();
        }

        for (int i=0;i<3;i++)
        {
            for(int ii=0;ii<3;ii++)
            {
                if(GameBoard[i,ii] == 1)
                {
                    GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), "O");
                }
                else if (GameBoard[i, ii] == -1)
                {
                    GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), "X");
                }
                if(GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), ""))
                {
                    if(Flag == false)
                    {
                        StepCount++;
                        GameBoard[i, ii] = Player;
                        Player = -Player;
                    }
                }
            }
        }
    }

    //初始化函数
    void Initialize()
    {
        for(int i=0;i<3;i++)
        {
            for(int ii=0;ii<3;ii++)
            {
                GameBoard[i, ii] = 0;   //未激活的位置为0
            }
        }
        Player = 1;
        StepCount = 0;
        Flag = false;
    }

    //游戏结束判断函数：玩家1胜返回1，玩家-1胜返回-1.平局返回0，未结束返回666
    int GameOver()
    {
        int Sum = 0;
        for(int i=0;i<3;i++)    //横向判断
        {
            if(GameBoard[i,0] == 0) //第一位没被点击，不可能存在
            {
                continue;
            }
            Sum = GameBoard[i, 0] + GameBoard[i, 1] + GameBoard[i, 2];
            if(Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if(Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        for(int i=0;i<3;i++)    //纵向判断
        {
            if(GameBoard[0,i] == 0)
            {
                continue;
            }
            Sum = GameBoard[0, i] + GameBoard[1, i] + GameBoard[2, i];
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        if (GameBoard[1, 1] != 0)   //斜向判断
        {
            Sum = GameBoard[0, 0] + GameBoard[1, 1] + GameBoard[2, 2]; 
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }

            Sum = GameBoard[0, 2] + GameBoard[1, 1] + GameBoard[2, 0];
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        if(StepCount == 9) //平局
        {
            Flag = true;
            return 0;
        }
        return 666;
    }
}

```
### 总结
    总体而言，这次游戏还是比较简单的，虽然我没有足够多的时间和精力去完成更多的设计。如果可以的话，我的代码还可以进一步精简，另外还可以拓展棋盘尺寸还有加入AI与玩家对抗，但这样做的话就需要花时间去设计更复杂的算法了，怕了怕了。


  [1]: http://i2.bvimg.com/638111/51f9b62c3f19aed5s.png
  [2]: http://i2.bvimg.com/638111/8cc8bd4b716bc73fs.png
  [3]: http://i2.bvimg.com/638111/7aea06b672d64153s.png
  [4]: http://i2.bvimg.com/638111/931c6b3306524385s.png
  [5]: http://i2.bvimg.com/638111/0545a9d2d1b39feas.png