using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFactory : MonoBehaviour {

    List<UFOObject> usingList = new List<UFOObject>();
    List<UFOObject> freeList = new List<UFOObject>();
    GameObject UFOPrefab;

    /*
    private static UFOFactory instance;
    public static UFOFactory GetInstance()
    {
        if(instance == null)
        {
            instance = new UFOFactory();
        }
        return instance;
    }
    */

    private void Awake()
    {
        UFOPrefab = Object.Instantiate(Resources.Load("Prefabs/UFO", typeof(GameObject))) as GameObject;
        UFOPrefab.SetActive(false);
    }


    public GameObject getUFO(int round)
    {
        Debug.Log("生成一个飞碟");
        GameObject newUFO = null;
        if(freeList.Count > 0)
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

        //难度测试Demo
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

        switch (round)
        {

            case 0:
                {
                    newUFO.GetComponent<UFOObject>().SetUFOColor(Color.white);
                    float speed = UnityEngine.Random.Range(6f, 8f);
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
                    float speed = UnityEngine.Random.Range(8f, 10f);
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
                    float speed = UnityEngine.Random.Range(10f, 12f);
                    newUFO.GetComponent<UFOObject>().SetUFOSpeed(speed);
                    float RanX = UnityEngine.Random.Range(-1f, 1f);
                    float RanY = UnityEngine.Random.Range(0.5f, 2f);
                    newUFO.GetComponent<UFOObject>().SetUFODirection(new Vector3(RanX, RanY, 0));
                    newUFO.GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
        }


        usingList.Add(newUFO.GetComponent<UFOObject>());
        newUFO.name = newUFO.GetInstanceID().ToString();
        return newUFO;
    }

    public void freeUFO(GameObject UFO)
    {
        UFOObject tmpUFO = null;
        foreach(UFOObject i in usingList)
        {
            if(UFO.GetInstanceID() == i.gameObject.GetInstanceID())
            {
                tmpUFO = i;
            }
        }

        if(tmpUFO != null)
        {
            tmpUFO.gameObject.SetActive(false);
            freeList.Add(tmpUFO);
            freeList.Remove(tmpUFO);
        }
        /*
        foreach(UFOObject i in usingList)
        {
            if(i == UFO)
            {
                UFO.SetActive(false);
                usingList.Remove(UFO);
                freeList.Add(UFO);
            }
        }
        */
    }
}
