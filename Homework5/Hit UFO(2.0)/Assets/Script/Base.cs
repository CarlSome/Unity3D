﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场记接口
public interface ISceneController
{
    void LoadResource();
}

public enum GameState
{
    START,
    PAUSE,
    PLAYING,
    ROUND_START,
    ROUND_FINISH
}
public enum GameMode
{
    PHYSIC,
    KINEMATIC,
    EMPTY
}
//用户动作接口
public interface IUserAction
{
    void SetGameState(GameState _gamestate);  //_gamestate: 0-游戏开始 1-游戏暂停 2-游戏结束 3-回合开始 4-回合结束 
    GameState GetGameState();
    void SetMode(GameMode mode);
    GameMode GetMode();
    int GetScore();
    void Shoot(Vector3 position);
}

//动作管理接口
public interface IActionManager
{
    void ReadyLaunch(Queue<GameObject> UFOList);
    void setUFONumber(int number);
    int getUFONumber();
}

//通信控制器接口
public enum SSActionEventType : int
{
    Started,
    Competeted
}
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParamameter = 0,
        string strParameter = null,
        Object objectParameter = null);

}

//导演单例
public class Director : System.Object
{
    private static Director instance;
    public ISceneController currentSceneController
    {
        get;
        set;
    }

    public static Director GetInstance()
    {
        if(instance == null)
        {
            instance = new Director();
        }
        return instance;
    }
}
