using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class ClickGUI : MonoBehaviour {
    IUserAction action;
    ICharacterController characterController;

    public void setController(ICharacterController _characterController)
    {
        characterController = _characterController;
    }

    // Use this for initialization
    void Start () {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
	}

    void OnMouseDown()
    {
        if(gameObject.name == "ship")
        {
            //Debug.Log("Ship is clicked");
            action.moveShip();
        }
        else
        {
            //Debug.Log("Character is clicked");
            action.characterIsClicked(characterController);
        }
    }
}
