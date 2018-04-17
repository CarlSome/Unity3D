using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    bool firstTime = true;
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
        if(Input.GetButton("Fire1"))
        {
            Vector3 position = Input.mousePosition;
            action.Shoot(position);
        }

        if(firstTime == true)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 60), "Start"))
            {
                firstTime = false;
                action.SetGameState(GameState.ROUND_START);
            }
        }

        if(firstTime == false && action.GetGameState() == GameState.ROUND_FINISH)
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 60), "Next Round"))
            {
                action.SetGameState(GameState.ROUND_START);
            }
        }

        GUI.Label(new Rect(0, 0, 400, 400), "Score: " + action.GetScore().ToString());
    }
}
