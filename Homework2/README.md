# Unity 3D Homework2

标签（空格分隔）： Unity Homework 潘茂林

---

## 1、简答题
- 游戏对象运动的本质是什么？
游戏对象运动的本质是游戏对象随时间的变换，如位置、旋转等变化。

- 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）
根据课堂的知识，Unity引擎对物体运动的执行是复合的，如：我们给物体添加一个x轴方向的运动，再添加一个y轴方向的运动，那么物体将会沿着两个运动向量的复合方向运动。因此，要实现物体的抛物线运动，我们只需要根据简单的物理知识——抛物线运动是水平方向的匀速直线运动和竖直方向的变速直线运动的复合。
接下来，我们就可以根据以上理论在Unity中模拟抛物线运动
方法一：
```
    #修改Transform属性
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PaoWuXian : MonoBehaviour {

        public float Speed = 1; //初速度
        public float Angle = 0;     //抛物角度，默认0度为水平向右，角度按逆时针方向增加
        private float XSpeed = 1;   //起始的X轴速度
        private float YSpeed = 0;   //起始的Y轴速度
        private float g = 9.8f;

	    // Use this for initialization
	    void Start () {
            XSpeed = Speed * Mathf.Cos(Angle / 57.3f);  //计算初始X轴速度
            YSpeed = Speed * Mathf.Sin(Angle / 57.3f);  //计算初始Y轴速度
	    }
	
	    // Update is called once per frame
	    void Update () {
            this.transform.position += Vector3.right * Time.deltaTime * XSpeed; //X轴移动
            this.transform.position += Vector3.down * Time.deltaTime * YSpeed;  //Y轴移动
            YSpeed += g * Time.deltaTime;   //Y轴速度变化
        }
    }
```

方法二:
```
    #使用Vector3方法
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PaoWuXian : MonoBehaviour {

        public float Speed = 1; //初速度
        public float Angle = 0;     //抛物角度，默认0度为水平向右，角度按逆时针方向增加
        private float XSpeed = 1;   //起始的X轴速度
        private float YSpeed = 0;   //起始的Y轴速度
        private float g = 9.8f;

	    // Use this for initialization
	    void Start () {
            XSpeed = Speed * Mathf.Cos(Angle / 57.3f);  //计算初始X轴速度
            YSpeed = Speed * Mathf.Sin(Angle / 57.3f);  //计算初始Y轴速度
	    }
	
	    // Update is called once per frame
	    void Update () {
            this.transform.position = new Vector3((this.transform.position.x + XSpeed *     Time.deltaTime), this.transform.position.y + YSpeed * Time.deltaTime, this.transform.position.z);         //X轴位移
            YSpeed -= g * Time.deltaTime; // Y轴速度变化
	    }
}
```

方法三：
```
    #使用Transform方法
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PaoWuXian : MonoBehaviour {

        public float Speed = 1; //初速度
        public float Angle = 0;     //抛物角度，默认0度为水平向右，角度按逆时针方向增加
        private float XSpeed = 1;   //起始的X轴速度
        private float YSpeed = 0;   //起始的Y轴速度
        private float g = 9.8f;

	    // Use this for initialization
	    void Start () {
            XSpeed = Speed * Mathf.Cos(Angle / 57.3f);  //计算初始X轴速度
            YSpeed = Speed * Mathf.Sin(Angle / 57.3f);  //计算初始Y轴速度
	    }
	
	    // Update is called once per frame
	    void Update () {
            this.transform.Translate(Vector3.right * Time.deltaTime * XSpeed);  //X轴移动
            this.transform.Translate(Vector3.down * Time.deltaTime * YSpeed);   //Y轴移动
            YSpeed += g * Time.deltaTime;   //Y轴速度变化
	    }
```

- 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上
考虑到太阳系中的星体太多，本次实验我们只取太阳与七大行星进行模拟。
首先我们需要创建8个球体分别代表：太阳、水星、木星、地球……通过查询资料可得，太阳系中的行星与太阳的体积差异非常悬殊，为提供更直观的视觉感受，我们默认太阳与其他行星的体积比均为2:1，以便模拟。建立完星体后，我们便可以将公转的脚本挂载到各个行星上。
值得一提的是，题目中要求其他星球围绕太阳的转速必须不一样，经查阅资料，可知行星公转的线速度比为水星:金星:地球:火星:木星:土星:天王星:海王星 ≈ 47:35:30:24:13:9:6:5， 根据此比值设置各行星的线速度即可。
除此以外，题目还要求个星球的公转轨道不在同一个法平面上，经查阅资料了解，使各行星的公转轴心不在同一直线——或y/z的比值不同即可。
根据要求，可写出脚本如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class RevolutionScript : MonoBehaviour {

        public Transform Center;    //公转中心，默认为太阳的Position
        public float Speed; //公转线速度
        public float RotateSpeed;//自转角速度
        //围绕旋转轴的x,y,z值
        public float X;
        public float Y;
        public float Z;
        private Vector3 Axis;

        // Use this for initialization
        void Start () {
            Axis = new Vector3(X, Y, Z);
	    }
	
	    // Update is called once per frame
	    void Update () {
            transform.RotateAround(Center.position, Axis, Speed * Time.deltaTime);  //公转运动
            transform.Rotate(Vector3.up * 360 * Time.deltaTime / RotateSpeed);  //自转运动
        }
}
```

----------

## 2、编程实践
- 列出游戏中提及的事物(Object)
牧师、魔鬼、船、开始岸、结束岸

- 用表格列出玩家动作表（规则表），注意，动作越少越好
动作表:
    |操作|响应|条件|
    |:-:|:-:|:-:|
    |点击牧师/恶魔|牧师/恶魔上船|船和被点击的牧师/恶魔在同一个岸边&&船未满|
    |点击牧师/恶魔|牧师/恶魔下船|牧师/恶魔在船上&&船已靠岸|
    |点击船|船移动到对岸|船上至少有一个牧师/恶魔&&船已靠岸|

- 具体实现见[源码][1]，实现主要参考[这位很强的师兄][2]


  [1]: https://github.com/CarlSome/Unity3D/tree/master/Homework2/Priests%20and%20Devils/Priests%20and%20Devils/Assets/Script
  [2]: https://www.jianshu.com/p/07028b3da573
