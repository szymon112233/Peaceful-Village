using UnityEngine;
using NPBehave;

public class BTreeTankRandom : MonoBehaviour
{
    private Blackboard blackboard;
    private Root behaviorTree;

    public TankControllerBTree controller;

    public float decisionTime = 1.5f;

    void Start()
    {
        // create our behaviour tree and get it's blackboard
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;

        

        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif

        // start the behaviour tree
        behaviorTree.Start();
    }

    private Root CreateBehaviourTree()
    {
        // we always need a root node
        return new Root(
            new Service(decisionTime, () => { behaviorTree.Blackboard["shouldUpdate"] = !behaviorTree.Blackboard.Get<bool>("shouldUpdate"); },
                new Selector(
                    new BlackboardCondition("shouldUpdate", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,
                        new Sequence(

                            new Action(() =>
                            {
                                Vector2 vector = new Vector2();
                                if (UnityEngine.Random.value > 0.5f)
                                {
                                    vector = new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f);
                                }
                                else
                                {
                                    vector = new Vector2(0.0f, UnityEngine.Random.Range(-1.0f, 1.0f));
                                }
                                SetMoveVector(vector);
                                

                                behaviorTree.Blackboard["shouldUpdate"] = false;

                            }) { Label = "Change inputs" },
                            new WaitUntilStopped()
                        )
                    ),

                    // when 'toggled' is false, we'll eventually land here
                    new Sequence(
                        new Action(() => SetShoot(UnityEngine.Random.value < 0.1f)),
                        new WaitUntilStopped()
                    )
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
}
