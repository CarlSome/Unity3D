using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAction : SSAction {

    private float gravity;
    private float time;
    public float xSpeed;
    public Vector3 direction;

    public override void Start()
    {
        gravity = 9.8f;
        enable = true;
        time = 0;
        xSpeed = gameObject.GetComponent<UFOObject>().GetUFOSpeed();
        direction = gameObject.GetComponent<UFOObject>().GetUFODirection();
    }

    public static UFOAction GetSSAction()
    {
        UFOAction action = ScriptableObject.CreateInstance<UFOAction>();
        return action;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        transform.Translate(direction * xSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * gravity * time * Time.deltaTime);

        if(this.transform.position.y < 0)
        {
            this.destory = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }
}
