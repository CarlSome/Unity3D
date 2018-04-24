using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;
    private bool firstTime = true;
    private GUIStyle style;
    private GUIStyle buttonStyle;

    public ScoreController scoreController
    {
        get;
        set;
    }

    void Start () {
        action = Director.GetInstance().currentSceneController as IUserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;

        this.gameObject.AddComponent<ScoreController>();
        scoreController = Singleton<ScoreController>.Instance;
    }

    private void OnGUI()
    {
        //Debug.Log("GameMode = " + action.GetMode());
        if(action.GetMode() == GameMode.EMPTY)
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 120, 60), "Kinematic Mode"))
            {
                action.SetMode(GameMode.KINEMATIC);
            }
            if(GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height / 2 - 50, 120, 60), "Physic Mode"))
            {
                action.SetMode(GameMode.PHYSIC);
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                Vector3 position = Input.mousePosition;
                action.Shoot(position);
            }

            GUI.Label(new Rect(0, 0, 400, 400), "Score: " + action.GetScore().ToString());

            if (firstTime == true)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 60), "Start"))
                {
                    firstTime = false;
                    action.SetGameState(GameState.ROUND_START);
                }
            }

            if (firstTime == false && action.GetGameState() == GameState.ROUND_FINISH)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 60), "Next Round"))
                {
                    action.SetGameState(GameState.ROUND_START);
                }
            }

            if(GUI.Button(new Rect(Screen.width - 50, Screen.height - 30, 40, 20), "Back"))
            {
                firstTime = true;
                action.SetMode(GameMode.EMPTY);
                action.SetGameState(GameState.START);
                scoreController.Reset();
            }
        }
    }
}
