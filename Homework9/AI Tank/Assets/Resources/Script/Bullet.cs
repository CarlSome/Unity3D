using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float explosionRadius = 3f;
    private tankType type;

    public void setTankType(tankType _type)
    {
        type = _type;
    }

    public void OnCollisionEnter(Collision collision)
    {
        ///Debug.Log("Collide");
        Factory factory = Singleton<Factory>.Instance;
        ParticleSystem explosion = factory.GetExplosion();
        explosion.transform.position = transform.position;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        for(int i=0;i<colliders.Length;i++)
        {
            if(colliders[i].tag == "Player" && this.type == tankType.ENEMY || colliders[i].tag == "Enemy" && this.type == tankType.PLAYER)
            {
                //Debug.Log("HIT");
                float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
                float damage = 100f / distance;
                float health = colliders[i].GetComponent<Tank>().getHealth();
                colliders[i].GetComponent<Tank>().setHealth(health - damage);
            }
        }
        explosion.Play();
        if(gameObject.activeSelf)
        {
            factory.recycleBullet(gameObject);
        }
    }
}
