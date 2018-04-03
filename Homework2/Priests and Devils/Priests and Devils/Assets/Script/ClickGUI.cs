using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnMouseDown()
    {
        if(gameObject.name == "ship")
        {
            action.moveShip();
        }
        else
        {
            action.characterIsClicked(characterController);
        }
    }
}
