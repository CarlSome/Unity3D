using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    /*
    private IUserAction action;
    GUIStyle style;
    GUIStyle buttonStyle;

	void Start () {
        action = Director.GetInstance().currentSceneController as IUserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
    }

    private void OnGUI()
    {
        
    }*/
    private IUserAction action;
    bool isFirst = true;
    // Use this for initialization  
    void Start()
    {
        action = Director.GetInstance().currentSceneController as IUserAction;

    }

    private void OnGUI()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            Vector3 pos = Input.mousePosition;
            action.Shoot(pos);

        }

        GUI.Label(new Rect(1000, 0, 400, 400), action.GetScore().ToString());

        if (isFirst && GUI.Button(new Rect(700, 100, 90, 90), "Start"))
        {
            isFirst = false;
            action.SetGameState(GameState.ROUND_START);
        }

        if (!isFirst && action.GetGameState() == GameState.ROUND_FINISH && GUI.Button(new Rect(700, 100, 90, 90), "Next Round"))
        {
            action.SetGameState(GameState.ROUND_START);
        }

    }
}
