using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAction : SSAction {

    public float xSpeed;
    public Vector3 direction;

    private float gravity;
    private float time;
    private Rigidbody rigidbody;

    private IUserAction Iaction;

    public override void Start()
    {
        Iaction = Director.GetInstance().currentSceneController as IUserAction;

        gravity = 9.8f;
        enable = true;
        time = 0;
        xSpeed = gameObject.GetComponent<UFOObject>().GetUFOSpeed();
        direction = gameObject.GetComponent<UFOObject>().GetUFODirection();

        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if(rigidbody != null)
        {
            rigidbody.velocity = xSpeed * direction;
        }
    }

    public static UFOAction GetSSAction()
    {
        UFOAction action = ScriptableObject.CreateInstance<UFOAction>();
        return action;
    }

    public override void Update()
    {
        if(gameObject.activeSelf && Iaction.GetMode() != GameMode.EMPTY)
        {
            time += Time.deltaTime;
            transform.Translate(direction * xSpeed * Time.deltaTime);
            transform.Translate(Vector3.down * gravity * time * Time.deltaTime);

            if (this.transform.position.y < -5)
            {
                this.destory = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public override void FixedUpdate()
    {
        if(gameObject.activeSelf)
        {
            if(this.transform.position.y < -5)
            {
                this.destory = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public static UFOAction GetUFOAction()
    {
        UFOAction action = ScriptableObject.CreateInstance<UFOAction>();
        return action;
    }
}
