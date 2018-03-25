# Unity 3D Homework1

标签（空格分隔）： Unity Homework 潘茂林

---

##1、简答题
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
##2、编程实践：小游戏

  [1]: http://i2.bvimg.com/638111/51f9b62c3f19aed5s.png
