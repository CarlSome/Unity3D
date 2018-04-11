using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    public int status = 0;  //0-进行中 -1-失败 1-胜利
    GUIStyle style;
    GUIStyle buttonStyle;

	// Use this for initialization
	void Start () {
        action = SSDirector.getInstance().currentSceneController as IUserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
	}

    private void OnGUI()
    {
        GUI.Box(new Rect(10 , 10, 100, 80), "Introduction");
        GUI.Label(new Rect(20, 30, 80, 20), "白球代表牧师");
        GUI.Label(new Rect(20, 50, 80, 20), "黑球代表魔鬼");
        if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Winner Winner, Chiken Dinner!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 55, Screen.height / 2 -20 , 110, 40), "Restart", buttonStyle))
            {
                status = 0;
                action.restart();
            }
        }
        else if(status == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Loser Loser, Try It Later!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 20, 110, 40), "Restart", buttonStyle))
            {
                status = 0;
                action.restart();
            }
        }
    }
}
