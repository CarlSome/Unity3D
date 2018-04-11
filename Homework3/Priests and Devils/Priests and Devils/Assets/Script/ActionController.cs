using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public interface ActionCallback
    {
        void actionDone(ObjectAction source);
    }

    public class ObjectAction : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;

        public GameObject gameObject
        {
            get;
            set;
        }
        public Transform transform
        {
            get;
            set;
        }
        public ActionCallback callback
        {
            get;
            set;
        }

        protected ObjectAction() { }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }

    public class MoveAction : ObjectAction
    {
        public Vector3 destination;
        public float speed;

        public override void Start() {
            Debug.Log("MoveAction on");
        }

        public static MoveAction getAction(Vector3 _destination, float _speed)
        {
            MoveAction action = ScriptableObject.CreateInstance<MoveAction>();
            action.destination = _destination;
            action.speed = _speed;
            return action;
        }

        public override void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);
            if (this.transform.position == destination)
            {
                this.destroy = true;
                this.callback.actionDone(this);
            }
        }
    }

    public class SequenceAction : MoveAction, ActionCallback
    {
        public List<ObjectAction> sequence;
        public int repeat = 1;
        public int currentAction = 0;

        public override void Start()
        {
            foreach (ObjectAction action in sequence)
            {
                action.gameObject = this.gameObject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        public void actionDone(ObjectAction source)
        {
            source.destroy = false;
            this.currentAction++;
            if (this.currentAction >= sequence.Count)
            {
                this.currentAction = 0;
                //注意
                if (repeat > 0)
                {
                    repeat--;
                }
                if (repeat == 0)
                {
                    this.destroy = true;
                    this.callback.actionDone(this);
                }
            }
        }

        public static SequenceAction getAction(int _repeat, int _currentAction, List<ObjectAction> _sequence)
        {
            SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
            action.repeat = _repeat;
            action.currentAction = _currentAction;
            action.sequence = _sequence;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0)
            {
                return;
            }

            if (currentAction < sequence.Count)
            {
                sequence[currentAction].Update();
            }
        }

        void OnDestory()
        {
            foreach (ObjectAction action in sequence)
            {
                DestroyObject(action);
            }
        }
    }

    public class ActionManager : MonoBehaviour, ActionCallback
    {
        public Dictionary<int, ObjectAction> actions = new Dictionary<int, ObjectAction>();
        private List<ObjectAction> addList = new List<ObjectAction>();
        private List<int> deleteList = new List<int>();

        public void Start()
        {
            Debug.Log("ActionManager on");
        }
        public void actionDone(ObjectAction source) { }

        protected void Update()
        {
            foreach (ObjectAction action in addList)
            {
                actions[action.GetInstanceID()] = action;
            }
            addList.Clear();

            foreach (KeyValuePair<int, ObjectAction> keyValue in actions)
            {
                ObjectAction action = keyValue.Value;

                if (action.destroy)
                {
                    deleteList.Add(action.GetInstanceID());
                }
                else if (action.enable)
                {
                    action.Update();
                }
            }

            foreach (int key in deleteList)
            {
                ObjectAction action = actions[key];
                actions.Remove(key);
                DestroyObject(action);
            }
            deleteList.Clear();
        }

        public void addAction(GameObject _gameObject, ObjectAction _action, ActionCallback _callback)
        {
            _action.gameObject = _gameObject;
            _action.transform = gameObject.transform;
            _action.callback = _callback;
            _action.Start();
            addList.Add(_action);
        }
    }

    public class ActionList : ActionManager
    {
        public void moveShip(ShipController ship)
        {
            if (ship.isEmpty())
            {
                return;
            }

            MoveAction action = MoveAction.getAction(ship.getDestination(), 20.0f);
            this.addAction(ship.getShip(), action, this);
        }

        public void moveCharacter(ICharacterController characterController, Vector3 destination)
        {
            Debug.Log("Trying to move the character");
            Vector3 currentPosition = characterController.getPosition();
            Vector3 targetPosition = characterController.getPosition();
            List<ObjectAction> actionList = new List<ObjectAction>();

            if (destination.y > currentPosition.y)
            {
                targetPosition.y = destination.y;
            }
            else
            {
                targetPosition.x = destination.x;
            }
            ObjectAction action1 = MoveAction.getAction(targetPosition, 20f);
            actionList.Add(action1);
            ObjectAction action2 = MoveAction.getAction(destination, 20f);
            actionList.Add(action2);
            ObjectAction actions = SequenceAction.getAction(1, 0, actionList);

            this.addAction(characterController.getGameObject(), actions, this);
        }
    }
}




