using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicActionManager : MonoBehaviour, IActionManager, ISSActionCallback {

    public FirstController firstController;
    public int UFONumber = 0;

    private List<SSAction> waitingList = new List<SSAction>();
    private List<int> freeList = new List<int>();
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();

    protected void Start()
    {
        firstController = (FirstController)Director.GetInstance().currentSceneController;
        firstController.actionManager = this;
    }

    protected void FixedUpdate()
    {
        //注册动作
        foreach (SSAction action in waitingList)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitingList.Clear();

        //管理所有动作: 根据UFO状态激活与删除
        foreach (KeyValuePair<int, SSAction> keyValuePair in actions)
        {
            SSAction action = keyValuePair.Value;

            if (action.destory == true)
            {
                freeList.Add(action.GetInstanceID());
            }
            else if (action.enable == true)
            {
                action.FixedUpdate();
            }
        }

        //清除删除队列中的动作
        foreach (int key in freeList)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
        }
        freeList.Clear();
    }

    public void SetAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = callback;

        waitingList.Add(action);
        action.Start();
    }

    public void setUFONumber(int number)
    {
        UFONumber = number;
    }

    public int getUFONumber()
    {
        return UFONumber;
    }

    public void ReadyLaunch(Queue<GameObject> UFOQueue)
    {
        UFOScriptFactory scriptFactory = Singleton<UFOScriptFactory>.Instance;
        foreach (GameObject UFO in UFOQueue)
        {
            SetAction(UFO, scriptFactory.GetSSAction(), (ISSActionCallback)this);
        }
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParamameter = 0,
        string strParameter = null,
        Object objectParameter = null)
    {
        if (source is UFOAction)
        {
            UFONumber--;
            source.gameObject.SetActive(false);
        }
    }
}
