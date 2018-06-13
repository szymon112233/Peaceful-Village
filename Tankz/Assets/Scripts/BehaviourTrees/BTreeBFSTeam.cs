using UnityEngine;
using NPBehave;
using System.Collections.Generic;
using System.Linq;

public class BTreeBFSTeam : MonoBehaviour
{
    public TankControllerBTree controller;

    private Blackboard blackboard;
    private Root behaviorTree;
    private Tank myTankComponent, target;
    private List<MapNode> path;
    private int currentNode = 0;
    private const float d = .775f;
    private BFSSearcher bfsSearcher;

    void Start()
    {
        // create our behaviour tree and get it's blackboard
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;
        blackboard["shouldMove"] = true;
        blackboard["isTarget"] = false;
        myTankComponent = GetComponent<Tank>();
        bfsSearcher = GetComponent<BFSSearcher>();
        if (bfsSearcher == null)
            Debug.Log("bfs null");


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
            new Sequence(
                    //new Selector(
                    // Pathfinding
                    //new BlackboardCondition("isTarget", Operator.IS_EQUAL, false, Stops.SELF,
                    new Action(() =>
                    {
                        foreach (Tank tank in GameManager.instance.gamestate.tanksList)
                        {
                            if (tank.team != myTankComponent.team)
                            {
                                target = tank;
                                break;
                            }
                        }


                        foreach (Tank tank in GameManager.instance.gamestate.tanksList.Where(x => x.team != myTankComponent.team))
                            if (tank != myTankComponent)
                                if ((tank.transform.position - transform.position).sqrMagnitude
                                < (target.transform.position - transform.position).sqrMagnitude)
                                    target = tank;

                        path = bfsSearcher.Search(target.Node);
                        blackboard["isTarget"] = true;
                    })
                    { Label = "Pathfinding" },
                //),
                // Moving along path
                new Sequence(
                    new Action(() =>
                    {
                        //if (target == null)
                        //    blackboard["isTarget"] = false;
                                                
                        Vector2 vector, direction = new Vector3(path[currentNode].transform.position.x + 4f,
                                                                path[currentNode].transform.position.y + 4f,
                                                                0f) - transform.position;

                        blackboard["position"] = transform.position;
                        blackboard["direction"] = direction;
                        if (direction.sqrMagnitude > d)
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
                    { Label = "Moving" },
                    new Action(() =>
                    {
                        if (transform.position == (Vector3)blackboard["position"])
                            SetShoot(true);
                        else
                        {
                            Vector2 targetDirection = target.transform.position - transform.position;
                            if (Mathf.Abs(targetDirection.x) <= 4f || Mathf.Abs(targetDirection.y) <= 4f)
                                SetShoot(true);
                            else
                                SetShoot(false);
                        }
                    })
                    { Label = "Shooting" }
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
