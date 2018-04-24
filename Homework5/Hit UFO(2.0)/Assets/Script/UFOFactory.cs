using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFactory : MonoBehaviour
{

    public GameObject UFOPrefab;

    private List<int> waitingList = new List<int>();
    private List<UFOObject> freeList = new List<UFOObject>();
    private Dictionary<int, UFOObject> usingList = new Dictionary<int, UFOObject>();

    private void Awake()
    {
        UFOPrefab = Object.Instantiate(Resources.Load("Prefabs/UFO", typeof(GameObject))) as GameObject;
        UFOPrefab.SetActive(false);
    }

    private void Update()
    {
        //检查正在使用的UFO中已失效的部分
        foreach (var UFO in usingList.Values)
        {
            if (UFO.gameObject.activeSelf == false)
            {
                waitingList.Add(UFO.GetInstanceID());
            }
        }

        //将失效的UFO加入删除队列
        foreach (int UFONumber in waitingList)
        {
            FreeUFO(usingList[UFONumber].gameObject);
        }
        waitingList.Clear();
    }

    //根据回合数和游戏模式生成UFO
    public GameObject GetUFO(int round, GameMode mode)
    {
        //Debug.Log("生成一个飞碟");
        GameObject newUFO = null;

        //优先从删除队列中回收利用UFO，若删除队列为空再新建UFO
        if (freeList.Count > 0)
        {
            newUFO = freeList[0].gameObject;
            freeList.Remove(freeList[0]);
        }
        else
        {
            newUFO = Object.Instantiate(Resources.Load("Prefabs/UFO", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            newUFO.AddComponent<UFOObject>();
            newUFO.SetActive(false);
        }

        /*
         * 难度控制Demo，需要独立出来
         */
        int start = 0;
        if (round == 1) start = 100;
        if (round == 2) start = 250;
        int selectedColor = Random.Range(start, round * 499);

        if (selectedColor > 500)
        {
            round = 2;
        }
        else if (selectedColor > 300)
        {
            round = 1;
        }
        else
        {
            round = 0;
        }

        UFOObject UFOProperty = newUFO.GetComponent<UFOObject>();
        switch (round)
        {
            
            case 0:
                {
                    newUFO.GetComponent<UFOObject>().SetUFOColor(Color.white);
                    float speed = UnityEngine.Random.Range(2f, 4f);
                    newUFO.GetComponent<UFOObject>().SetUFOSpeed(speed);
                    float RanX = UnityEngine.Random.Range(-1f, 1f);
                    float RanY = UnityEngine.Random.Range(0.5f, 2f);
                    newUFO.GetComponent<UFOObject>().SetUFODirection(new Vector3(RanX, RanY, 0));
                    newUFO.GetComponent<Renderer>().material.color = Color.white;
                    break;
                }
            case 1:
                {
                    newUFO.GetComponent<UFOObject>().SetUFOColor(Color.yellow);
                    float speed = UnityEngine.Random.Range(4f, 6f);
                    newUFO.GetComponent<UFOObject>().SetUFOSpeed(speed);
                    float RanX = UnityEngine.Random.Range(-1f, 1f);
                    float RanY = UnityEngine.Random.Range(0.5f, 2f);
                    newUFO.GetComponent<UFOObject>().SetUFODirection(new Vector3(RanX, RanY, 0));
                    newUFO.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                }
            case 2:
                {
                    newUFO.GetComponent<UFOObject>().SetUFOColor(Color.red);
                    float speed = UnityEngine.Random.Range(6f, 8f);
                    newUFO.GetComponent<UFOObject>().SetUFOSpeed(speed);
                    float RanX = UnityEngine.Random.Range(-1f, 1f);
                    float RanY = UnityEngine.Random.Range(0.5f, 2f);
                    newUFO.GetComponent<UFOObject>().SetUFODirection(new Vector3(RanX, RanY, 0));
                    newUFO.GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
        }

        //若当前游戏模式为物理模式，需要为飞碟添加刚体
        if (mode == GameMode.PHYSIC)
        {
            newUFO.AddComponent<Rigidbody>();
        }

        usingList.Add(newUFO.GetComponent<UFOObject>().GetInstanceID(), newUFO.GetComponent<UFOObject>());
        newUFO.name = newUFO.GetInstanceID().ToString();
        return newUFO;
    }

    public void FreeUFO(GameObject UFO)
    {
        UFOObject tmpUFO = null;
        foreach (UFOObject UFOProperty in usingList.Values)
        {
            if (UFO.GetInstanceID() == UFOProperty.gameObject.GetInstanceID())
            {
                tmpUFO = UFOProperty;
            }
        }

        if (tmpUFO != null)
        {
            tmpUFO.gameObject.SetActive(false);
            freeList.Add(tmpUFO);
            usingList.Remove(tmpUFO.GetInstanceID());
        }
    }
}


public class UFOScriptFactory : MonoBehaviour
{
    public UFOAction UFOActionScript;

    private Dictionary<int, SSAction> usingList = new Dictionary<int, SSAction>();
    private List<SSAction> freeList = new List<SSAction>();
    private List<int> waitingList = new List<int>();

    private void Start()
    {
        UFOActionScript = UFOAction.GetUFOAction();
    }

    private void Update()
    {
        //添加动作
        foreach (var action in usingList.Values)
        {
            if (action.destory == true)
            {
                waitingList.Add(action.GetInstanceID());
            }
        }

        //删除动作
        foreach (var action in waitingList)
        {
            FreeSSAction(usingList[action]);
        }
        waitingList.Clear();
    }

    public SSAction GetSSAction()
    {
        SSAction action = null;

        if (freeList.Count > 0)
        {
            action = freeList[0];
            freeList.Remove(freeList[0]);
        }
        else
        {
            action = ScriptableObject.Instantiate(UFOActionScript) as UFOAction;
        }

        usingList.Add(action.GetInstanceID(), action);

        return action;
    }

    public void FreeSSAction(SSAction action)
    {
        SSAction tmpAction = null;
        int actionKey = action.GetInstanceID();
       
        if (usingList.ContainsKey(actionKey))
        {
            tmpAction = usingList[actionKey];
            tmpAction.Reset();
            freeList.Add(tmpAction);
            usingList.Remove(actionKey);
        }
    }

    public void Clear()
    {
        foreach (var action in usingList.Values)
        {
            action.enable = false;
            action.destory = true;
        }
    }
}