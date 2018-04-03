using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                        
//导演单例
public class SSDirector : System.Object
{
    private static SSDirector _instance;
    public ISceneController currentSceneController
    {
        get;
        set;
    }

    public static SSDirector getInstance()
    {
        if(_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }
}

//场景控制器
public interface ISceneController
{
    void loadResources();
}

//用户界面与游戏模型的交互接口
public interface IUserAction
{
    void characterIsClicked(ICharacterController characterController);
    void moveShip();
    void restart();
}

public class MoveController : MonoBehaviour
{
    float speed = 20f;
    bool isMoving = false;
    Vector3 destination;

    void Update()
    {
        if(isMoving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        if(transform.position == destination)
        {
            isMoving = false;
        }
    }

    public void setDestination(Vector3 _destination)
    {
        destination = _destination;
        isMoving = true;
    }

    public void reset()
    {
        isMoving = false;
    }
}

public class BankController
{
    readonly GameObject bank;
    readonly Vector3 beginPosition = new Vector3(7, 0, 0);
    readonly Vector3 endPosition = new Vector3(-7, 0, 0);
    readonly Vector3[] positions;
    enum Bank
    {
        begin,
        end
    };
    readonly Bank whichBank;

    ICharacterController[] passengerList;

    public BankController(string _whichBank)
    {
        positions = new Vector3[]
        {
            new Vector3(3.5f, 1.5f, 0),
            new Vector3(5f, 1.5f, 0),
            new Vector3(6.5f, 1.5f, 0),
            new Vector3(8f, 1.5f, 0),
            new Vector3(9.5f, 1.5f, 0),
            new Vector3(11f, 1.5f, 0),
        };

        passengerList = new ICharacterController[6];

        if(_whichBank == "begin")
        {
            bank = Object.Instantiate(Resources.Load("Prefabs/Bank", typeof(GameObject)), beginPosition, Quaternion.identity, null) as GameObject;
            bank.name = "beginBank";
            whichBank = Bank.begin;
        }
        else
        {
            bank = Object.Instantiate(Resources.Load("Prefabs/Bank", typeof(GameObject)), endPosition, Quaternion.identity, null) as GameObject;
            bank.name = "endBank";
            whichBank = Bank.end;
        }
    }

    public Vector3 getEmptyPosition()
    {
        int index = -1;
        for (int i = 0; i < passengerList.Length; i++)
        {
            if (passengerList[i] == null)
            {
                index = i;
                break;
            }
        }

        Vector3 _positions = positions[index];

        if(whichBank == Bank.end)
        {
            _positions.x = -_positions.x;
        }

        return _positions;
    }

    public void getOnBank(ICharacterController charactercontroller)
    {
        int index = -1;
        for (int i = 0; i < passengerList.Length; i++)
        {
            if (passengerList[i] == null)
            {
                index = i;
                break;
            }
        }
        passengerList[index] = charactercontroller;
    }

    public ICharacterController getOffBank(string passengerName)
    {
        for(int i=0;i<passengerList.Length;i++)
        {
            if(passengerList[i] != null && passengerList[i].getName() == passengerName)
            {
                ICharacterController characterController = passengerList[i];
                passengerList[i] = null;
                return characterController;
            }
        }
        return null;
    }

