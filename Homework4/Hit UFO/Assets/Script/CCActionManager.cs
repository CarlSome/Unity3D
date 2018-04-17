using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{

    public FirstController firstController;
    public List<UFOAction> waitingList;
    public int UFONumber = 0;
    private List<SSAction> usingList = new List<SSAction>();
    private List<SSAction> freeList = new List<SSAction>();

    protected new void Start()
    {
        firstController = (FirstController)Director.GetInstance().currentSceneController;
        firstController.actionManager = this;
        waitingList.Add(UFOAction.GetSSAction());
        //UFONumber = 0;
        //usingList = new List<SSAction>();
       // freeList = new List<SSAction>();
    }

    public SSAction GetSSAction()
    {
        SSAction action = null;
        if(freeList.Count > 0)
        {
            action = freeList[0];
            freeList.Remove(freeList[0]);
        }
        else
        {
            action = ScriptableObject.Instantiate(waitingList[0]) as UFOAction ;
        }

        usingList.Add(action);

        return action;
    }

    public void FreeSSAction(SSAction action)
    {
        foreach(SSAction i in usingList)
        {
            if(i.GetInstanceID() == action.GetInstanceID())
            {
                action.Reset();
                freeList.Add(action);
                usingList.Remove(action);
            }
        }
    }

    public void RefillUFO()
    {
        UFONumber = 10;
    }

    public void ReadyLaunch(Queue<GameObject> UFOQueue)
    {
        foreach(GameObject i in UFOQueue)
        {
            Act(GetSSAction(), i, (ISSActionCallback)this);
        }
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParamameter = 0,
        string strParameter = null,
        Object objectParameter = null)
    {
        if(source is UFOAction)
        {
            UFONumber--;
            UFOFactory factory = Singleton<UFOFactory>.Instance;
            factory.freeUFO(source.gameObject);
            FreeSSAction(source);
        }
    }

}
