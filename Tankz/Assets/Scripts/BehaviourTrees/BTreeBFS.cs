using UnityEngine;
using NPBehave;
using System.Collections.Generic;

public class BTreeBFS : MonoBehaviour
{
    private Blackboard blackboard;
    private Root behaviorTree;
    private Tank myTankComponent, target;
    private List<Vector3> path;
    private int currentNode = 0;
    private const float e = .775f;

    public TankControllerBTree controller;

    void Start()
    {
        // create our behaviour tree and get it's blackboard
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;
        blackboard["shouldMove"] = true;
        blackboard["isTarget"] = false;
        myTankComponent = GetComponent<Tank>();


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
        return new Root(
            new Selector(
                // Pathfinding
                new BlackboardCondition("isTarget", Operator.IS_EQUAL, false, Stops.SELF,
                    new Action(() =>
                    {
                        if (GameManager.instance.gamestate.tanksList[0] != myTankComponent)
                            target = GameManager.instance.gamestate.tanksList[0];
                        else
                            target = GameManager.instance.gamestate.tanksList[1];

                        
                        foreach (Tank tank in GameManager.instance.gamestate.tanksList)
                            if (tank != myTankComponent)
                                if ((tank.transform.position - transform.position).sqrMagnitude
                                < (target.transform.position - transform.position).sqrMagnitude)
                                    target = tank;
                        
                        path = BFSSearch.Search(myTankComponent, target.Node);
                        //path.ForEach(x => Debug.Log(x));
                        blackboard["isTarget"] = true;
                    })
                    { Label = "Pathfinding" }
                ),
                // Moving along path
                new Action(() =>
                {
                    if (target == null)
                        blackboard["isTarget"] = false;

                    Vector2 vector, direction = path[currentNode] - transform.position;
                    blackboard["direction"] = direction;
                    if (direction.sqrMagnitude > e)
                    {
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            vector = new Vector2(direction.x, 0f);
                        else
                            vector = new Vector2(0f, direction.y);

                        SetMoveVector(vector);
                    }
                    else
                        currentNode++;
                })
                { Label = "Moving" }
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