    public int[] getCharacterNum()
    {
        int[] count = {0, 0};
        for(int i=0;i<passengerList.Length;i++)
        {
            if(passengerList[i] == null)
            {
                continue;
            }
            
            if(passengerList[i].getType() == 0)
            {
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public int getSide()
    {
        if(whichBank == Bank.begin)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void reset()
    {
        passengerList = new ICharacterController[6];
    }
}

public class ShipController
{
    readonly public GameObject ship;
    readonly MoveController moveControllerScript;
    readonly Vector3 beginPosition = new Vector3(1.5f, 0.3f, 0);
    readonly Vector3 endPosition = new Vector3(-1.5f, 0.3f, 0);
    readonly Vector3[] beginPositions;
    readonly Vector3[] endPositions;
    ICharacterController[] passengerList = new ICharacterController[2];
    enum direction
    {
        left,
        right
    };
    direction moveDirection;

    public ShipController()
    {
        moveDirection = direction.left;

        beginPositions = new Vector3[]
        {
            new Vector3(0.75f, 0.9f, 0),
            new Vector3(2.25f, 0.9f, 0)
        };
        endPositions = new Vector3[]
        {
            new Vector3(-2.25f, 0.9f, 0),
            new Vector3(-0.75f, 0.9f, 0)
        };

        ship = Object.Instantiate(Resources.Load("Prefabs/ship", typeof(GameObject)), beginPosition, Quaternion.identity, null) as GameObject;
        ship.name = "ship";

        moveControllerScript = ship.AddComponent(typeof(MoveController)) as MoveController;
        ship.AddComponent(typeof(ClickGUI));
    }

    public void Move()
    {
        if(moveDirection == direction.right)
        {
            moveControllerScript.setDestination(beginPosition);
            moveDirection = direction.left;
        }
        else
        {
            moveControllerScript.setDestination(endPosition);
            moveDirection = direction.right;
        }
    }

    public bool isEmpty()
    {
        for(int i=0;i<passengerList.Length;i++)
        {
            if(passengerList[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool isFull()
    {
        int index = -1;
        for (int i = 0; i < passengerList.Length; i++)
        {
            if (passengerList[i] == null)
            {
                index = i;
            }
        }
        if(index == -1)
        {
            return true;
        }
        return false;
    }

    public Vector3 getEmptyPosition()
    {
        Vector3 position;
        int index = -1;
        for (int i=0;i<passengerList.Length;i++)
        {
            if (passengerList[i] == null)
            {
                index = i;
            }
        }
        
        if(moveDirection == direction.right)
        {
            position = endPositions[index];
        }
        else
        {
            position = beginPositions[index];
        }

        return position;
    }

    public void getOnShip(ICharacterController characterController)
    {
        int index = -1;
        for (int i = 0; i < passengerList.Length; i++)
        {
            if (passengerList[i] == null)
            {
                index = i;
            }
        }

        passengerList[index] = characterController;
    }

    public ICharacterController getOffShip(string passengerName)
    {
        for(int i=0;i<passengerList.Length;i++)
        {
            if(passengerList[i] != null && passengerList[i].getName() == passengerName)
            {
                ICharacterController characterController = passengerList[i];
                passengerList[i] = null;
                return characterController;
            }
        }
        return null;
    }

    public GameObject getShip()
    {
        return ship;
    }

    public int getDirection()
    {
        if(moveDirection == direction.left)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public int[] getCharacterNum()
    {
        int[] count = { 0, 0 };
        for(int i=0;i<passengerList.Length;i++)
        {
            if(passengerList[i] == null)
            {
                continue;
            }
            if(passengerList[i].getType() == 0)
            {
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public void reset()
    {
        moveControllerScript.reset();
        if (moveDirection == direction.right)
        {
            Move();
        }
        passengerList = new ICharacterController[2];
    }
}

public class ICharacterController
{
    private readonly GameObject character;
    private readonly MoveController moveControllerScript;
    private readonly ClickGUI clickGUI;
    enum CharacterType
    {
        priest,
        devil
    };
    private readonly CharacterType characterType;

    private bool isOnShip;
    BankController bankController;

    public ICharacterController(string _characterType)
    {
        if(_characterType == "priest")
        {
            character = Object.Instantiate(Resources.Load("Prefabs/Priests", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = CharacterType.priest;
        }
        else
        {
            character = Object.Instantiate(Resources.Load("Prefabs/Devils", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = CharacterType.devil;
        }

        moveControllerScript = character.AddComponent(typeof(MoveController)) as MoveController;

        clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController(this);
    }

    public void setName(string _name)
    {
        character.name = _name;
    }

    public void setPosition(Vector3 _position)
    {
        character.transform.position = _position;
    }

    public void moveToPosition(Vector3 _destination)
    {
        moveControllerScript.setDestination(_destination);
    }

    public bool IsOnShip()
    {
        return isOnShip;
    }

    public int getType()
    {
        if(characterType == CharacterType.priest)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public string getName()
    {
        return character.name;
    }

    public BankController getBankController()
    {
        return bankController;
    }

    public void getOnShip(ShipController shipController)
    {
        bankController = null;
        character.transform.parent = shipController.getShip().transform;
        isOnShip = true;
    }

    public void getOnBank(BankController _bankController)
    {
        bankController = _bankController;
        character.transform.parent = null;
        isOnShip = false;
    }

    public void reset()
    {
        bankController = (SSDirector.getInstance().currentSceneController as FirstController).beginBank;
        getOnBank(bankController);
        setPosition(bankController.getEmptyPosition());
        bankController.getOnBank(this);
        moveControllerScript.reset();
    }
}
