using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {

    private Dictionary<int, SSAction> registActions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingActions = new List<SSAction>();
    private List<int> toDeleteActions = new List<int>();

    protected void Start() {

    }

    protected void Update()
    {
        foreach(SSAction action in waitingActions)
        {
            registActions[action.GetInstanceID()] = action;
        }
        waitingActions.Clear();

        foreach(KeyValuePair<int, SSAction> keyValue in registActions)
        {
            SSAction action = keyValue.Value;
            if(action.destory == true)
            {
                toDeleteActions.Add(action.GetInstanceID());
            }
            else if(action.enable == true)
            {
                action.Update();
            }
        }

        foreach(int key in toDeleteActions)
        {
            SSAction action = registActions[key];
            registActions.Remove(key);
            DestroyObject(action);
        }
        toDeleteActions.Clear();
    }

    public void Act(SSAction action, GameObject _gameObject, ISSActionCallback _callback)
    {
        action.gameObject = _gameObject;
        action.transform = _gameObject.transform;
        action.callback = _callback;
        waitingActions.Add(action);
        action.Start();
    }
}
