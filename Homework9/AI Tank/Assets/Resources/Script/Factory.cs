using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    public GameObject player;
    public GameObject tank;
    public GameObject bullet;
    public ParticleSystem bulletExplode;

    private Dictionary<int, GameObject> usingTanks;
    private Dictionary<int, GameObject> freeTanks;
    private Dictionary<int, GameObject> usingBullets;
    private Dictionary<int, GameObject> freeBullets;
    private List<ParticleSystem> bulletExplodeList;

    void Awake()
    {
        usingTanks = new Dictionary<int, GameObject>();
        freeTanks = new Dictionary<int, GameObject>();
        usingBullets = new Dictionary<int, GameObject>();
        freeBullets = new Dictionary<int, GameObject>();
        bulletExplodeList = new List<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {
        Enemy.recycleEvent += recycleTank;
	}
	
	public GameObject getPlayer()
    {
        return player;
    }

    public GameObject getTank()
    {
        if(freeTanks.Count == 0)
        {
            GameObject newTank = Instantiate<GameObject>(tank);
            usingTanks.Add(newTank.GetInstanceID(), newTank);
            newTank.transform.position = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            return newTank;
        }
        foreach(KeyValuePair<int, GameObject> pair in freeTanks)
        {
            pair.Value.SetActive(true);
            freeTanks.Remove(pair.Key);
            usingTanks.Add(pair.Key, pair.Value);
            pair.Value.transform.position = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            return pair.Value;
        }
        return null;
    }

    public GameObject getBullet(tankType type)
    {
        if(freeBullets.Count == 0)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.GetComponent<Bullet>().setTankType(type);
            usingBullets.Add(newBullet.GetInstanceID(), newBullet);
            return newBullet;
        }
        
        foreach(KeyValuePair<int, GameObject> pair in freeBullets)
        {
            pair.Value.SetActive(true);
            pair.Value.GetComponent<Bullet>().setTankType(type);
            freeBullets.Remove(pair.Key);
            usingBullets.Add(pair.Key, pair.Value);
            return pair.Value;
        }

        return null;
    }

    public ParticleSystem GetExplosion()
    {
        for(int i=0; i<bulletExplodeList.Count;i++)
        {
            if(bulletExplodeList[i].isPlaying == false)
            {
                return bulletExplodeList[i];
            }
        }
        ParticleSystem newExplosion = Instantiate<ParticleSystem>(bulletExplode);
        bulletExplodeList.Add(newExplosion);
        return newExplosion;
    }

    public void recycleTank(GameObject tank)
    {
        usingTanks.Remove(tank.GetInstanceID());
        freeTanks.Add(tank.GetInstanceID(), tank);
        tank.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        tank.SetActive(false);
    }

    public void recycleBullet(GameObject bullet)
    {
        usingBullets.Remove(bullet.GetInstanceID());
        freeBullets.Add(bullet.GetInstanceID(), bullet);
        bullet.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        bullet.SetActive(false);
    }
}
