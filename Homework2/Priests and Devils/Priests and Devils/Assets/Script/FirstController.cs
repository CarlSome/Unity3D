using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    UserGUI userGUI;

    public BankController beginBank;
    public BankController endBank;
    public ShipController ship;
    public ICharacterController[] characters;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new ICharacterController[6];
        loadResources();
    }

    public void loadResources()
    {
        ship = new ShipController();
        beginBank = new BankController("begin");
        endBank = new BankController("end");

        for(int i=0;i<3;i++)
        {
            ICharacterController character = new ICharacterController("priest");
            character.setName("priest" + i);
            character.setPosition(beginBank.getEmptyPosition());
            character.getOnBank(beginBank);
            beginBank.getOnBank(character);

            characters[i] = character;
        }
        for(int i=3;i<6;i++)
        {
            ICharacterController character = new ICharacterController("devil");
            character.setName("devil" + i);
            character.setPosition(beginBank.getEmptyPosition());
            character.getOnBank(beginBank);
            beginBank.getOnBank(character);

            characters[i] = character;
        }
        
    }

    public int getGameStatus()
    {
        int beginPriest = 0;
        int beginDevil = 0;
        int endPriest = 0;
        int endDevil = 0;

        int[] beginCount = beginBank.getCharacterNum();
        beginPriest += beginCount[0];
        beginDevil += beginCount[1];

        int[] endCount = endBank.getCharacterNum();
        endPriest += endCount[0];
        endDevil += endCount[1];

        if (endPriest + endDevil == 6)
        {
            //Debug.Log("全部到对岸");
            return 1;
        }

        int[] shipCount = ship.getCharacterNum();
        if(ship.getDirection() == -1)
        {
            endPriest += shipCount[0];
            endDevil += shipCount[1];
        }
        else
        {
            beginPriest += shipCount[0];
            beginDevil += shipCount[1];
        }

        if(beginPriest < beginDevil && beginPriest > 0)
        {
            //Debug.Log("右边爆炸");
            return -1;
        }
        if(endPriest < endDevil && endPriest > 0)
        {
            //Debug.Log("左边爆炸");
            return -1;
        }
        return 0;
    }

    public void moveShip()
    {
        if(userGUI.status != 0)
        {
            return;
        }

        if(ship.isEmpty())
        {
            return;
        }

        ship.Move();
        userGUI.status = getGameStatus();
    }

    public void characterIsClicked(ICharacterController characterController)
    {
        if(characterController.IsOnShip())
        {
            BankController whichBank;
            if(ship.getDirection() == -1)
            {
                whichBank = endBank;
            }
            else
            {
                whichBank = beginBank;
            }

            ship.getOffShip(characterController.getName());
            characterController.moveToPosition(whichBank.getEmptyPosition());
            characterController.getOnBank(whichBank);
            whichBank.getOnBank(characterController);
        }
        else
        {
            BankController whichBank = characterController.getBankController();
            
            if(ship.isFull())
            {
                return;
            }

            if(whichBank.getSide() != ship.getDirection())
            {
                return;
            }

            whichBank.getOffBank(characterController.getName());
            characterController.moveToPosition(ship.getEmptyPosition());
            characterController.getOnShip(ship);
            ship.getOnShip(characterController);
        }
        userGUI.status = getGameStatus();
    }

    public void restart()
    {
        ship.reset();
        beginBank.reset();
        endBank.reset();
        for (int i=0;i<characters.Length;i++)
        {
            characters[i].reset();
        }
    }
}
