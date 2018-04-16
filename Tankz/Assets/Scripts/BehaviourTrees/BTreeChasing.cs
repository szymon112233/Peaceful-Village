using UnityEngine;
using NPBehave;

public class BTreeChasing : MonoBehaviour
{
    private Blackboard blackboard;
    private Root behaviorTree;
    private Tank tankComponent;

    public TankControllerBTree controller;

    void Start()
    {
        // create our behaviour tree and get it's blackboard
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;
        blackboard["shouldMove"] = true;
        tankComponent = GetComponent<Tank>();


        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif

        // start the behaviour tree
        behaviorTree.Start();
        SetShoot(true);
    }

    private Root CreateBehaviourTree()
    {
        // we always need a root node
        return new Root(
                    new Sequence(
                            // Moving
                            new Action(() =>
                            {
                                Vector2 vector, direction;
                                if (GameManager.instance.gamestate.tanksList[0] != tankComponent)
                                    direction = GameManager.instance.gamestate.tanksList[0].transform.position - transform.position;
                                else
                                    direction = GameManager.instance.gamestate.tanksList[1].transform.position - transform.position;

                                foreach (Tank tank in GameManager.instance.gamestate.tanksList)
                                {
                                    if (tank != tankComponent)
                                        if ((vector = tank.transform.position - transform.position).sqrMagnitude < direction.sqrMagnitude)
                                            direction = vector;
                                }

                                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                    vector = new Vector2(0f, direction.y);
                                else
                                    vector = new Vector2(direction.x, 0f);

                                if (Mathf.Abs(direction.x) <= 1 || Mathf.Abs(direction.y) <= 1)
                                {
                                    blackboard["shouldMove"] = false;
                                    blackboard["direction"] = direction;
                                }
                                SetMoveVector(vector);
                            })
                            { Label = "Change inputs" },
                            // Rotating
                            new BlackboardCondition("shouldMove", Operator.IS_EQUAL, false, Stops.NONE,
                                new Action(() =>
                                {
                                    Vector2 vector, direction = (Vector2)blackboard["direction"];
                                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                        vector = new Vector2(direction.x, 0f);
                                    else
                                        vector = new Vector2(0f, direction.y);
                                    SetMoveVector(vector);
                                    blackboard["shouldMove"] = true;
                                })
                            )
                    )
          );
    }

    private void SetMoveVector(Vector2 moveVector)
    {
        controller.SetMoveVector(moveVector);
    }

    private void SetShoot(bool value)
    {
        controller.SetShoot(value);
    }

    private void OnDestroy()
    {
        behaviorTree.Stop();
    }
}
